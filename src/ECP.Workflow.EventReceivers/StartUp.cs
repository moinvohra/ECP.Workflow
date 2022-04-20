namespace ECP.Workflow.EventReceivers
{
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using ECP.Messages;
    using ECP.Messaging.RabbitMQ;
    using ECP.Messaging.RabbitMQ.Abstractions;
    using ECP.Messaging.RabbitMQ.Connection;
    using ECP.Messaging.RabbitMQ.MessageQueue;
    using ECP.Workflow.EventReceivers.EventHandlers;
    using ECP.Workflow.Repository;
    using ECP.Workflow.Repository.ConnectionProvider;
    using ECP.Workflow.Repository.ProvisionTenantApplications;
    using ECP.Workflow.Repository.ProvisionWorkflowRepository;
    using ECP.Workflow.Service.DeProvisionTenantApplication;
    using ECP.Workflow.Service.ProvisionTenantApplication;
    using ECP.Workflow.Service.ProvisionWorkflowService;
    using HealthChecks.UI.Client;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using RabbitMQ.Client;
    using System;
    using System.Web;
    public class StartUp
    {
        public IConfiguration Configuration { get; }
        
        public StartUp(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            #region HealthCheck
            string rabbitmqconnection = $"amqp://{HttpUtility.UrlEncode(Configuration["RabbitMQ:UserName"])}:{HttpUtility.UrlEncode(Configuration["RabbitMQ:Password"])}@{Configuration["RabbitMQ:HostName"]}:{Configuration["RabbitMQ:Port"]}/{HttpUtility.UrlEncode(Configuration["RabbitMQ:VirtualHost"])}";

            services.AddHealthChecks()
                    .AddNpgSql(Configuration["ConnectionStrings:DefaultConnection"])
                    .AddRabbitMQ(rabbitMQConnectionString: rabbitmqconnection);

            #endregion

            #region ConfigureDb
            services.Configure<ConnectionSettings>(options =>
            {
                options.ConnectionString = Configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
            });
            #endregion


        
            services.AddTransient<IConnectionProvider, ConnectionProvider>();
            services.AddTransient<IProvisionWorkflowService, ProvisionWorkflowService>();
            services.AddTransient<IProvisionWorkflowRepository, ProvisionWorkflowRepository>();

            services.AddTransient<IProvisionTenantApplicationsService, ProvisionTenantApplicationsService>();
            services.AddTransient<IProvisionTenantApplicationsRepository, ProvisionTenantApplicationsRepository>();

            services.AddTransient<IDeProvisionTenantApplicationsService, DeProvisionTenantApplicationsService>();
            services.AddTransient<IDeProvisionTenantApplicationsRepository, DeProvisionTenantApplicationsRepository>();

            int retryCount = 5;
            services.AddSingleton<IMqConnection>(sp =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["RabbitMQ:HostName"],
                    DispatchConsumersAsync = true,
                    Port = Convert.ToInt16(Configuration["RabbitMQ:Port"]),
                    VirtualHost = Configuration["RabbitMQ:VirtualHost"],
                    AutomaticRecoveryEnabled = true,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                    UserName = Configuration["RabbitMQ:UserName"],
                    Password = Configuration["RabbitMQ:Password"],
                    RequestedHeartbeat = 30,
                };

                return new MqConnection(factory);
            });

            RegisterEventBus(services, retryCount);
            var container = new ContainerBuilder();
            container.Populate(services);
            return new AutofacServiceProvider(container.Build());
        }

        public void Configure(IApplicationBuilder app) 
        {
            ConfigureEventBus(app);

            #region Healthcheck
            app.UseHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            #endregion
        }
        private void RegisterEventBus(IServiceCollection services, int rCount)
        {

            services.AddSingleton<IConsumeEventBus, ConsumeMq>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IMqConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<ConsumeMq>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                if (string.IsNullOrEmpty(Configuration["RabbitMQ:RetryCount"]))
                    rCount = 5;

                return new ConsumeMq(rabbitMQPersistentConnection, logger, eventBusSubcriptionsManager, iLifetimeScope, rCount);
            });
            services.AddTransient<WorkflowActivatedEventHandler>();
            services.AddTransient<TenantApplicationAssignedEventHandler>();
            services.AddTransient<TenantApplicationDeAssignedEventHandler>();

            services.AddSingleton<IEventBusSubscriptionsManager, EventBusSubscriptionsManager>();
        }

        public void ConfigureEventBus(IApplicationBuilder app)
        {
          

            if (app != null)
            {
                var eventBus = app.ApplicationServices.GetRequiredService<IConsumeEventBus>();

                eventBus.Subscribe<TenantApplicationAssigned, TenantApplicationAssignedEventHandler>
                 (exchangeName: Configuration["RabbitMQ:TenantApplicationAssignedQueue:ExchangeName"],
                 queueName: Configuration["RabbitMQ:TenantApplicationAssignedQueue:QueueName"],
                 routingKey: Configuration["RabbitMQ:TenantApplicationAssignedQueue:RouteName"]);

                eventBus.Subscribe<WorkflowActivated, WorkflowActivatedEventHandler>
                    (exchangeName: Configuration["RabbitMQ:WorkFlowActivatedQueue:ExchangeName"],
                    queueName: Configuration["RabbitMQ:WorkFlowActivatedQueue:QueueName"],
                    routingKey: Configuration["RabbitMQ:WorkFlowActivatedQueue:RouteName"]);

                eventBus.Subscribe<TenantApplicationDeAssigned, TenantApplicationDeAssignedEventHandler>
                    (exchangeName: Configuration["RabbitMQ:TenantApplicationDeAssignedQueue:ExchangeName"],
                    queueName: Configuration["RabbitMQ:TenantApplicationDeAssignedQueue:QueueName"],
                    routingKey: Configuration["RabbitMQ:TenantApplicationDeAssignedQueue:RouteName"]);
            }
        }
    }
}

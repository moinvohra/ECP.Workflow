using Autofac;
using Autofac.Extensions.DependencyInjection;
using ECP.Messaging.RabbitMQ.Abstractions;
using ECP.Messaging.RabbitMQ.Connection;
using ECP.workflow.Repository;
using ECP.Workflow.EventPublisher;
using ECP.Workflow.Repository;
using ECP.Workflow.Repository.ConnectionProvider;
using ECP.Workflow.Service;
using ECP.Workflow.Service.MessageBroker;
using EventBus.MessageQueue;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Serilog;
using System;
using System.IO;
using System.Web;

namespace ECP.Notifications.EventPublisher
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            #region ConfigureDb
            services.Configure<ConnectionSettings>(options =>
            {
                options.ConnectionString = Configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
            });
            #endregion

            services.AddTransient<IConnectionProvider, ConnectionProvider>();

            services.AddTransient<IOutboundTransactionQueueService, OutboundTransactionQueueService>();

            services.AddTransient<IOutboundTransactionQueueRepository, OutboundTransactionQueueRepository>();

            services.AddTransient<IMessageBroker, MessageBroker>();

            services.AddHostedService<WorkflowEventPublisher>();

            #region HealthCheck
            string rabbitmqconnection = $"amqp://{HttpUtility.UrlEncode(Configuration["RabbitMQ:UserName"])}:{HttpUtility.UrlEncode(Configuration["RabbitMQ:Password"])}@{Configuration["RabbitMQ:HostName"]}:{Configuration["RabbitMQ:Port"]}/{HttpUtility.UrlEncode(Configuration["RabbitMQ:VirtualHost"])}";

            services.AddHealthChecks()
                    .AddNpgSql(Configuration["ConnectionStrings:DefaultConnection"])
                    .AddRabbitMQ(rabbitMQConnectionString: rabbitmqconnection);
            #endregion


            #region MessageBroker
            services.AddSingleton<IMqConnection>(sp =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["RabbitMQ:HostName"],
                    DispatchConsumersAsync = true,
                    VirtualHost = Configuration["RabbitMQ:VirtualHost"],
                    Port = Convert.ToInt16(Configuration["RabbitMQ:Port"]),
                    RequestedHeartbeat = 60
                };

                if (!string.IsNullOrEmpty(Configuration["RabbitMQ:UserName"]))
                    factory.UserName = Configuration["RabbitMQ:UserName"];

                if (!string.IsNullOrEmpty(Configuration["RabbitMQ:Password"]))
                    factory.Password = Configuration["RabbitMQ:Password"];

                return new MqConnection(factory);
            });

            //Initialize Queue
            RegisterEventBus(services);
            #endregion

            var container = new ContainerBuilder();
            container.Populate(services);

            return new AutofacServiceProvider(container.Build());
        }

        public void RegisterEventBus(IServiceCollection services)
        {
            services.AddSingleton<IPublishEventBus, PublishMq>(sp =>
            {
                var mqConnection = sp.GetRequiredService<IMqConnection>();
                var logger = sp.GetRequiredService<ILogger<PublishMq>>();

                var retryCount = 5;
                if (!string.IsNullOrEmpty(Configuration["RabbitMQ:RetryCount"]))
                    retryCount = int.Parse(Configuration["RabbitMQ:RetryCount"]);

                return new PublishMq(mqConnection, logger, retryCount);
            });
        }
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            #region Healthcheck
            app.UseHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            #endregion
        }


    }
}

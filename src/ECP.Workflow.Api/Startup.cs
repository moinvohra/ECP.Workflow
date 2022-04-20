using System;
using System.IO;
using System.Net.Http;
using System.Web;
using Dapper;
using ECP.KendoGridFilter;
using ECP.Messaging.RabbitMQ.Abstractions;
using ECP.Messaging.RabbitMQ.Connection;
using ECP.workflow.Repository;
using ECP.Workflow.Api.Validation;
using ECP.Workflow.Common;
using ECP.Workflow.Model;
using ECP.Workflow.Repository;
using ECP.Workflow.Repository.ConnectionProvider;
using ECP.Workflow.Repository.DBContext;
using ECP.Workflow.Repository.Handlers;
using ECP.Workflow.Repository.Query;
using ECP.Workflow.Repository.WorkflowRepository;
using ECP.Workflow.Service;
using ECP.Workflow.Service.MessageBroker;
using ECP.Workflow.Service.Model.Utility;
using ECP.Workflow.Service.Validation;
using EventBus.MessageQueue;
using FluentValidation;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Serilog;

namespace ECP.Workflow.Api
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false)
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = c =>
                    {
                        return new ValidationFailedResult(c.ModelState);
                      
                    };
                })
                .AddFluentValidation()
                .AddNewtonsoftJson();

            #region ConfigureDb
            services.Configure<ConnectionSettings>(options =>
            {
                options.ConnectionString = Configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
            });
            #endregion


            #region AddDBContext
            services.AddDbContext<WorkflowDbContext>(options =>
                options.UseNpgsql(Configuration["ConnectionStrings:DefaultConnection"])

                );
            #endregion


            SqlMapper.AddTypeHandler(new DateTimeHandler());

            #region FluentValidation
            services.AddSingleton<IValidator<AddWorkflowReq>, AddWorkflowReqValidator>();
            services.AddSingleton<IValidator<EditWorkflowReq>, EditWorkflowReqValidator>();
            services.AddSingleton<IValidator<DataSourceRequest>, DataSourceRequestValidator>();



            #endregion


            #region IdentityImplementation
            ConfigureAuth(services);
            #endregion

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiReader", policy => policy.RequireClaim("scope",
                    Configuration["IdentityServer:ApiName"]));
            });

            ConfigureCors(services);

            #region HealthCheck
            Uri uri1 = new Uri(Configuration["IdentityServer:IdentityURL"]);
            string rabbitmqconnection = $"amqp://{HttpUtility.UrlEncode(Configuration["RabbitMQ:UserName"])}:{HttpUtility.UrlEncode(Configuration["RabbitMQ:Password"])}@{Configuration["RabbitMQ:HostName"]}:{Configuration["RabbitMQ:Port"]}/{HttpUtility.UrlEncode(Configuration["RabbitMQ:VirtualHost"])}";

            services.AddHealthChecks()
                                .AddIdentityServer(uri1)
                               .AddNpgSql(Configuration["ConnectionStrings:DefaultConnection"])
                               .AddRabbitMQ(rabbitConnectionString: rabbitmqconnection);

            #endregion

            #region Swagger Configuration
            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Workflow";
                    document.Info.Description = "A simple ASP.NET Core web API performing CRUD operation on Workflow";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "ecp-workflow",
                        Email = "ecp@dovetail.com",
                        Url = ""
                    };
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = "https://example.com/license"
                    };
                };
            });
            #endregion

            #region RegisterServices
            services.AddTransient<IConnectionProvider, ConnectionProvider>();
            services.AddHttpClient<HttpClient>();
            services.AddTransient<IAuthorizationApiClient, AuthorizationApiClient>();

            services.AddTransient<IWorkflowRepository, WorkflowRepository>();
            services.AddTransient<IWorkflowService, WorkflowService>();


            services.AddTransient<IWorkflowSearchRepository, WorkFlowSearchRepository>();
            services.AddTransient<IOutboundTransactionQueueRepository, OutboundTransactionQueueRepository>();
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

            services.AddTransient<IMessageBroker, MessageBroker>();
            #endregion

            services.AddTransient<IGetIpAddress, WebHelper>();
        }

        public virtual void ConfigureAuth(IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
              .AddIdentityServerAuthentication(options =>
              {

                  options.Authority = Configuration["IdentityServer:IdentityUrl"];
                  options.RequireHttpsMetadata = Convert.ToBoolean(Configuration["IdentityServer:HTTPMetadata"]);
                  options.ApiName = Configuration["IdentityServer:ApiName"];
              });
        }

        /// <summary>
        ///  Configures Cross Origin Requests
        /// </summary>
        /// <param name="services">Collection of services</param>
        public void ConfigureCors(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
           

            app.UsePathBase("/workflow");

            #region Identity
            app.UseAuthentication();
            #endregion

            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseCors("CorsPolicy");

            app.ConfigureCustomExceptionMiddleware();
            app.UseMvc();

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

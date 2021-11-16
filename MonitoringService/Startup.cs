using System;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MonitoringService.Configurations;
using MonitoringService.Stream;
using static MonitoringService.AssemblyVersionHelpers;
using static MonitoringService.Configurations.Constants.Configuration;


namespace MonitoringService
{
    public class Startup
    {
        public Startup(IConfiguration configuration,
            IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        private IConfiguration Configuration { get; }
        private IHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var settings = Configuration.GetSection(nameof(Settings)).Get<Settings>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //todo: Add MiniProfiler, setup for swagger as well
            // services.AddMiniProfiler(options =>
            // {
            //     options.RouteBasePath = "/profiler";
            //     options.PopupRenderPosition = StackExchange.Profiling.RenderPosition.BottomLeft;
            //     options.PopupShowTimeWithChildren = true;
            // });
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();

            // Add CORS allowed domains  
            services.AddCors(options =>
            {
                options.AddPolicy(Cors.NotificationsApiPolicy,
                    builder =>
                    {
                        builder
                            .WithOrigins(settings.NotificationsAppUrl)
                            .AllowCredentials()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
                options.AddPolicy(Cors.NotificationsStreamPolicy,
                    builder =>
                    {
                        builder
                            .WithOrigins(settings.NotificationsAppUrl)
                            .AllowCredentials()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
                options.AddPolicy(Cors.HealthChecksPolicy,
                    builder =>
                    {
                        builder
                            // .WithOrigins(/*<allowed-origins>*/)
                            // .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });

            // All lowercase routes  
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddControllers();

            //todo: add and setup later.
            // services.AddAuthentication()
            //     .AddIdentityServerJwt();
            // services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>());
            // services.AddAuthorization();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = OpenApi.NotificationsApiTitle,
                        Version = GetAppVersion()
                    });
            });
            services.Configure<Settings>(Configuration.GetSection(nameof(Settings)));
            services.AddScoped(serviceProvider => serviceProvider.GetService<IOptionsSnapshot<Settings>>()?.Value!);
            services.AddSignalR(config =>
            {
                if (Environment.IsDevelopment())
                {
                    config.EnableDetailedErrors = true;
                }
            });

            //todo: add and setup later.
            services.AddSingleton<IUserIdProvider, SignalRUserProvider>();
            services.AddMediatR(typeof(Startup));
            var healthChecks = services.AddHealthChecks();
            foreach (var database in settings.HealthChecks.Databases)
            {
                healthChecks
                    .AddSqlServer(database.ConnectionString,
                        name: database.Name);
            }

            healthChecks.AddRedis(settings.HealthChecks.RedisConnectionString,
                HealthCheck.Redis);
            healthChecks.AddRabbitMQ(new Uri(settings.HealthChecks.RabbitMqConnectionString),
                name: "RabbitMQ");
            healthChecks.AddUrlGroup(new Uri(settings.HealthChecks.SeqUri),
                "Seq");
            healthChecks.AddElasticsearch(s =>
                {
                    s.UseServer(settings.HealthChecks.ElasticSearchUri);
                    s.UseBasicAuthentication(settings.HealthChecks.ElasticSearchUsername,
                        settings.HealthChecks.ElasticSearchPassword);
                },
                "Elastic");
            healthChecks.AddSignalRHub(settings.HealthChecks.NotificationsStreamUrl);

            // services.AddHealthChecksUI(opt =>
            //     {
            //         // opt.SetEvaluationTimeInSeconds(15); //time in seconds between check
            //         // opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks
            //         opt.SetApiMaxActiveRequests(1); //api requests concurrency
            //
            //         opt.AddHealthCheckEndpoint(HealthCheck.ApiName,
            //             HealthCheck.ApiUrl); //map health check api
            //     })
            //     .AddInMemoryStorage();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //todo: Add MiniProfiler, setup for swagger as well
                // app.UseMiniProfiler();
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    // c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("MonitoringService.index.html");
                    c.SwaggerEndpoint("/swagger/v1/swagger.json",
                        OpenApi.NotificationsApiName);
                    c.RoutePrefix = string.Empty;
                    c.DisplayRequestDuration();
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();
            // app.UseAuthorization();

            // Any connection or hub wire up and configuration should go here
            // SEEMS NOT TO BE TRIGGERED in ASP.NET Core
            // GlobalHost.HubPipeline.AddModule(new ErrorHandlingPipelineModule());
            // GlobalHost.HubPipeline.AddModule(new LoggingPipelineModule());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>(AppRoutes.Stream.Notifications)
                    .RequireCors(Cors.NotificationsStreamPolicy);
                endpoints.MapHealthChecks(HealthCheck.ApiUrl,
                        new HealthCheckOptions
                        {
                            Predicate = _ => true,
                            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                        })
                    .RequireCors(Cors.HealthChecksPolicy)
                    // .RequireAuthorization();
                    ;
                // endpoints.MapHealthChecksUI(o =>
                // {
                //     o.UIPath = HealthCheck.UIPath;
                // });
            });
        }
    }
}
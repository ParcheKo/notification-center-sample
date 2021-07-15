using MediatR;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using IUserIdProvider = Microsoft.AspNetCore.SignalR.IUserIdProvider;

namespace MonitoringService
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        private IConfiguration Configuration { get; }
        private IHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var settings = Configuration.GetSection("Settings").Get<Settings>();

            // Add CORS allowed domains  
            services.AddCors(options =>
            {
                options.AddPolicy("NotificationsCorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins(settings.NotificationCenterAppUrl)
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
            
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Notification Center Sample", Version = "v1"}); });
            services.Configure<Settings>(Configuration.GetSection(nameof(Settings)));
            services.AddScoped(cfg => cfg.GetService<IOptionsSnapshot<Settings>>()?.Value);
            services.AddSignalR(config =>
            {
                if (Environment.IsDevelopment())
                {
                    config.EnableDetailedErrors = true;
                }
            });
            
            //todo: add and setup later.
            services.AddSingleton<IUserIdProvider, NameUserIdProvider>();
            services.AddMediatR(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication v1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseCors("NotificationsCorsPolicy");

            app.UseHttpsRedirection();

            app.UseRouting();

            // app.UseAuthorization();

            // Any connection or hub wire up and configuration should go here
            GlobalHost.HubPipeline.AddModule(new ErrorHandlingPipelineModule());
            GlobalHost.HubPipeline.AddModule(new LoggingPipelineModule());
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>(NotificationHubRoutes.Notifications);
            });
        }
    }
}
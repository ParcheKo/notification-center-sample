using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MonitoringService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext,
                    services) =>
                {
                    services.AddHostedService<MonitoringService>();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseKestrel(options =>
                    {
                        // TCP 8007
                        // options.ListenLocalhost(8007, builder => { builder.UseConnectionHandler<TestConnectionHandler>(); });

                        // HTTP 5000
                        options.ListenLocalhost(5050);

                        // HTTPS 5001
                        options.ListenLocalhost(5051, builder => { builder.UseHttps(); });
                    });
                })
                .ConfigureWebHost(config => { config.UseUrls("http://*:5050;http://*:5051"); })
                .UseWindowsService(cfg => { cfg.ServiceName = "Notification Center Service"; });
    }
}
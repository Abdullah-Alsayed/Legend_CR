using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using DicomApp.DAL.DB;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DicomApp.Portal
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
                XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

                var host = BuildWebHost(args);
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var context = services.GetRequiredService<ShippingDBContext>();
                    if (context != null)
                        await DataSeeder.Seed(context);
                    else
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(
                            "ShippingDBContext could not be resolved from the service provider."
                        );
                    }
                }

                await host.RunAsync();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().Build();
    }
}

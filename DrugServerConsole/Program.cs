using System;
using FirstDataBank.DrugServer.API.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;
using DrugServerConsole.Services;
using DrugServerConsole.Utilities;
using FirstDataBank.DrugServer.API;

namespace DrugServerConsole
{
    public class Program
    {
        public static IServiceProvider ServiceProvider { get; set; }

        private static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().RunAsync();

            var productService = ServiceProvider.GetRequiredService<IProductSearchService>();

            productService.Execute();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                {
                    builder
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false)
                        .AddJsonFile($"appsettings.Development.json", true, true)
                        .AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddDrugServerApi(settings =>
                    {
                        //Get Settings from Config File
                        settings.ConfigurationSourceSettings = context.Configuration.GetSection("FDBDrugServerSettings");
                    });
                    services.AddSingleton<IDrugSystem>(serviceProvider =>
                    {
                        var factory = serviceProvider.GetRequiredService<IDrugSystemFactory>();
                        return factory.CreateSystem();
                    });
                    services.AddTransient<IProductSearchService, ProductSearchService>();
                    services.AddTransient<IProductUtility, ProductUtility>();

                    ServiceProvider = services.BuildServiceProvider();
                });
        }
    }
}

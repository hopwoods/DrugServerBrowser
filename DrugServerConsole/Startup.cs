using System;
using FirstDataBank.DrugServer.API.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DrugServerConsole
{
    internal static class Startup
    {
        public static IConfiguration Configuration { get; set; }
        public static IServiceProvider ServiceProvider;
        private static readonly IServiceCollection Services = new ServiceCollection();

        public static void BuildConfiguration()
        {
            var builder = new ConfigurationBuilder();
            Configuration = builder.AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.Development.json", true, true)
                .AddEnvironmentVariables()
                .Build();
        }

        public static void ConfigureServices()
        {
            ServiceProvider = Services.AddDrugServerApi(settings =>
            {
                //Get Settings from Config File
                settings.ConfigurationSourceSettings = Configuration.GetSection("FDBDrugServerSettings"); 

            }).BuildServiceProvider();
            
        }
    }
}

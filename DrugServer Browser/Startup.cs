using DrugServer_Browser.DrugSystem;
using FirstDataBank.DrugServer.API.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;

namespace DrugServer_Browser
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }
        private IServiceCollection _services;
        private ILogger<Startup> _logger;

        private IWebHostEnvironment Env { get; set; }

        public Startup(IWebHostEnvironment env)
        {
            Env = env;

            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Env.ContentRootPath);
            builder.AddJsonFile("appsettings.json", false, true);
            builder.AddJsonFile($"appsettings.{Env.EnvironmentName}.json", true, true);

            if (Env.IsDevelopment())
                builder.AddUserSecrets<Startup>();

            builder.AddEnvironmentVariables(); // Must be loaded last to avoid issues when deployed to Azure.
            Configuration = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _services = services;

            services
                .AddRazorPages()
                .AddRazorRuntimeCompilation();

            services.AddLogging(config =>
            {
                config.AddDebug();
                config.AddConsole();
                //etc
            });

            services.AddDrugServerApi(settings =>
            {
                settings.ConfigurationSourceSettings = Configuration.GetSection("FDBDrugServerSettings");
                //settings.ManualSettings = new ApiSettings()
                //{
                //    DefaultLanguage = Language.English,
                //    DefaultRegion = Region.England,
                //    ConnectionStrings = new ConnectionStringSettingsCollection()
                //    {
                //        new ConnectionStringSettings(
                //            "DotNetTest",
                //            "Data Source=(Local);Initial Catalog=DatabaseName;Integrated Security=True",
                //            "System.Data.SqlClient")
                //    },
                //    DatabaseConnectionName = "DotNetTest",
                //    DatabaseRetrySleepInMilliseconds = 1000,
                //    DatabaseMaxRetryCount = 3
                //};
            });
            services.AddSingleton<IBrowserDrugSystem, BrowserDrugSystem>();
        }
        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            _logger = logger;

            foreach (ServiceDescriptor service in _services)
            {
                _logger.LogInformation(
                    $"Service: {service.ServiceType.FullName}\n Lifetime: {service.Lifetime}\n Instance: {service.ImplementationType?.FullName}");
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            app.Run(async context =>
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<h1>All Services</h1>");
                sb.Append("<table><thead>");
                sb.Append("<tr><th>Type</th><th>Lifetime</th><th>Instance</th></tr>");
                sb.Append("</thead><tbody>");
                foreach (ServiceDescriptor svc in _services)
                {
                    sb.Append("<tr>");
                    sb.Append($"<td>{svc.ServiceType.FullName}</td>");
                    sb.Append($"<td>{svc.Lifetime}</td>");
                    sb.Append($"<td>{svc.ImplementationType?.FullName}</td>");
                    sb.Append("</tr>");
                }

                sb.Append("</tbody></table>");
                await context.Response.WriteAsync(sb.ToString());
            });
        }
    }
}

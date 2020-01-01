using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Swashbuckle.AspNetCore.Swagger;

namespace BankBros.Backend.GatewayAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Run();
        }

        public static IWebHost CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel()
                .UseUrls("https://*:*")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile("otest.json",false,true)
                        .AddEnvironmentVariables();
                })
                .ConfigureServices(s =>
                {
                    s.AddOcelot().WithConfigurationRepository();
                    s.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
                    s.AddSwaggerGen(c =>
                    {
                        c.SwaggerDoc("v1", new Info { Title = "GW", Version = "v1" });
                    });
                })
                .UseIISIntegration()
                .Configure(a =>
                {
                    a.UseMvc().UseSwagger().UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/banking/swagger.json", "BankingAPI");
                        c.SwaggerEndpoint("/billing/swagger.json", "BillingAPI");
                    });

                    a.UseOcelotExtended().Wait();
                }).Build();
    }
}

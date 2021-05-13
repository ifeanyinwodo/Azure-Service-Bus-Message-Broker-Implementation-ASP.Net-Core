using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Workerservice_Consumer.Models;

namespace Workerservice_Consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static KestrelOptions MongoDBURL(string[] args)
        {

            var config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
           .AddCommandLine(args)
           .Build();

            var options = new KestrelOptions();
            var section = config.GetSection("KestrelOptions");
            section.Bind(options);
            return options;
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                   .UseMetrics(options => {
                       options.EndpointOptions = endpointsOptions =>
                       {
                           endpointsOptions.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
                           endpointsOptions.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
                           endpointsOptions.EnvironmentInfoEndpointEnabled = false;
                       };
                   })
                 .UseMetricsWebTracking()
                 /*.ConfigureAppMetricsHostingConfiguration(options =>
                 {
                     options.AllEndpointsPort = 9119;
                     options.MetricsEndpoint = "app-metrics";
                 })*/
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                   .UseUrls(MongoDBURL(args).Url);
                });
    }
}

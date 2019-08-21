using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace ServiceTemplate
{
    class Program
    {    
        static async Task Main(string[] args)
        {
            await new HostBuilder()
			
				// Setup your DI here
                .ConfigureServices((hostContext, services) =>
                {
                    // Register stuff

                    // Register the service
                    services.AddHostedService<MyService>();               
                })
				
				// Read the config file in here
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {							
                    configApp
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", reloadOnChange: true, optional: true);
                })
				
				// Configure the logger here
                .ConfigureLogging((hostContext, configLogging) =>
                {					
                    Log.Logger = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .Enrich.WithProperty("Application", hostContext.Configuration.GetSection("ElasticConfiguration:Application")?.Value)
                        .Enrich.WithProperty("FriendlyName", System.AppDomain.CurrentDomain.FriendlyName)
                        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(hostContext.Configuration.GetSection("ElasticConfiguration:Uri")?.Value))
                        {
                            AutoRegisterTemplate = true,
                        })
                        .WriteTo.Console()
                        .WriteTo.File($"{hostContext.Configuration.GetSection("Serilog:LogRoot")?.Value}log-.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger();

                    configLogging.AddSerilog();			
                })
				
				// Run the service
                .RunConsoleAsync();
        }    
    }
}



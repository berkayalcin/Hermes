using System;
using System.Linq;
using Hermes.API.Catalog.Domain.Constants;
using Hermes.API.Catalog.Domain.Data;
using Hermes.API.Catalog.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;

namespace Hermes.API.Catalog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().MigrateDatabase<HermesDbContext>().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureLogging(l =>
                    {
                        l.ClearProviders();
                        l.AddConsole();
                        l.SetMinimumLevel(LogLevel.Information);
                    });

                    webBuilder.UseSerilog((context, configuration) =>
                    {
                        var hostUrls = context.Configuration[ConfigConstants.ElasticSearchUrl].Split(",");
                        var elasticSearchUrls = hostUrls
                            .Select(u => new Uri(u));
                        configuration.Enrich.FromLogContext()
                            .WriteTo.Console(new ElasticsearchJsonFormatter())
                            .MinimumLevel.Override(ConfigConstants.MicrosoftLogsSourceName, LogEventLevel.Warning)
                            .WriteTo.Elasticsearch(
                                new ElasticsearchSinkOptions(
                                    elasticSearchUrls)
                                {
                                    IndexFormat = ConfigConstants.ElasticSearchLogsIndexFormat,
                                    NumberOfReplicas = 2,
                                    NumberOfShards = 2
                                })
                            .Enrich.WithProperty(ConfigConstants.SerilogApplicationPropertyName,
                                ConfigConstants.ApplicationName)
                            .ReadFrom.Configuration(context.Configuration);
                    });

                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls(Environment.GetEnvironmentVariable("SERVICE_URL")!);
                });
    }
}
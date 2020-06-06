namespace EventSourcing.WriteModel
{
    using System;
    using System.IO;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using Serilog.Filters;
    using Serilog.Sinks.Elasticsearch;

    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
                .CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .ConfigureAppConfiguration((hostBuilder, config) =>
                    {
                        config
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables();
                    })
                    .UseStartup<Startup>()
                    .UseSerilog((hostingContext, loggerConfiguration) =>
                    {
                        var connectionString = hostingContext.Configuration["App:elasticSearch:ConnectionString"];
                        var port = hostingContext.Configuration["App:elasticSearch:Port"];
                        var uri = new Uri($"http://{connectionString}:{port}");

                        loggerConfiguration
                            .ReadFrom.Configuration(hostingContext.Configuration)
                            .WriteTo.Async(
                                a => a.Elasticsearch(
                                new ElasticsearchSinkOptions(uri)
                                {
                                    AutoRegisterTemplate = true,
                                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6
                                }).Filter.ByExcluding(Matching.FromSource("Microsoft")))
                            .WriteTo.Async(a => a.Console());
                    })
                    ;
                });

            //return new WebHostBuilder()
            //    .UseKestrel()
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .ConfigureAppConfiguration((hostBuilder, config) =>
            //    {
            //        config
            //            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //            .AddEnvironmentVariables();
            //    })
            //    .UseStartup<Startup>()
            //    .UseSerilog((hostingContext, loggerConfiguration) =>
            //    {
            //        var connectionString = hostingContext.Configuration["App:elasticSearch:ConnectionString"];
            //        var port = hostingContext.Configuration["App:elasticSearch:Port"];
            //        var uri = new Uri($"http://{connectionString}:{port}");

            //        loggerConfiguration
            //            .ReadFrom.Configuration(hostingContext.Configuration)
            //            .WriteTo.Async(
            //                a => a.Elasticsearch(
            //                new ElasticsearchSinkOptions(uri)
            //                {
            //                    AutoRegisterTemplate = true,
            //                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6
            //                }).Filter.ByExcluding(Matching.FromSource("Microsoft")))
            //            .WriteTo.Async(a => a.Console());
            //    })
            //    .Build();
        }
    }
}
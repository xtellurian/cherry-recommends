using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SignalBox.Infrastructure;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using SignalBox.Infrastructure.Services;
using System.IO;
using System;
using SignalBox.Infrastructure.Files;
using SignalBox.Functions.Services;

namespace SignalBox.Functions
{
    public class Program
    {

        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddJsonFile("local.settings.json", optional: true);
                    builder.AddEnvironmentVariables();
                })
                .ConfigureServices(services =>
                {
                    var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
                    var provider = configuration.GetValue("Provider", "sqlserver");
                    var azureWebJobsConnectionString = configuration.GetValue<string>("AzureWebJobsStorage");
                    if (string.IsNullOrEmpty(azureWebJobsConnectionString))
                    {
                        throw new Exception("AzureWebJobsStorage connection string missing");
                    }
                    System.Console.WriteLine($"Database Provider: {provider}");
                    if (provider == "sqlserver")
                    {
                        services.UseSqlServer(configuration.GetConnectionString("Application"));
                    }
                    else if (provider == "sqlite")
                    {
                        // fix the relative path to the sqlite directory
                        var cs = configuration.GetConnectionString("Application")
                            .Replace("{AppDir}", Directory.GetCurrentDirectory())
                            .Replace("dotnetFunctions/bin/output/", "");
                        services.UseSqlLite(cs);
                    }

                    services.RegisterCoreServices();
                    services.RegisterDefaultInfrastructureServices();

                    services.AddScoped<ITelemetry, AzureFunctionsAppInsightsTelemetry>();

                    services.Configure<QueueMessagesFileHosting>(_ =>
                    {
                        _.ConnectionString = azureWebJobsConnectionString;
                        _.ContainerName = "queue-messages";

                    });
                    services.AddScoped<IQueueMessagesFileStore, AzureBlobQueueMessagesFileStore>();

                    services.AddAzureStorageQueueStores();
                    services.AddEFStores();
                    // add the workflows needed for backend processing
                    services.RegisterWorkflows();
                    // add some required services
                })
                .Build();

            host.Run();
        }
    }
}
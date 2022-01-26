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
using SignalBox.Core.Integrations;
using SignalBox.Infrastructure.Azure;
using SignalBox.Infrastructure.Sqlite;
using SignalBox.Infrastructure.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SignalBox.Functions
{
    public class Program
    {

        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(workerApplication =>
                {
                    workerApplication.UseMiddleware<TenantSelectorFunctionsMiddleware>();
                })
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddJsonFile("local.settings.json", optional: true);
                    builder.AddEnvironmentVariables();
                })
                .ConfigureServices(services =>
                {
                    var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
                    var provider = configuration.GetValue("Provider", "sqlserver");
                    provider.ValidateDbProvider();
                    System.Console.WriteLine($"Database Provider: {provider}");

                    var useMulti = configuration.GetSection("Hosting").GetValue<bool>("Multitenant", false);

                    var sqliteConfig = AutomagicFixSqliteDatabasePath(provider, configuration);

                    if (useMulti)
                    {
                        services.RegisterMultiTenantInfrastructure();

                        // multi tenant
                        if (provider == "sqlserver")
                        {
                            services.UseSqlServer<MultiTenantDbContext>(configuration.GetConnectionString("Tenants"));
                            var azEnv = new SignalBoxAzureEnvironment();
                            configuration.GetSection("AzureEnvironment").Bind(azEnv);
                            var localEnv = new SqlServerCredentials();
                            configuration.GetSection("LocalSql").Bind(localEnv);

                            if (!string.IsNullOrEmpty(azEnv.SqlServerPassword))
                            {
                                services.UseMultitenantAzureSqlServer(
                                    azEnv.SqlServerName,
                                    azEnv.SqlServerPassword,
                                    azEnv.SqlServerUserName);
                            }
                            else
                            {
                                services.UseMultitenantLocalSqlServer(localEnv.SqlServerPassword, localEnv.SqlServerUserName);
                            }
                        }
                        else if (provider == "sqlite")
                        {
                            // the multitenant once works for single tenants too now
                            services.UseSqlite<MultiTenantDbContext>(configuration.GetConnectionString("Tenants").FixSqliteConnectionString());
                            services.UseMultitenantSqlite("databases", dbConfig: sqliteConfig, manipulateConnectionString: _ => _.FixSqliteConnectionString());
                        }
                        else
                        {
                            throw new ConfigurationException("Provider must be sqlite or sqlserver");
                        }
                    }
                    else
                    {
                        services.RegisterSingleTenantInfrastructure();
                        // single tenant
                        if (provider == "sqlserver")
                        {
                            var azEnv = new SignalBoxAzureEnvironment();
                            configuration.GetSection("AzureEnvironment").Bind(azEnv);
                            var localEnv = new SqlServerCredentials();
                            configuration.GetSection("LocalSql").Bind(localEnv);

                            if (!string.IsNullOrEmpty(azEnv.SqlServerPassword))
                            {
                                services.UseMultitenantAzureSqlServer(
                                    azEnv.SqlServerName,
                                    azEnv.SqlServerPassword,
                                    azEnv.SqlServerUserName);
                            }
                            else
                            {
                                services.UseMultitenantLocalSqlServer(localEnv.SqlServerPassword, localEnv.SqlServerUserName);
                            }
                        }
                        else if (provider == "sqlite")
                        {
                            // the multitenant once works for single tenants too now
                            services.UseMultitenantSqlite("databases", dbConfig: sqliteConfig, manipulateConnectionString: _ => _.FixSqliteConnectionString());
                        }
                        else
                        {
                            throw new ConfigurationException("Provider must be sqlite or sqlserver");
                        }
                    }

                    services.RegisterCoreServices();
                    services.RegisterDefaultInfrastructureServices();

                    services.AddScoped<ITelemetry, AzureFunctionsAppInsightsTelemetry>();
                    var azureEnvironmentConfigSection = configuration.GetSection("AzureEnvironment");

                    if (provider == "sqlite")
                    {
                        services.AddScoped<IDatabaseManager, SqliteDatabaseManager>();
                    }
                    else if (provider == "sqlserver")
                    {
                        if (string.IsNullOrEmpty(azureEnvironmentConfigSection.GetValue<string>("SqlServerAzureId")))
                        {
                            throw new NullReferenceException("SqlServerAzureId is null");
                        }
                        services.Configure<SignalBoxAzureEnvironment>(azureEnvironmentConfigSection);
                        services.AddScoped<IDatabaseManager, AzureSqlDatabaseManager>();
                    }
                    else
                    {
                        throw new ConfigurationException("No valid provider");
                    }

                    services.Configure<JsonSerializerOptions>(options => 
                    {
                        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                        options.Converters.Add(new JsonStringEnumConverter());
                    });

                    services.AddScoped<IQueueMessagesFileStore, AzureBlobQueueMessagesFileStore>();
                    services.AddScoped<IEnvironmentProvider, AzFunctionEnvironmentProvider>();

                    ConfigureAppSettings(services, configuration, sqliteConfig);

                    services.AddAzureStorageQueueStores();
                    services.AddEFStores();
                    // add the workflows needed for backend processing
                    services.RegisterWorkflows();
                    // add some required services
                })
                .Build();

            host.Run();
        }

        private static void ConfigureAppSettings(IServiceCollection services,
                                                 IConfiguration configuration,
                                                 SqliteDatabasesConfig sqliteConfig)
        {
            var azureWebJobsConnectionString = configuration.GetValue<string>("AzureWebJobsStorage");
            if (string.IsNullOrEmpty(azureWebJobsConnectionString))
            {
                throw new Exception("AzureWebJobsStorage connection string missing");
            }

            services.Configure<QueueMessagesFileHosting>(_ =>
            {
                _.ConnectionString = azureWebJobsConnectionString;
                _.ContainerName = "queue-messages";

            });


            services.Configure<SqliteDatabasesConfig>(_ =>
            {
                _.AppDir = sqliteConfig?.AppDir;
                _.Directory = sqliteConfig?.Directory;
            });

            services.Configure<HubspotAppCredentials>(configuration.GetSection("HubSpot").GetSection("AppCredentials"));
            services.Configure<Auth0ManagementCredentials>(configuration.GetSection("Auth0").GetSection("Management"));
            services.Configure<Auth0M2MClient>(configuration.GetSection("Auth0").GetSection("M2M"));
            services.Configure<Hosting>(configuration.GetSection("Hosting"));
        }

        private static SqliteDatabasesConfig AutomagicFixSqliteDatabasePath(string provider, IConfiguration configuration)
        {
            if (provider == "sqlite")
            {
                var sqliteConfig = new SqliteDatabasesConfig();
                configuration.Bind("Sqlite", sqliteConfig);
                sqliteConfig.AppDir = Directory.GetCurrentDirectory().Replace("dotnetFunctions/bin/output/", "");
                return sqliteConfig;
            }
            else
            {
                return null;
            }
        }
    }
}
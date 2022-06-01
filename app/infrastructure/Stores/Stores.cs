using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Infrastructure.EntityFramework;
using SignalBox.Infrastructure.Queues;

namespace SignalBox.Infrastructure
{
    public static class Stores
    {
        public static IServiceCollection UseSqlServer<T>(this IServiceCollection services,
                                                    string connectionString,
                                                    string migrationAssembly = "sqlserver") where T : DbContext
        {
            // this is scoped
            services.AddDbContext<T>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationAssembly).CommandTimeout(180)));
            return services;
        }
        public static IServiceCollection UseMultitenantAzureSqlServer(this IServiceCollection services,
                                                    string serverName,
                                                    string sqlServerPassword,
                                                    string sqlServerUserName,
                                                    int maxPoolSize = 50,
                                                    string migrationAssembly = "sqlserver")
        {
            System.Console.WriteLine($"SQL Server Name: {serverName}");
            // this is scoped
            services.AddDbContextFactory<SignalBoxDbContext>((sp, options) =>
                {
                    var tenantProvider = sp.GetRequiredService<ITenantProvider>();
                    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                    var logger = loggerFactory.CreateLogger("DbFactory");
                    logger.LogInformation("Database Name: {dbName}", tenantProvider.CurrentDatabaseName);
                    logger.LogInformation("Azure SQL Server Username: {username}", sqlServerUserName);
                    var cs = SqlServerConnectionStringFactory.GenerateAzureSqlConnectionString(
                        serverName,
                        tenantProvider.CurrentDatabaseName,
                        sqlServerUserName: sqlServerUserName,
                        sqlServerPassword: sqlServerPassword,
                        maxPoolSize);
                    options.UseSqlServer(cs, b => b.MigrationsAssembly(migrationAssembly).CommandTimeout(180));
                }, ServiceLifetime.Scoped);
            return services;
        }

        public static IServiceCollection UseMultitenantLocalSqlServer(this IServiceCollection services,
                                                    string sqlServerPassword,
                                                    string sqlServerUserName,
                                                    string migrationAssembly = "sqlserver")
        {
            System.Console.WriteLine("Connecting to a local SQL");
            // this is scoped
            services.AddDbContextFactory<SignalBoxDbContext>((sp, options) =>
                {
                    var tenantProvider = sp.GetRequiredService<ITenantProvider>();
                    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                    var logger = loggerFactory.CreateLogger("DbFactory");
                    logger.LogInformation("Database Name: {dbName}", tenantProvider.CurrentDatabaseName);
                    logger.LogInformation("Local SQL Server Username: {username}", sqlServerUserName);
                    var cs = SqlServerConnectionStringFactory.GenerateLocalSqlConnectionString(
                        tenantProvider.CurrentDatabaseName,
                        sqlServerUserName: sqlServerUserName,
                        sqlServerPassword: sqlServerPassword);
                    options.UseSqlServer(cs, b => b.MigrationsAssembly(migrationAssembly).CommandTimeout(180));
                }, ServiceLifetime.Scoped);
            return services;
        }

        public static IServiceCollection UseMultitenantSqlite(this IServiceCollection services,
                                                    string directoryName,
                                                    SqliteDatabasesConfig dbConfig = null,
                                                    string migrationAssembly = "sqlite",
                                                    Func<string, string> manipulateConnectionString = null)
        {
            // this is scoped
            services.AddDbContextFactory<SignalBoxDbContext>((sp, options) =>
                {
                    var tenantProvider = sp.GetRequiredService<ITenantProvider>();
                    var cs = SqlLiteConnectionStringFactory.GenerateConnectionString(
                        dbConfig?.AppDir,
                        directoryName,
                        tenantProvider.CurrentDatabaseName);
                    if (manipulateConnectionString != null)
                    {
                        cs = manipulateConnectionString(cs);
                    }
                    options.UseSqlite(cs, b => b.MigrationsAssembly(migrationAssembly));
                }, ServiceLifetime.Scoped);
            return services;
        }
        public static IServiceCollection UseSqlite<T>(this IServiceCollection services,
                                                    string connectionString,
                                                    // bool enableEnvironments = true,
                                                    string migrationAssembly = "sqlite") where T : DbContextBase
        {
            services.AddDbContext<T>((provider, options) =>
               {
                   options.UseSqlite(connectionString, b => b.MigrationsAssembly(migrationAssembly));
                   //    if (enableEnvironments)
                   //    {
                   //        options.AddInterceptors(provider.GetRequiredService<IEnvironmentInterceptor>());
                   //    }
               });
            return services;
        }
        // public static IServiceCollection UseMemory(this IServiceCollection services)
        // {
        //     services.AddDbContext<SignalBoxDbContext>(options =>
        //         options.UseInMemoryDatabase("Server"));
        //     return services;
        // }

        public static IServiceCollection AddAzureStorageQueueStores(this IServiceCollection services)
        {
            services.AddScoped<INewTenantQueueStore, NewTenantQueueStore>();
            services.AddScoped<INewTenantMembershipQueueStore, NewTenantMembershipQueueStore>();
            services.AddScoped<IRunMetricGeneratorQueueStore, RunMetricGeneratorQueueStore>();
            services.AddScoped<IRunAllMetricGeneratorsQueueStore, RunAllMetricGeneratorsQueueStore>();
            services.AddScoped<IRunSegmentEnrolmentRuleQueueStore, RunSegmentEnrolmentRuleQueueStore>();

            services.AddScoped<IQueueStores, QueueStores>();
            return services;
        }

        public static IServiceCollection AddEFStores(this IServiceCollection services)
        {
            services.AddInterceptedScoped<ICustomerEventStore, EFCustomerEventStore, TimingInterceptor>();
            services.AddInterceptedScoped<ICustomerStore, EFCustomerStore, TimingInterceptor>();

            services.AddInterceptedScoped<ISegmentStore, EFSegmentStore, TimingInterceptor>();
            services.AddInterceptedScoped<IAudienceStore, EFAudienceStore, TimingInterceptor>();
            services.AddInterceptedScoped<IEnrolmentRuleStore, EFEnrolmentRuleStore, TimingInterceptor>();
            services.AddInterceptedScoped<IMetricEnrolmentRuleStore, EFMetricEnrolmentRuleStore, TimingInterceptor>();

            services.AddInterceptedScoped<IRecommendableItemStore, EFRecommendableItemStore, TimingInterceptor>();
            services.AddInterceptedScoped<IParameterStore, EFParameterStore, TimingInterceptor>();

            services.AddInterceptedScoped<IBusinessStore, EFBusinessStore, TimingInterceptor>();

            // environment
            services.AddInterceptedScoped<IEnvironmentStore, EFEnvironmentStore, TimingInterceptor>();

            // recommenders
            services.AddInterceptedScoped<IPromotionsCampaignStore, EFPromotionsCampaignStore, TimingInterceptor>();
            services.AddInterceptedScoped<IParameterSetCampaignStore, EFParameterSetCampaignStore, TimingInterceptor>();

            services.AddInterceptedScoped<IArgumentRuleStore, EFArgumentRuleStore, TimingInterceptor>();

            // recommendations
            services.AddInterceptedScoped<IRecommendationCorrelatorStore, EFRecommendationCorrelatorStore, TimingInterceptor>();
            services.AddInterceptedScoped<IParameterSetRecommendationStore, EFParameterSetRecommendationStore, TimingInterceptor>();
            services.AddInterceptedScoped<IItemsRecommendationStore, EFItemsRecommendationStore, TimingInterceptor>();
            services.AddInterceptedScoped<IOfferStore, EFOfferStore, TimingInterceptor>();

            // optimisers
            services.AddInterceptedScoped<IPromotionOptimiserStore, EFPromotionOptimiserStore, TimingInterceptor>();

            // performance
            services.AddInterceptedScoped<IItemsRecommenderPerformanceReportStore, EFItemsRecommenderPerformanceReportStore, TimingInterceptor>();

            services.AddInterceptedScoped<IMetricStore, EFMetricStore, TimingInterceptor>();
            services.AddInterceptedScoped<IMetricGeneratorStore, EFMetricGeneratorStore, TimingInterceptor>();
            services.AddInterceptedScoped<IHistoricCustomerMetricStore, EFHistoricCustomerMetricStore, TimingInterceptor>();
            services.AddInterceptedScoped<IGlobalMetricValueStore, EFGlobalMetricValueStore, TimingInterceptor>();
            services.AddInterceptedScoped<IBusinessMetricValueStore, EFBusinessMetricValueStore, TimingInterceptor>();

            // channels
            services.AddInterceptedScoped<IChannelStore, EFChannelStore, TimingInterceptor>();
            services.AddInterceptedScoped<IWebhookChannelStore, EFWebhookChannelStore, TimingInterceptor>();
            services.AddInterceptedScoped<IWebChannelStore, EFWebChannelStore, TimingInterceptor>();
            services.AddInterceptedScoped<IEmailChannelStore, EFEmailChannelStore, TimingInterceptor>();

            // deliveries
            services.AddInterceptedScoped<IDeferredDeliveryStore, EFCDeferredDeliveryStore, TimingInterceptor>();

            services.AddInterceptedScoped<IHashedApiKeyStore, EFHashedAPIKeyStore, TimingInterceptor>();
            services.AddInterceptedScoped<IModelRegistrationStore, EFModelRegistrationStore, TimingInterceptor>();
            services.AddInterceptedScoped<ITrackedUserSystemMapStore, EFTrackedUserSystemMapStore, TimingInterceptor>();
            services.AddInterceptedScoped<IWebhookReceiverStore, EFSWebhookReceiverStore, TimingInterceptor>();

            // integrated systems
            services.AddInterceptedScoped<IIntegratedSystemStore, EFIntegratedSystemStore, TimingInterceptor>();
            services.AddInterceptedScoped<ICustomIntegratedSystemStore, EFCustomIntegratedSystemStore, TimingInterceptor>();
            services.AddInterceptedScoped<IWebsiteIntegratedSystemStore, EFWebsiteIntegratedSystemStore, TimingInterceptor>();
            services.AddInterceptedScoped<IIntegratedSystemCredentialStore, EFIntegratedSystemCredentialStore, TimingInterceptor>();

            // discount codes
            services.AddInterceptedScoped<IDiscountCodeStore, EFDiscountCodeStore, TimingInterceptor>();

            // add a way to get all the stores in one  container
            services.AddScoped<IStoreCollection, DIStoreCollection>();

            // extra utils
            services.AddScoped<IStorageContext, EFStorageContext>();
            services.RegisterCollections();

            return services;
        }
    }
}
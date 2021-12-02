using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
                                                    string migrationAssembly = "sqlserver")
        {
            System.Console.WriteLine($"SQL Server Name: {serverName}");
            // this is scoped
            services.AddDbContextFactory<SignalBoxDbContext>((sp, options) =>
                {
                    var tenantProvider = sp.GetRequiredService<ITenantProvider>();
                    Console.WriteLine($"Database Name: {tenantProvider.CurrentDatabaseName}");
                    var cs = SqlServerConnectionStringFactory.GenerateAzureSqlConnectionString(
                        serverName,
                        tenantProvider.CurrentDatabaseName,
                        sqlServerUserName: sqlServerUserName,
                        sqlServerPassword: sqlServerPassword);
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
                    Console.WriteLine($"Database Name: {tenantProvider.CurrentDatabaseName}");
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
                                                    string migrationAssembly = "sqlite",
                                                    Func<string, string> manipulateConnectionString = null)
        {
            // this is scoped
            services.AddDbContextFactory<SignalBoxDbContext>((sp, options) =>
                {
                    var tenantProvider = sp.GetRequiredService<ITenantProvider>();
                    var cs = SqlLiteConnectionStringFactory.GenerateConnectionString(
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
                                                    bool enableEnvironments = true,
                                                    string migrationAssembly = "sqlite") where T : DbContextBase
        {
            services.AddDbContext<T>((provider, options) =>
               {
                   options.UseSqlite(connectionString, b => b.MigrationsAssembly(migrationAssembly));
                   if (enableEnvironments)
                   {
                       options.AddInterceptors(provider.GetRequiredService<IEnvironmentService>());
                   }
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
            services.AddScoped<ITrackedUserEventQueueStore, TrackedUserEventQueueStore>();
            services.AddScoped<INewTrackedUserEventQueueStore, NewTrackedUserEventQueueStore>();
            services.AddScoped<INewTenantQueueStore, NewTenantQueueStore>();
            services.AddScoped<INewTenantMembershipQueueStore, NewTenantMembershipQueueStore>();
            return services;
        }
        public static IServiceCollection AddEFStores(this IServiceCollection services)
        {
            services.AddScoped<ITrackedUserEventStore, EFTrackedUserEventStore>();
            services.AddScoped<ITrackedUserActionStore, EFTrackedUserActionStore>();
            services.AddScoped<IRewardSelectorStore, EFRewardSelectorStore>();
            services.AddScoped<ITrackedUserStore, EFTrackedUserStore>();

            services.AddScoped<ISegmentStore, EFSegmentStore>();
            services.AddScoped<IRuleStore, EFRuleStore>();

            services.AddScoped<IRecommendableItemStore, EFRecommendableItemStore>();
            services.AddScoped<IProductStore, EFProductStore>();
            services.AddScoped<IParameterStore, EFParameterStore>();

            // environment
            services.AddScoped<IEnvironmentStore, EFEnvironmentStore>();

            // recommenders
            services.AddScoped<IProductRecommenderStore, EFProductRecommenderStore>();
            services.AddScoped<IItemsRecommenderStore, EFItemsRecommenderStore>();
            services.AddScoped<IParameterSetRecommenderStore, EFParameterSetRecommenderStore>();

            // recommendations
            services.AddScoped<IRecommendationCorrelatorStore, EFRecommendationCorrelatorStore>();
            services.AddScoped<IParameterSetRecommendationStore, EFParameterSetRecommendationStore>();
            services.AddScoped<IProductRecommendationStore, EFProductRecommendationStore>();
            services.AddScoped<IItemsRecommendationStore, EFItemsRecommendationStore>();

            services.AddScoped<IFeatureStore, EFFeatureStore>();
            services.AddScoped<IFeatureGeneratorStore, EFFeatureGeneratorStore>();
            services.AddScoped<IHistoricTrackedUserFeatureStore, EFHistoricTrackedUserFeatureStore>();

            services.AddScoped<ITouchpointStore, EFTouchpointStore>();
            services.AddScoped<ITrackedUserTouchpointStore, EFTrackedUserTouchpointStore>();

            services.AddScoped<IHashedApiKeyStore, EFHashedAPIKeyStore>();
            services.AddScoped<IModelRegistrationStore, EFModelRegistrationStore>();
            services.AddScoped<ITrackedUserSystemMapStore, EFTrackedUserSystemMapStore>();
            services.AddScoped<IWebhookReceiverStore, EFSWebhookReceiverStore>();

            // integrated systems
            services.AddScoped<IIntegratedSystemStore, EFIntegratedSystemStore>();
            services.AddScoped<ICustomIntegratedSystemStore, EFCustomIntegratedSystemStore>();


            services.AddScoped<IStorageContext, EFStorageContext>();
            services.RegisterCollections();

            return services;
        }
    }
}
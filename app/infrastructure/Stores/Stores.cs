using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SignalBox.Core;
using SignalBox.Infrastructure.EntityFramework;
using SignalBox.Infrastructure.Queues;

namespace SignalBox.Infrastructure
{
    public static class Stores
    {
        public static IServiceCollection UseSqlServer(this IServiceCollection services,
                                                    string connectionString,
                                                    string migrationAssembly = "sqlserver")
        {
            services.AddDbContext<SignalBoxDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationAssembly).CommandTimeout(180)));
            return services;
        }
        public static IServiceCollection UseSqlLite(this IServiceCollection services,
                                                    string connectionString,
                                                    string migrationAssembly = "sqlite")
        {
            services.AddDbContext<SignalBoxDbContext>((provider, options) =>
               {
                   options.UseSqlite(connectionString, b => b.MigrationsAssembly(migrationAssembly));
                   options.AddInterceptors(provider.GetRequiredService<IEnvironmentService>());
               });
            return services;
        }
        public static IServiceCollection UseMemory(this IServiceCollection services)
        {
            services.AddDbContext<SignalBoxDbContext>(options =>
                options.UseInMemoryDatabase("Server"));
            return services;
        }

        public static IServiceCollection AddAzureStorageQueueStores(this IServiceCollection services)
        {
            services.AddScoped<ITrackedUserEventQueueStore, TrackedUserEventQueueStore>();
            services.AddScoped<INewTrackedUserEventQueueStore, NewTrackedUserEventQueueStore>();
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
            services.AddScoped<IIntegratedSystemStore, EFIntegratedSystemStore>();
            services.AddScoped<ITrackedUserSystemMapStore, EFTrackedUserSystemMapStore>();
            services.AddScoped<IWebhookReceiverStore, EFSWebhookReceiverStore>();

            services.AddScoped<IStorageContext, EFStorageContext>();

            return services;
        }
    }
}
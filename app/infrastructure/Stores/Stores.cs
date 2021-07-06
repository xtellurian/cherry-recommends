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
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationAssembly)));
            return services;
        }
        public static IServiceCollection UseSqlLite(this IServiceCollection services,
                                                    string connectionString,
                                                    string migrationAssembly = "sqlite")
        {
            services.AddDbContext<SignalBoxDbContext>(options =>
                options.UseSqlite(connectionString, b => b.MigrationsAssembly(migrationAssembly)));
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
            services.AddScoped<ITrackedUserStore, EFTrackedUserStore>();
            services.AddScoped<IExperimentStore, EFExperimentStore>();
            services.AddScoped<ISegmentStore, EFSegmentStore>();
            services.AddScoped<IRuleStore, EFRuleStore>();
            services.AddScoped<IOfferStore, EFOfferStore>();
            services.AddScoped<IPresentationOutcomeStore, EFPresentationOutcomeStore>();
            services.AddScoped<IProductStore, EFProductStore>();
            services.AddScoped<IParameterStore, EFParameterStore>();

            // recommenders
            services.AddScoped<IProductRecommenderStore, EFProductRecommenderStore>();
            services.AddScoped<IParameterSetRecommenderStore, EFParameterSetRecommenderStore>();

            // recommendations
            services.AddScoped<IRecommendationCorrelatorStore, EFRecommendationCorrelatorStore>();
            services.AddScoped<IOfferRecommendationStore, EFOfferRecommendationStore>();
            services.AddScoped<IParameterSetRecommendationStore, EFParameterSetRecommendationStore>();
            services.AddScoped<IProductRecommendationStore, EFProductRecommendationStore>();

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
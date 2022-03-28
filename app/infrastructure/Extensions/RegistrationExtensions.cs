using Microsoft.Extensions.DependencyInjection;
using SignalBox.Core;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;
using SignalBox.Infrastructure.EntityFramework;
using SignalBox.Infrastructure.ML;
using SignalBox.Infrastructure.Services;
using SignalBox.Infrastructure.Shims;
using SignalBox.Infrastructure.Webhooks;

namespace SignalBox.Infrastructure
{
    public static class RegistrationExtensions
    {
        public static IServiceCollection RegisterDefaultInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IApiTokenFactory, Auth0ApiTokenFactory>();
            services.AddScoped<IModelClientFactory, ModelClientFactory>();
            services.AddScoped<IRecommenderModelClientFactory, RecommenderModelClientFactory>();
            services.AddScoped<IInternalOptimiserClientFactory, InternalOptimiserClientFactory>();
            services.AddTransient<IDateTimeProvider, SystemDateTimeProvider>();
            services.AddScoped<IHubspotService, HubspotService>();
            services.AddScoped<IShopifyService, ShopifyService>();
            services.AddScoped<IAuth0Service, Auth0Manager>();
            services.AddScoped<IWebhookSenderClient, WebhookSenderClient>();
            services.AddScoped<IRecommendationCache<ItemsRecommender, ItemsRecommendation>, SimpleItemsRecommendationCache>();
            services.AddScoped<IRecommendationCache<ParameterSetRecommender, ParameterSetRecommendation>, SimpleParameterSetRecommendationCache>();
            services.AddScoped<ICategoricalOptimiserClient, AzureFunctionsOptimiserClient>();
            services.AddSingleton<IM2MTokenCache, Caches.TokenMemoryCache>();
            services.AddSingleton<IEventIngestor, AzureEventHubEventIngestor>(); // singleton to re-use the same eventhub connection
            services.AddHttpClient();
            return services;
        }

        public static IServiceCollection RegisterSingleTenantInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ITenantProvider, SingleTenantTenantProvider>();
            services.AddScoped<ITenantStore, ShimTenantStore>();
            services.AddScoped<ITenantMembershipStore, ShimTenantMembershipsStore>();
            services.AddScoped<IDbContextProvider<SignalBoxDbContext>, MultiTenantContextProvider<SignalBoxDbContext>>();
            return services;
        }
        public static IServiceCollection RegisterMultiTenantInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ITenantProvider, EFTenantProvider>();
            services.AddScoped<ITenantStore, EFTenantStore>();
            services.AddScoped<ITenantMembershipStore, EFTenantMembershipStore>();
            services.AddScoped<IDbContextProvider<SignalBoxDbContext>, MultiTenantContextProvider<SignalBoxDbContext>>();
            return services;
        }
    }
}
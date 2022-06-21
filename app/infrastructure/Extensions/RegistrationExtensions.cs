using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SignalBox.Core;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Campaigns;
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
            services.AddScoped<IRecommendationCache<PromotionsCampaign, ItemsRecommendation>, SimpleItemsRecommendationCache>();
            services.AddScoped<IRecommendationCache<ParameterSetCampaign, ParameterSetRecommendation>, SimpleParameterSetRecommendationCache>();
            services.AddScoped<ICategoricalOptimiserClient, AzureFunctionsOptimiserClient>();
            services.AddScoped<IDiscountCodeGenerator, ShopifyDiscountCodeGenerator>();
            services.AddScoped<IKlaviyoService, KlaviyoService>();
            services.AddSingleton<IM2MTokenCache, Caches.TokenMemoryCache>();
            services.AddSingleton<ICustomerEventIngestor, EventHubEventProcessingIngestor>(); // singleton to re-use the same eventhub connection
            services.AddSingleton<ICustomerHasUpdatedIngestor, EventHubCustomerHasUpdatedIngestor>(); // singleton to re-use the same eventhub connection
            services.AddSingleton<IPlatformEmailService, SimplePlatformEmailService>();

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

        public static IServiceCollection AddInterceptedScoped<TInterface, TImplemetation, TInterceptor>(this IServiceCollection services)
            where TInterface : class
            where TImplemetation : class, TInterface
            where TInterceptor : class, IInterceptor
        {

            services.TryAddSingleton<IProxyGenerator, ProxyGenerator>(); // register an internal castle.core thing
            services.TryAddScoped<TImplemetation>(); // register for use below
            services.TryAddTransient<TInterceptor>(); // register for use below
            services.TryAddScoped(provider =>
            {
                var proxyGenerator = provider.GetRequiredService<IProxyGenerator>();
                var impl = provider.GetRequiredService<TImplemetation>();
                var interceptor = provider.GetRequiredService<TInterceptor>();
                return proxyGenerator.CreateInterfaceProxyWithTarget<TInterface>(impl, interceptor);
            });
            return services;
        }
    }
}
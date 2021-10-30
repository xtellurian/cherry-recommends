using Microsoft.Extensions.DependencyInjection;

namespace SignalBox.Core
{
    public static class CollectionExtensions
    {
        public static IServiceCollection RegisterCollections(this IServiceCollection services)
        {
            services.AddScoped<IntegratedSystemStoreCollection>();

            return services;
        }
    }
}
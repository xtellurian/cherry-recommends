using Microsoft.Extensions.DependencyInjection;
using SignalBox.Core;
using SignalBox.Core.Recommenders;
using SignalBox.Infrastructure.ML;
using SignalBox.Infrastructure.Services;

namespace SignalBox.Infrastructure
{
    public static class RegistrationExtensions
    {
        public static IServiceCollection RegisterDefaultServices(this IServiceCollection services)
        {
            services.AddScoped<IApiTokenFactory, Auth0ApiTokenFactory>();
            services.AddScoped<IModelClientFactory, ModelClientFactory>();
            services.AddScoped<IRecommenderModelClientFactory, RecommenderModelClientFactory>();
            return services;
        }
    }
}
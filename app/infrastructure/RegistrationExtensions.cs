using Microsoft.Extensions.DependencyInjection;
using SignalBox.Core;
using SignalBox.Infrastructure.Services;

namespace SignalBox.Infrastructure
{
    public static class RegistrationExtensions
    {
        public static IServiceCollection RegisterAuth0M2M(this IServiceCollection services)
        {
            services.AddScoped<IApiTokenFactory, Auth0ApiTokenFactory>();
            return services;
        }
    }
}
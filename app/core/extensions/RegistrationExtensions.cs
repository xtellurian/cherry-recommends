using Microsoft.Extensions.DependencyInjection;

namespace SignalBox.Core
{
    public static class RegistrationExtensions
    {
        public static IServiceCollection RegisterCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IHasher, Sha256HasherService>();

            return services;
        }
    }
}
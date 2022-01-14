using Microsoft.Extensions.DependencyInjection;

namespace SignalBox.Core
{
    public static class RegistrationExtensions
    {
        public static void ValidateDbProvider(this string provider)
        {
            if (string.Equals(provider, "sqlite") || string.Equals(provider, "sqlserver"))
            {
                return;
            }

            throw new ConfigurationException($"{provider} provider is invalid");
        }

        public static IServiceCollection RegisterCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IHasher, Sha256HasherService>();

            return services;
        }
    }
}
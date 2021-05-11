using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SignalBox.Core;

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
        public static IServiceCollection AddBasicInMemoryStores(this IServiceCollection services)
        {
            services.AddSingleton<ITrackedUserEventStore, InMemoryEventStore>();
            services.AddSingleton<ITrackedUserStore, InMemoryTrackedUserStore>();
            services.AddSingleton<IExperimentStore, InMemoryExperimentStore>();
            services.AddSingleton<ISegmentStore, InMemorySegmentStore>();
            services.AddSingleton<IRuleStore, InMemoryRuleStore>();
            services.AddSingleton<IOfferStore, InMemoryOfferStore>();

            services.AddSingleton<IStorageContext, InMemoryStorageContext>();

            return services;
        }

        public static IServiceCollection AddEFStores(this IServiceCollection services)
        {
            services.AddScoped<IHashedApiKeyStore, EFHashedAPIKeyStore>();
            services.AddScoped<ITrackedUserEventStore, InMemoryEventStore>();
            services.AddScoped<ITrackedUserStore, EFTrackedUserStore>();
            services.AddScoped<IExperimentStore, EFExperimentStore>();
            services.AddScoped<ISegmentStore, EFSegmentStore>();
            services.AddScoped<IRuleStore, EFRuleStore>();
            services.AddScoped<IOfferStore, EFOfferStore>();
            services.AddScoped<IPresentationOutcomeStore, EFPresentationOutcomeStore>();
            services.AddScoped<IOfferRecommendationStore, EFOfferRecommendationStore>();

            services.AddScoped<IStorageContext, EFStorageContext>();

            return services;
        }
    }
}
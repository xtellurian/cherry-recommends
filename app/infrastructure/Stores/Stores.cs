using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SignalBox.Core;
using SignalBox.Infrastructure.EntityFramework;

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

        public static IServiceCollection AddEFStores(this IServiceCollection services)
        {
            services.AddScoped<ITrackedUserEventStore, EFTrackedUserEventStore>();
            services.AddScoped<ITrackedUserStore, EFTrackedUserStore>();
            services.AddScoped<IExperimentStore, EFExperimentStore>();
            services.AddScoped<ISegmentStore, EFSegmentStore>();
            services.AddScoped<IRuleStore, EFRuleStore>();
            services.AddScoped<IOfferStore, EFOfferStore>();
            services.AddScoped<IPresentationOutcomeStore, EFPresentationOutcomeStore>();
            services.AddScoped<IOfferRecommendationStore, EFOfferRecommendationStore>();
            
            services.AddScoped<IHashedApiKeyStore, EFHashedAPIKeyStore>();
            services.AddScoped<IModelRegistrationStore, EFModelRegistrationStore>();
            services.AddScoped<IIntegratedSystemStore, EFIntegratedSystemStore>();
            services.AddScoped<ITrackedUserSystemMapStore, EFTrackedUserSystemMapStore>();

            services.AddScoped<IStorageContext, EFStorageContext>();

            return services;
        }
    }
}
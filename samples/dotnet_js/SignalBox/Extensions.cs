using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SignalBox.Client
{
    public static class Extensions
    {
        public static IServiceCollection RegisterSignalBox(this IServiceCollection services)
        {
            // services.AddHttpClient();
            services.AddSingleton<Authenticator>();
            services.AddScoped<SignalBoxClient>();
            return services;
        }

        public static IServiceCollection ConfigureSignalBox(this IServiceCollection services, IConfigurationSection configSection)
        {
            services.Configure<Connection>(configSection);
            return services;
        }

        public static IServiceCollection ConfigureSignalBox(this IServiceCollection services, Connection connection)
        {
            services.AddOptions<Connection>().Configure(options =>
            {
                options.Host = connection.Host;
                options.ClientId = connection.ClientId;
                options.ClientSecret = connection.ClientSecret;
            });
            return services;
        }

        public static IApplicationBuilder UseSignalBox(this IApplicationBuilder app)
        {
            app.MapWhen(context => context.Request.Path.StartsWithSegments("/_SignalBox"), appBuilder =>
            {
                appBuilder.UseMiddleware<SignalBoxMiddleware>();
            });

            return app;
        }
    }
}
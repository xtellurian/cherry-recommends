using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalBox.Core;
using SignalBox.Infrastructure;
using SignalBox.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SignalBox.Core.Workflows;
using SignalBox.Web.Config;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace SignalBox.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var provider = Configuration.GetValue("Provider", "sqlserver");
            System.Console.WriteLine($"Database Provider: {provider}");
            if (provider == "sqlserver")
            {
                services.UseSqlServer(Configuration.GetConnectionString("Application"));
            }
            else if (provider == "sqlite")
            {
                services.UseSqlLite(Configuration.GetConnectionString("Application"));
            }
            else if (provider == "memory" || Env.IsDevelopment())
            {
                services.UseMemory();
            }

            // add core services
            services.RegisterCoreServices();

            services.AddEFStores();
            // add our logical workflows from the Core project
            services.RegisterWorkflows();
            services.AddHttpClient();
            services.RegisterAuth0M2M();
            services.Configure<Auth0M2MClient>(Configuration.GetSection("Auth0").GetSection("M2M"));
            services.AddTransient<IDateTimeProvider, SystemDateTimeProvider>();

            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(ApiVersions.MAJOR_0, ApiVersions.MINOR_1);
            });

            services.AddDatabaseDeveloperPageExceptionFilter();


            var auth0Config = Configuration.GetSection("Auth0");
            services.Configure<Auth0ReactConfig>(auth0Config.GetSection("ReactApp"));
            // and this is for using Auth0
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = auth0Config.GetValue<string>("Authority");
                options.Audience = auth0Config.GetValue<string>("Audience");
            });

            // TODO: fix security problems with app service and identity server
            // fixes inavlid issuer error when running in Azure App Service
            // probably a security hole
            // note the DEVELOPER certificate in appsettings.json when running in prod.
            // that is BAD. 
            // services.Configure<JwtBearerOptions>(
            //     IdentityServerJwtConstants.IdentityServerJwtBearerScheme,
            //     options =>
            //     {
            //         options.TokenValidationParameters.ValidateIssuer = false;
            //     });

            services.AddControllers().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            });
            services.AddRazorPages();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            // app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}

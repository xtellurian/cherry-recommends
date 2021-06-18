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
using Microsoft.OpenApi.Models;
using Hellang.Middleware.ProblemDetails;
using System;
using System.Net.Http;
using Hellang.Middleware.ProblemDetails.Mvc;
using Microsoft.AspNetCore.Mvc;
using SignalBox.Infrastructure.Files;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Integrations;
using Microsoft.ApplicationInsights.Extensibility;

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

            // Configure file storage type
            var fileSource = Configuration.GetSection("FileHosting").GetValue<string>("Source");
            if (fileSource == "local")
            {
                services.AddScoped<IFileStore, LocalFileStore>();
            }
            else if (fileSource == "blob")
            {
                services.Configure<FileHosting>(Configuration.GetSection("FileHosting"));
                services.AddScoped<IFileStore, AzureBlobFileStore>();
            }
            else
            {
                throw new NotImplementedException($"File Source {fileSource} is unknown");
            }

            services.Configure<DeploymentInformation>(Configuration.GetSection("Deployment"));

            services.AddApplicationInsightsTelemetry();

            // add core services
            services.RegisterCoreServices();

            services.AddEFStores();
            // add our logical workflows from the Core project
            services.RegisterWorkflows();
            services.AddHttpClient();
            services.RegisterAuth0M2M();
            services.Configure<Auth0M2MClient>(Configuration.GetSection("Auth0").GetSection("M2M"));
            services.Configure<HubspotAppCredentials>(Configuration.GetSection("HubSpot").GetSection("AppCredentials"));
            services.AddTransient<IDateTimeProvider, SystemDateTimeProvider>();
            services.AddScoped<IHubspotService, HubspotService>();

            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(ApiVersions.MAJOR_0, ApiVersions.MINOR_1);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SignalBox API", Version = "v1" });
                c.OperationFilter<ErrorOperationFilter>();

                // Set the comments path for the Open API JSON.
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(System.AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            if (Env.IsDevelopment())
            {
                services.AddDatabaseDeveloperPageExceptionFilter();
            }
            services.AddMvcCore().AddApiExplorer(); // required for swashbuckle

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

            services
                .AddProblemDetails(ConfigureProblemDetails)
                .AddControllers()
                .AddProblemDetailsConventions()
                .AddJsonOptions(o =>
                    {
                        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                    });

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TelemetryConfiguration configuration)
        {
            configuration.DisableTelemetry = env.IsDevelopment(); // disable AppInights in dev
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

            app.UseProblemDetails(); // must come after app.UseDeveloperExceptionPage()
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api/docs/{documentName}/spec.json";
            });

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
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


        private void ConfigureProblemDetails(ProblemDetailsOptions options)
        {
            // Only include exception details in a development environment. There's really no nee
            // to set this as it's the default behavior. It's just included here for completeness :)
            options.IncludeExceptionDetails = (ctx, ex) => Env.IsDevelopment();
            // You can configure the middleware to re-throw certain types of exceptions, all exceptions or based on a predicate.
            // This is useful if you have upstream middleware that needs to do additional handling of exceptions.
            options.Rethrow<NotSupportedException>();
            options.OnBeforeWriteDetails = (context, problemDetails) =>
            {
                // log the errors to the logging system
                var logger = context.RequestServices.GetService<ILogger<ProblemDetailsMiddleware>>();
                logger.LogError($"{problemDetails.Title} | {problemDetails.Status} | {problemDetails.Detail}");
            };
            options.Map<SignalBoxException>(_ => new ProblemDetails
            {
                Title = _.Title
            });

            // This will map NotImplementedException to the 501 Not Implemented status code.
            options.MapToStatusCode<NotImplementedException>(Microsoft.AspNetCore.Http.StatusCodes.Status501NotImplemented);

            // This will map HttpRequestException to the 503 Service Unavailable status code.
            options.MapToStatusCode<HttpRequestException>(Microsoft.AspNetCore.Http.StatusCodes.Status503ServiceUnavailable);

            // Because exceptions are handled polymorphically, this will act as a "catch all" mapping, which is why it's added last.
            // If an exception other than NotImplementedException and HttpRequestException is thrown, this will handle it.
            options.MapToStatusCode<Exception>(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError);
        }
    }
}

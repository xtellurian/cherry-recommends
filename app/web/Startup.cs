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
using SignalBox.Web.Services;
using SignalBox.Core.Security;
using SignalBox.Infrastructure.Models;
using SignalBox.Core.Serialization;
using SignalBox.Core.Optimisers;

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
            provider.ValidateDbProvider();

            var useMulti = Configuration.GetSection("Hosting").GetValue<bool>("Multitenant");
            System.Console.WriteLine($"Multitenant: {useMulti}");

            services.AddHttpContextAccessor();

            // enable the tenancy context connection

            if (useMulti)
            {
                services.RegisterMultiTenantInfrastructure();
                if (provider == "sqlserver")
                {
                    services.UseSqlServer<MultiTenantDbContext>(Configuration.GetConnectionString("Tenants"));
                    var azEnv = new SignalBoxAzureEnvironment();
                    Configuration.GetSection("AzureEnvironment").Bind(azEnv);
                    var localEnv = new SqlServerCredentials();
                    Configuration.GetSection("LocalSql").Bind(localEnv);
                    if (!string.IsNullOrEmpty(azEnv.SqlServerPassword))
                    {
                        services.UseMultitenantAzureSqlServer(
                            azEnv.SqlServerName,
                            azEnv.SqlServerPassword,
                            azEnv.SqlServerUserName);
                    }
                    else
                    {
                        services.UseMultitenantLocalSqlServer(localEnv.SqlServerPassword, localEnv.SqlServerUserName);
                    }
                }
                else if (provider == "sqlite")
                {
                    services.UseSqlite<MultiTenantDbContext>(Configuration.GetConnectionString("Tenants"));
                    services.UseMultitenantSqlite("databases");
                }
                else
                {
                    throw new NotImplementedException($"Provider {provider} is unknown");
                }
                services.AddSingleton<ITenantResolutionStrategy, SubdomainTenantResolutionStrategy>();
            }
            else
            {
                services.RegisterSingleTenantInfrastructure();
                // add single tenant databases

                if (provider == "sqlserver")
                {
                    var azEnv = new SignalBoxAzureEnvironment();
                    Configuration.GetSection("AzureEnvironment").Bind(azEnv);
                    var localEnv = new SqlServerCredentials();
                    Configuration.GetSection("LocalSql").Bind(localEnv);
                    if (!string.IsNullOrEmpty(azEnv.SqlServerPassword))
                    {
                        services.UseMultitenantAzureSqlServer(
                            azEnv.SqlServerName,
                            azEnv.SqlServerPassword,
                            azEnv.SqlServerUserName);
                    }
                    else
                    {
                        services.UseMultitenantLocalSqlServer(localEnv.SqlServerPassword, localEnv.SqlServerUserName);
                    }
                }
                else if (provider == "sqlite")
                {
                    services.UseMultitenantSqlite("databases");
                }
                else
                {
                    throw new ConfigurationException("Provider must be sqlite or sqlserver");
                }

                services.AddSingleton<ITenantResolutionStrategy, SingleTenantResolverStrategy>();
            }

            services.AddHttpContextAccessor();
            services.AddScoped<IEnvironmentProvider, EFCoreHttpHeaderEnvironmentProvider>();

            // configure storage queues
            services.Configure<AzureQueueConfig>(Configuration.GetSection("Queues"));
            services.Configure<QueueMessagesFileHosting>(Configuration.GetSection("Queues"));
            services.AddScoped<IQueueMessagesFileStore, AzureBlobQueueMessagesFileStore>();

            // Configure file storage type
            var fileSource = Configuration.GetSection("ReportFileHosting").GetValue<string>("Source");
            if (fileSource == "local")
            {
                services.AddScoped<IReportFileStore, LocalFileStore>();
            }
            else if (fileSource == "blob")
            {
                services.Configure<ReportFileHosting>(Configuration.GetSection("ReportFileHosting"));
                services.AddScoped<IReportFileStore, AzureBlobReportFileStore>();
            }
            else
            {
                throw new NotImplementedException($"File Source {fileSource} is unknown");
            }

            // Configure recommender reporting file storage type
            var recommenderFileSource = Configuration.GetSection("RecommenderImageFileHosting").GetValue<string>("Source");
            if (recommenderFileSource == "local")
            {
                throw new NotImplementedException("No local storage for recommender report images");
            }
            else if (recommenderFileSource == "blob")
            {
                services.Configure<RecommenderImageFileHosting>(Configuration.GetSection("RecommenderImageFileHosting"));
                services.AddScoped<IRecommenderImageFileStore, AzureBlobRecommenderImageFileStore>();
            }
            else
            {
                throw new NotImplementedException($"File Source {recommenderFileSource} is unknown");
            }

            services.Configure<DeploymentInformation>(Configuration.GetSection("Deployment"));

            services.AddApplicationInsightsTelemetry();
            services.AddScoped<ITelemetry, AppInsightsTelemetry>();

            // add core services
            services.RegisterCoreServices();

            services.AddAzureStorageQueueStores();
            services.AddEFStores();
            // add our logical workflows from the Core project
            services.RegisterWorkflows();
            services.RegisterDefaultInfrastructureServices();

            services.Configure<Auth0ManagementCredentials>(Configuration.GetSection("Auth0").GetSection("Management"));
            services.Configure<Auth0M2MClient>(Configuration.GetSection("Auth0").GetSection("M2M"));
            services.Configure<HubspotAppCredentials>(Configuration.GetSection("HubSpot").GetSection("AppCredentials"));
            services.Configure<Hosting>(Configuration.GetSection("Hosting"));
            services.Configure<PythonAzureFunctionsConnectionOptions>(Configuration.GetSection("PythonFunctions"));
            services.Configure<DotnetAzureFunctionsConnectionOptions>(Configuration.GetSection("DotnetFunctions"));


            services.AddScoped<ITenantAuthorizationStrategy, TokenClaimTenantAuthorizor>();

            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(ApiVersions.MAJOR_0, ApiVersions.MINOR_1);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cherry Recommends API", Version = "v1" });
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

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.AdminOnlyPolicyName, policy =>
                {
                    policy.RequireClaim("scope");
                    policy.RequireAssertion(_ =>
                    {
                        return _.User.FindFirst("scope").Value.Contains(Scopes.Features.Write);
                    });
                });

            });

            services.AddCors(options =>
            {
                // don't add a default policy, it will apply to all endpoints.
                options.AddPolicy(CorsPolicies.WebApiKeyPolicy, builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .WithHeaders("x-api-key");
                });
            });

            services
                .AddProblemDetails(ConfigureProblemDetails)
                .AddControllers()
                .AddProblemDetailsConventions()
                .AddJsonOptions(o =>
                    {
                        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                        o.JsonSerializerOptions.Converters.Add(new TimeSpanJsonConverter());
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
                app.UseHttpsRedirection();
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseProblemDetails(); // must come after app.UseDeveloperExceptionPage()
            app.UseMiddleware<ExceptionTelemetryMiddleware>(); // must come after UseProblemDetails()

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api/docs/{documentName}/spec.json";
            });

            // the order is important.
            app.UseMiddleware<TenantSelectorMiddleware>(); // select must occur before API Key Middleware, cos db access is required.
            app.UseMiddleware<ApiKeyMiddleware>();
            app.UseAuthorization();
            app.UseMiddleware<TenantAuthorizerMiddleware>();
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

            options.Map<SignalBoxException>(_ =>
            {
                if (_ is ModelInvokationException modelInvokationException)
                {
                    return new ProblemDetails
                    {
                        Title = "Model invokation failed.",
                        Status = 400,
                        Detail = modelInvokationException.ModelResponseContent
                    };
                }
                return new ProblemDetails
                {
                    Title = _.Title,
                    Status = _.Status,
                };
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

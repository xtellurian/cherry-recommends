using System.Threading.Tasks;
using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Web.Inputs;
using Pulumi.AzureNative.Web;
using System.Collections.Generic;
using System;

namespace SignalBox.Azure
{
    partial class AppSvc : ComponentWithStorage
    {
        protected Pulumi.Config appSvcConfig;
        protected Pulumi.Config auth0Config;
        protected Pulumi.Config azureConfig;
        protected Pulumi.Config launchDarklyConfig;
        protected string environment;
        protected string? canonicalRootDomain;
        private readonly bool multitenant;
        private readonly Dictionary<string, string> tags;

        public AppSvc(ResourceGroup rg,
                      MultitenantDatabaseComponent multiDb,
                      Storage storage,
                      AzureML ml,
                      EventProcessing eventProcessing,
                      AppObservation observation,
                      Dictionary<string, string>? tags = null)
        {
            this.tags ??= new Dictionary<string, string>();
            // create an app service plan
            this.appSvcConfig = new Pulumi.Config("appsvc");
            this.auth0Config = new Pulumi.Config("auth0");
            this.azureConfig = new Pulumi.Config("azure-native");
            this.launchDarklyConfig = new Pulumi.Config("launch-darkly");
            this.environment = new Pulumi.Config().Require("environment");
            this.canonicalRootDomain = appSvcConfig.Get("canonical-root-domain");
            this.multitenant = appSvcConfig.RequireBoolean("multitenant");

            var hubspotConfig = new Pulumi.Config("hubspot");
            var segmentConfig = new Pulumi.Config("segment");
            var hotjarConfig = new Pulumi.Config("hotjar");
            var shopifyConfig = new Pulumi.Config("shopify");
            System.Console.WriteLine($"Canonical Root Domain is {canonicalRootDomain ?? "null"}");

            var plan = new AppServicePlan("asp", new AppServicePlanArgs
            {
                ResourceGroupName = rg.Name,
                Kind = "linux",
                Reserved = true,
                Sku = new SkuDescriptionArgs
                {
                    Tier = appSvcConfig.Get("sku-tier") ?? "Free",
                    Name = appSvcConfig.Get("sku-name") ?? "F1",
                    Capacity = appSvcConfig.GetInt32("capacity") ?? 1
                },
                Tags = tags!
            });

            var webApp = new WebApp("app", new WebAppArgs
            {
                Tags = tags!,
                ResourceGroupName = rg.Name,
                ServerFarmId = plan.Id,
                Kind = "app,linux",
                SiteConfig = new SiteConfigArgs
                {
                    Cors = new CorsSettingsArgs
                    {
                        // Allowed Origins cannot be * if SupportCredentials = True
                        AllowedOrigins = "*",
                        SupportCredentials = false
                    },
                    AppSettings = {
                        // warning! these are overwritten below
                        new NameValuePairArgs{
                            Name = "CURRENT_STACK",
                            Value = "dotnetcore",
                        },
                        new NameValuePairArgs{
                            Name = "WEBSITE_HTTPLOGGING_RETENTION_DAYS",
                            Value = "1",
                        },
                    },
                    LinuxFxVersion = "DOTNETCORE|6.0",
                    ConnectionStrings = {
                        new ConnStringInfoArgs
                        {
                            Name = "Tenants",
                            Type = ConnectionStringType.SQLAzure,
                            ConnectionString = multiDb.TenantDbConnectionString
                        },
                    },
                    HealthCheckPath = "/health",
                }
            });
            this.PythonFunctionApp = CreatePythonFunctions(rg, storage, observation.AppInsights, plan);

            if (multitenant && !string.IsNullOrEmpty(canonicalRootDomain))
            {
                AppSvcCertificate(rg);
            }

            this.WebApp = webApp;

            // connect to Auth0 users
            var auth0 = new Auth0(webApp);
            var dotnetFunctionApp = CreateDotnetFuncs(rg,
                                                      multiDb,
                                                      storage,
                                                      observation.AppInsights,
                                                      eventProcessing,
                                                      plan,
                                                      auth0);

            this.DotnetFunctionApp = dotnetFunctionApp;
            // include the functionApp Id in this method so it doesn't get called too early
            // also, dont think I actually need thissubs
            // this.FunctionAppDefaultKey = Output.Tuple(rg.Name, functionApp.Name, functionApp.Id).Apply(names =>
            //    Output.CreateSecret(GetDefaultFunctionKey(names.Item1, names.Item2)));

            // these are set AFTER creating the web app because of the Auth0 circular dependency
            var appsettings = new WebAppApplicationSettings("appsettings", new WebAppApplicationSettingsArgs
            {
                Name = webApp.Name,
                ResourceGroupName = rg.Name,
                Properties = {
                    {"CURRENT_STACK", "dotnetcore"},
                    {"ASPNETCORE_HTTPS_PORT", "443"},
                    {"Deployment__Stack", Pulumi.Deployment.Instance.StackName},
                    {"Deployment__Project", Pulumi.Deployment.Instance.ProjectName},
                    {"Deployment__Environment", environment},
                    {"ApplicationInsights__InstrumentationKey", observation.AppInsights.InstrumentationKey},
                    {"WEBSITE_HTTPLOGGING_RETENTION_DAYS", "1"},

                    // auth0
                    {"Auth0__Authority", auth0.Authority},
                    {"Auth0__Audience", auth0.Audience},

                    {"Auth0__ReactApp__Domain", auth0.ReactDomain},
                    {"Auth0__ReactApp__ClientId", auth0.ReactClientId},
                    {"Auth0__ReactApp__Audience", auth0.Audience},
                    {"Auth0__ReactApp__ManagementAudience", auth0.ReactManagementAudience},

                    {"Auth0__M2M__Audience", auth0.Audience},
                    {"Auth0__M2M__ClientId", auth0.M2MClientId},
                    {"Auth0__M2M__ClientSecret", auth0.M2MClientSecret},
                    {"Auth0__M2M__Endpoint", auth0.M2MEndpoint},

                    {"Auth0__Management__DefaultAudience", auth0.DefaultAudience},
                    {"Auth0__Management__ClientId", auth0Config.Require("clientId")},
                    {"Auth0__Management__ClientSecret", auth0Config.Require("clientSecret")},
                    {"Auth0__Management__Domain", auth0Config.Require("domain")},
                    {"Auth0__Management__Endpoint", $"https://{auth0Config.Require("domain")}/oauth/token"},

                    {"ReportFileHosting__ConnectionString", ml.PrimaryStorageConnectionString},
                    {"ReportFileHosting__ContainerName", "reports"},
                    {"ReportFileHosting__Source", "blob"},

                    {"RecommenderImageFileHosting__ConnectionString", ml.PrimaryStorageConnectionString},
                    {"RecommenderImageFileHosting__ContainerName", "recommenders"},
                    {"RecommenderImageFileHosting__SubPath", "reports"},
                    {"RecommenderImageFileHosting__Source", "blob"},

                    {"Queues__ConnectionString", storage.PrimaryConnectionString},
                    {"Queues__ContainerName", "queue-messages"},
                    {"Queues__EnableWriteQueue", true.ToString()},
                    {"Queues__EnableReadQueue", false.ToString()},

                    // python functions connection
                    {"PythonFunctions__Url", (PythonFunctionApp == null) ? Output.Create("") :  Output.Format($"https://{PythonFunctionApp.DefaultHostName}/")},
                    {"PythonFunctions__Key", (PythonFunctionApp == null) ? Output.Create("") :   Output.Format($"{this.PythonFunctionAppDefaultKey}")},

                    // dotnet functions connection
                    {"DotnetFunctions__Url",  Output.Format($"https://{dotnetFunctionApp.DefaultHostName}/")},
                    {"DotnetFunctions__Key",  Output.Format($"{this.DotnetFunctionAppDefaultKey}")},

                    // hubspot connection
                    {"HubSpot__AppCredentials__AppId", hubspotConfig.Get("appId") ?? ""},
                    {"HubSpot__AppCredentials__ClientId", hubspotConfig.Get("clientId") ?? ""},
                    {"HubSpot__AppCredentials__ClientSecret", hubspotConfig.Get("clientSecret") ?? ""},
                    {"HubSpot__AppCredentials__Scope", hubspotConfig.Get("scope") ?? "contacts oauth tickets" },
                    {"LastDeployed", System.DateTime.Now.ToString()}, // so these always get re-deployed when run.
                    
                    // multitenant
                    {"Provider", "sqlserver"},
                    {"Hosting__Multitenant", multitenant.ToString()},
                    {"Hosting__CanonicalRootDomain", canonicalRootDomain ?? ""},
                    {"Hosting__SingleTenantDatabaseName", appSvcConfig.Get("singleTenantDatabaseName") ?? "single"},

                    // azure
                    {"AzureEnvironment__SqlServerName", multiDb.ServerName},
                    {"AzureEnvironment__SqlServerUserName", multiDb.AdminUserName},
                    {"AzureEnvironment__SqlServerPassword", multiDb.AdminPassword},

                    // event processing 
                    {"EventProcessing__EventIngestion__ConnectionString", eventProcessing.PrimaryNamespaceWriteConnectionString},
                    {"EventProcessing__EventIngestion__EventhubName", eventProcessing.EventProcessingHubName},
                    {"EventProcessing__CustomerHasUpdated__ConnectionString", eventProcessing.PrimaryNamespaceWriteConnectionString},
                    {"EventProcessing__CustomerHasUpdated__EventhubName", eventProcessing.CustomerHasUpdatedHubName},

                    // segment
                    {"Segment__WriteKey", segmentConfig.Get("writeKey") ?? ""},

                    // hotjar
                    {"Hotjar__SiteId", hotjarConfig.Get("siteId") ?? "0"},
                    {"Hotjar__SnippetVersion", hotjarConfig.Get("snippetVersion") ?? "0"},
                    
                    // launch darkly
                    {"LaunchDarkly__ClientSideId", launchDarklyConfig.Get("clientSideId") ?? ""},

                    // shopify
                    {"Shopify__AppCredentials__ApiKey", shopifyConfig.Get("apiKey") ?? ""},
                    {"Shopify__AppCredentials__SecretKey", shopifyConfig.Get("secretKey") ?? ""},
                    {"Shopify__Billing__Test", shopifyConfig.Get("billing_test") ?? "false"},
                    {"Shopify__Billing__Skip", shopifyConfig.Get("billing_skip") ?? "false"},
                }
            }, new CustomResourceOptions
            {
                DependsOn = { webApp }
            });

            this.CanonicalRootDomain = canonicalRootDomain ?? "";
        }

        public string CanonicalRootDomain { get; }
        public WebApp WebApp { get; }
        public WebApp? PythonFunctionApp { get; }
        public WebApp DotnetFunctionApp { get; }
        public bool Multitenant => multitenant;

        private static async Task<string> GetDefaultFunctionKey(string resourceGroupName, string name)
        {
            var keys = await ListWebAppHostKeys.InvokeAsync(new ListWebAppHostKeysArgs
            {
                Name = name,
                ResourceGroupName = resourceGroupName
            });
            return keys?.FunctionKeys!["default"]!; // ! fixes compiler null warnings
        }
    }

}

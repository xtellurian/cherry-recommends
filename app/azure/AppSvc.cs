using System.Threading.Tasks;
using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Web.Inputs;
using Pulumi.AzureNative.Web;
using System.Collections.Generic;

namespace SignalBox.Azure
{
    class AppSvc : ComponentWithStorage
    {
        public AppSvc(ResourceGroup rg,
                      DatabaseComponent db,
                      Storage storage,
                      AzureML ml,
                      Pulumi.AzureNative.Insights.Component insights,
                      Dictionary<string, string>? tags = null)
        {
            tags ??= new Dictionary<string, string>();
            // create an app service plan
            var appSvcConfig = new Pulumi.Config("appsvc");
            var hubspotConfig = new Pulumi.Config("hubspot");
            var environment = new Pulumi.Config().Require("environment");
            var corsOrigins = appSvcConfig.GetObject<List<string>>("corsorigins") ?? new List<string>();

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
                Tags = tags
            });

            var webApp = new WebApp("app", new WebAppArgs
            {
                Tags = tags,
                ResourceGroupName = rg.Name,
                ServerFarmId = plan.Id,
                Kind = "app,linux",
                SiteConfig = new SiteConfigArgs
                {
                    Cors = new CorsSettingsArgs
                    {
                        // Allowed Origins cannot be * if SupportCredentials = True
                        AllowedOrigins = corsOrigins,
                        SupportCredentials = true
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
                    LinuxFxVersion = "DOTNETCORE|5.0",
                    ConnectionStrings = {
                        new ConnStringInfoArgs
                        {
                            Name = "Application",
                            Type = ConnectionStringType.SQLAzure,
                            ConnectionString = db.DatabaseConnectionString
                        },
                    },
                }
            });

            var functionApp = new WebApp("pythonJobs", new WebAppArgs
            {
                Tags = tags,
                ResourceGroupName = rg.Name,
                ServerFarmId = plan.Id,
                Kind = "functionapp",
                SiteConfig = new SiteConfigArgs
                {
                    AppSettings = {
                        new NameValuePairArgs{
                            Name = "AzureWebJobsStorage",
                            Value = storage.PrimaryConnectionString,
                        },
                        new NameValuePairArgs{
                            Name = "FUNCTIONS_EXTENSION_VERSION",
                            Value = "~3",
                        },
                        new NameValuePairArgs{
                            Name = "FUNCTIONS_WORKER_RUNTIME",
                            Value = "python",
                        },
                        new NameValuePairArgs{
                            Name = "APPINSIGHTS_INSTRUMENTATIONKEY",
                            Value = insights.InstrumentationKey,
                        }
                    },
                    Http20Enabled = true,
                },
            });

            var dotnetFunctionApp = new WebApp("dotnetjobs", new WebAppArgs
            {
                Tags = tags,
                ResourceGroupName = rg.Name,
                ServerFarmId = plan.Id,
                Kind = "functionapp",
                SiteConfig = new SiteConfigArgs
                {
                    AlwaysOn = true, // recommended in the Azure portal.
                    AppSettings = {
                        new NameValuePairArgs{
                            Name = "AzureWebJobsStorage",
                            Value = storage.PrimaryConnectionString,
                        },
                        new NameValuePairArgs{
                            Name = "FUNCTIONS_EXTENSION_VERSION",
                            Value = "~3",
                        },
                        new NameValuePairArgs{
                            Name = "FUNCTIONS_WORKER_RUNTIME",
                            Value = "dotnet-isolated",
                        },
                        new NameValuePairArgs{
                            Name = "APPINSIGHTS_INSTRUMENTATIONKEY",
                            Value = insights.InstrumentationKey,
                        }
                    },
                    ConnectionStrings = {
                        new ConnStringInfoArgs
                        {
                            Name = "Application",
                            Type = ConnectionStringType.SQLAzure,
                            ConnectionString = db.DatabaseConnectionString
                        },
                    },
                    Http20Enabled = true,
                    LinuxFxVersion = "dotnet|5.0"
                },
            });

            this.WebApp = webApp;
            this.FunctionApp = functionApp;
            this.DotnetFunctionApp = dotnetFunctionApp;
            // include the functionApp Id in this method so it doesn't get called too early
            // also, dont think I actually need thissubs
            // this.FunctionAppDefaultKey = Output.Tuple(rg.Name, functionApp.Name, functionApp.Id).Apply(names =>
            //    Output.CreateSecret(GetDefaultFunctionKey(names.Item1, names.Item2)));


            // connect to Auth0 users
            var auth0 = new Auth0(webApp);

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
                    {"ApplicationInsights__InstrumentationKey", insights.InstrumentationKey},
                    {"WEBSITE_HTTPLOGGING_RETENTION_DAYS", "1"},
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
                    {"ReportFileHosting__ConnectionString", ml.PrimaryStorageConnectionString},
                    {"ReportFileHosting__ContainerName", "reports"},
                    {"ReportFileHosting__Source", "blob"},
                    {"Queues__ConnectionString", storage.PrimaryConnectionString},
                    {"Queues__ContainerName", "queue-messages"},
                    {"Queues__EnableWriteQueue", true.ToString()},
                    {"Queues__EnableReadQueue", false.ToString()},
                    {"HubSpot__AppCredentials__AppId", hubspotConfig.Get("appId") ?? ""},
                    {"HubSpot__AppCredentials__ClientId", hubspotConfig.Get("clientId") ?? ""},
                    {"HubSpot__AppCredentials__ClientSecret", hubspotConfig.Get("clientSecret") ?? ""},
                    {"HubSpot__AppCredentials__Scope", hubspotConfig.Get("scope") ?? "contacts oauth tickets" },
                    {"LastDeployed", System.DateTime.Now.ToString()} // so these always get re-deployed when run.
                }
            }, new CustomResourceOptions
            {
                DependsOn = { webApp }
            });
        }

        public WebApp WebApp { get; }
        public WebApp FunctionApp { get; }
        public WebApp DotnetFunctionApp { get; }
        // public Output<string> FunctionAppDefaultKey { get; }

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

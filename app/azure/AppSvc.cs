using System.Threading.Tasks;
using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Web.Inputs;
using Pulumi.AzureNative.Web;

namespace Signalbox.Azure
{
    class AppSvc
    {
        public AppSvc(ResourceGroup rg, DatabaseComponent db, Storage storage)
        {
            // create an app service plan
            var appSvcConfig = new Pulumi.Config("appsvc");

            var plan = new AppServicePlan("asp", new AppServicePlanArgs
            {
                ResourceGroupName = rg.Name,
                Kind = "linux",
                Reserved = true,
                Sku = new SkuDescriptionArgs
                {
                    Tier = appSvcConfig.Get("sku-tier") ?? "Free",
                    Name = appSvcConfig.Get("sku-name") ?? "F1",
                },
            });

            var webApp = new WebApp("app", new WebAppArgs
            {
                ResourceGroupName = rg.Name,
                ServerFarmId = plan.Id,
                Kind = "app,linux",
                SiteConfig = new SiteConfigArgs
                {
                    Cors = new CorsSettingsArgs
                    {
                        // Allowed Origins cannot be * if SupportCredentials = True
                        AllowedOrigins = {
                            "http://localhost:5000",
                            "https://localhost:5001"
                        },
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
                //     new NameValuePairArgs{
                //         Name = "APPINSIGHTS_INSTRUMENTATIONKEY",
                //         Value = appInsights.InstrumentationKey
                //     },
                //     new NameValuePairArgs{
                //         Name = "APPLICATIONINSIGHTS_CONNECTION_STRING",
                //         Value = appInsights.InstrumentationKey.Apply(key => $"InstrumentationKey={key}"),
                //     },
                //     new NameValuePairArgs{
                //         Name = "ApplicationInsightsAgent_EXTENSION_VERSION",
                //         Value = "~2",
                //     },
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
                ResourceGroupName = rg.Name,
                ServerFarmId = plan.Id,
                Kind = "functionapp",
                SiteConfig = new SiteConfigArgs
                {
                    AppSettings = {
                        // warning! these are overwritten below
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
                        }
                        },
                    Http20Enabled = true,
                },
            });

            this.WebApp = webApp;
            this.FunctionApp = functionApp;
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
                    {"LastDeployed", System.DateTime.Now.ToString()} // so these always get re-deployed when run.
                }
            }, new CustomResourceOptions
            {
                DependsOn = { webApp }
            });
        }

        public WebApp WebApp { get; }
        public WebApp FunctionApp { get; }
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

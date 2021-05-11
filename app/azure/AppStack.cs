using System.Collections.Generic;
using System.Threading.Tasks;
using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Web.Inputs;

namespace Signalbox.Azure
{
    class AppStack : Stack
    {
        public AppStack()
        {
            var tags = new Dictionary<string, string>
            {
                {"Pulumi", "true"},
                {"Pulumi Stack", Pulumi.Deployment.Instance.StackName},
                {"Pulumi Project", Pulumi.Deployment.Instance.ProjectName}
            };

            var commonRgArgs = new ResourceGroupArgs
            {
                Tags = tags,
            };
            // Create Resource Groups
            var appRg = new ResourceGroup("application", commonRgArgs);
            var analyticsRg = new ResourceGroup("analytics", commonRgArgs);

            var db = new DatabaseComponent(appRg);
            this.DatabaseConnectionString = db.DatabaseConnectionString;
            // Create an Azure analytics environment
            var analytics = new AzureML(analyticsRg, db);
            this.PrimaryStorageKey = analytics.PrimaryStorageKey;

            // create an app service plan
            var appSvcConfig = new Pulumi.Config("appsvc");

            var appServicePlan = new AppServicePlan("asp", new AppServicePlanArgs
            {
                ResourceGroupName = appRg.Name,
                Kind = "linux",
                Reserved = true,
                Sku = new SkuDescriptionArgs
                {
                    Tier = appSvcConfig.Get("sku-tier") ?? "Free",
                    Name = appSvcConfig.Get("sku-name") ?? "F1",
                },
            });

            var app = new WebApp("app", new WebAppArgs
            {
                ResourceGroupName = appRg.Name,
                ServerFarmId = appServicePlan.Id,
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

            // connect to Auth0 users
            var auth0 = new Auth0(app);

            // these are set AFTER creating the web app because of the Auth0 circular dependency
            var appsettings = new WebAppApplicationSettings("appsettings", new WebAppApplicationSettingsArgs
            {
                Name = app.Name,
                ResourceGroupName = appRg.Name,
                Properties = {
                    {"CURRENT_STACK", "dotnetcore"},
                    {"WEBSITE_HTTPLOGGING_RETENTION_DAYS", "1"},
                    {"Auth0__Authority", auth0.Authority},
                    {"Auth0__Audience", auth0.Audience},
                    {"Auth0__ReactApp__Domain", auth0.ReactDomain},
                    {"Auth0__ReactApp__ClientId", auth0.ReactClientId},
                    {"Auth0__ReactApp__Audience", auth0.Audience},
                    {"Auth0__ReactApp__ManagementAudience", auth0.ReactManagementAudience},
                    {"LastDeployed", System.DateTime.Now.ToString()} // so these always get re-deployed when run.
                }
            }, new CustomResourceOptions
            {
                DependsOn = { app }
            });

            this.WebappName = app.Name;
            this.ResourceGroup = app.ResourceGroup;
            // Export the primary key of the Storage Account

        }

        [Output]
        public Output<string> WebappName { get; set; }
        [Output]
        public Output<string> ResourceGroup { get; set; }

        [Output]
        public Output<string> PrimaryStorageKey { get; set; }
        [Output]
        public Output<string> DatabaseConnectionString { get; private set; }

        private static async Task<string> GetStorageAccountPrimaryKey(string resourceGroupName, string accountName)
        {
            var accountKeys = await ListStorageAccountKeys.InvokeAsync(new ListStorageAccountKeysArgs
            {
                ResourceGroupName = resourceGroupName,
                AccountName = accountName
            });
            return accountKeys.Keys[0].Value;
        }
    }

}
using System.Threading.Tasks;
using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Web.Inputs;
using Pulumi.AzureNative.Web;
using System.Collections.Generic;
using Pulumi.AzureNative.Insights;
using Pulumi.AzureNative.Authorization;

namespace SignalBox.Azure
{
    partial class AppSvc : ComponentWithStorage
    {
        private WebApp CreateDotnetFuncs(ResourceGroup rg,
                                        MultitenantDatabaseComponent multiDb,
                                        Storage storage,
                                        Component insights,
                                        AppServicePlan plan,
                                        Auth0 auth0)
        {
            var hubspotConfig = new Pulumi.Config("hubspot");

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
                        },
                        // multiteant things
                        new NameValuePairArgs{
                            Name = "Provider",
                            Value = "sqlserver",
                        },
                        new NameValuePairArgs{
                            Name = "Hosting__Multitenant",
                            Value = appSvcConfig.Require("multitenant"),
                        },
                        new NameValuePairArgs{
                            Name = "Hosting__SingleTenantDatabaseName",
                            Value = appSvcConfig.Get("singleTenantDatabaseName") ?? "single",
                        },
                        new NameValuePairArgs{
                            Name = "Hosting__CanonicalRootDomain",
                            Value = canonicalRootDomain ?? "",
                        },
                        // hubspot 
                        new NameValuePairArgs{
                            Name = "HubSpot__AppCredentials__AppId",
                            Value = hubspotConfig.Get("appId") ?? "",
                        },
                        new NameValuePairArgs{
                            Name = "HubSpot__AppCredentials__ClientId",
                            Value = hubspotConfig.Get("clientId") ?? "",
                        },
                        new NameValuePairArgs{
                            Name = "HubSpot__AppCredentials__ClientSecret",
                            Value = hubspotConfig.Get("clientSecret") ?? "",
                        },
                        new NameValuePairArgs{
                            Name = "HubSpot__AppCredentials__Scope",
                            Value = hubspotConfig.Get("scope") ?? "contacts oauth tickets",
                        },
                        // auth0
                        new NameValuePairArgs{
                            Name = "Auth0__Management__DefaultAudience",
                            Value = auth0.DefaultAudience
                        },
                        new NameValuePairArgs{
                            Name = "Auth0__Management__ClientId",
                            Value = auth0Config.Require("clientId")
                        },
                        new NameValuePairArgs{
                            Name = "Auth0__Management__ClientSecret",
                            Value = auth0Config.Require("clientSecret")
                        },
                        new NameValuePairArgs{
                            Name = "Auth0__Management__Domain",
                            Value = auth0Config.Require("domain")
                        },
                        new NameValuePairArgs{
                            Name = "Auth0__Management__Endpoint",
                            Value = $"https://{auth0Config.Require("domain")}/oauth/token"
                        },
                        // azure envuroment
                        new NameValuePairArgs{
                            Name = "AzureEnvironment__SubscriptionId",
                            Value = azureConfig.Require("subscriptionId")
                        },
                        new NameValuePairArgs{
                            Name = "AzureEnvironment__SqlServerName",
                            Value = multiDb.ServerName
                        },
                        new NameValuePairArgs{
                            Name = "AzureEnvironment__SqlServerAzureId",
                            Value = multiDb.Server.Id
                        },
                        new NameValuePairArgs{
                            Name = "AzureEnvironment__SqlServerUserName",
                            Value = multiDb.UserName
                        },
                        new NameValuePairArgs{
                            Name = "AzureEnvironment__SqlServerPassword",
                            Value = multiDb.Password
                        },
                        new NameValuePairArgs{
                            Name = "AzureEnvironment__ElasticPoolName",
                            Value = multiDb.ElasticPool.Name
                        },
                    },
                    ConnectionStrings = {
                        new ConnStringInfoArgs
                        {
                            Name = "Tenants",
                            Type = ConnectionStringType.SQLAzure,
                            ConnectionString = multiDb.TenantDbConnectionString
                        },
                    },
                    Http20Enabled = true,
                    LinuxFxVersion = "dotnet|5.0",
                },
                Identity = new ManagedServiceIdentityArgs
                {
                    Type = Pulumi.AzureNative.Web.ManagedServiceIdentityType.SystemAssigned
                }
            });

            var subscriptionId = azureConfig.Require("subscriptionId");
            // az role definition list --name "Contributor"
            var roleAssignment = new RoleAssignment("databaseContrib", new RoleAssignmentArgs
            {
                PrincipalId = dotnetFunctionApp.Identity.Apply(_ => _!.PrincipalId),
                PrincipalType = "ServicePrincipal",
                // RoleAssignmentName = "b24988ac-6180-42a0-ab88-20f7382dd24c", // contributor
                RoleDefinitionId = $"/subscriptions/{subscriptionId}/providers/Microsoft.Authorization/roleDefinitions/b24988ac-6180-42a0-ab88-20f7382dd24c",
                Scope = Output.Format($"subscriptions/{subscriptionId}/resourceGroups/{multiDb.ResourceGroup.Name}")
            });

            return dotnetFunctionApp;
        }

        private WebApp CreatePythonFunctions(ResourceGroup rg,
                                                    Storage storage,
                                                    Component insights,
                                                    AppServicePlan plan)
        {
            return new WebApp("pythonJobs", new WebAppArgs
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
                        },
                        new NameValuePairArgs{
                            Name = "Provider",
                            Value = "sqlserver",
                        }
                    },
                    Http20Enabled = true,
                },
            });
        }
    }
}
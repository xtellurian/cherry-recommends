using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Web.Inputs;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Insights;
using Pulumi.AzureNative.Authorization;
using System.Threading.Tasks;

namespace SignalBox.Azure
{
    partial class AppSvc : ComponentWithStorage
    {
        public Output<string?>? DotnetFunctionAppMasterKey { get; private set; }
        public Output<string?>? DotnetFunctionAppDefaultKey { get; private set; }
        public Output<string?>? PythonFunctionAppMasterKey { get; private set; }
        public Output<string?>? PythonFunctionAppDefaultKey { get; private set; }

        private WebApp CreateDotnetFuncs(ResourceGroup rg,
                                        MultitenantDatabaseComponent multiDb,
                                        Storage storage,
                                        Component insights,
                                        EventProcessing eventProcessing,
                                        AppServicePlan plan,
                                        Auth0 auth0)
        {
            var hubspotConfig = new Pulumi.Config("hubspot");
            var shopifyConfig = new Pulumi.Config("shopify");

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
                            // https://docs.microsoft.com/en-us/azure/azure-functions/functions-best-practices?tabs=csharp#worker-process-count
                            // this should be the same as the number of cores on the app service plan machine
                            Name = "FUNCTIONS_WORKER_PROCESS_COUNT",
                            Value = appSvcConfig.Get("process-count") ?? "1"
                        },
                        new NameValuePairArgs{
                            Name = "EventIngestionConnectionString",
                            Value = eventProcessing.PrimaryNamespaceReadConnectionString
                        },
                        new NameValuePairArgs{
                            Name = "CustomerHasUpdatedConnectionString", // for the event hubconsumer
                            Value = eventProcessing.PrimaryNamespaceReadConnectionString
                        },
                        // customer has updated
                        new NameValuePairArgs{
                            Name = "EventProcessing__CustomerHasUpdated__ConnectionString",
                            Value = eventProcessing.PrimaryNamespaceReadWriteConnectionString
                        },
                        new NameValuePairArgs{
                            Name = "EventProcessing__CustomerHasUpdated__EventhubName",
                            Value = eventProcessing.CustomerHasUpdatedHubName
                        },
                        new NameValuePairArgs{
                            Name = "AzureWebJobsStorage",
                            Value = storage.PrimaryConnectionString,
                        },
                        new NameValuePairArgs{
                            Name = "FUNCTIONS_EXTENSION_VERSION",
                            Value = "~4",
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
                        // shopify 
                        new NameValuePairArgs{
                            Name = "Shopify__AppCredentials__ApiKey",
                            Value = shopifyConfig.Get("apiKey") ?? "",
                        },
                        new NameValuePairArgs{
                            Name = "Shopify__AppCredentials__SecretKey",
                            Value = shopifyConfig.Get("secretKey") ?? "",
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
                        new NameValuePairArgs{ // for giving client access to tenant scopes
                            Name = "Auth0__M2M__ClientId",
                            Value = auth0.M2MClientId
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
                    LinuxFxVersion = "DOTNET-ISOLATED|6.0",
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

            var keys = Output.Tuple(dotnetFunctionApp.Name, dotnetFunctionApp.ResourceGroup, Output.CreateSecret(""))
               .Apply(GetHostKeys);

            this.DotnetFunctionAppMasterKey = keys.Apply(k => k?.MasterKey);

            this.DotnetFunctionAppDefaultKey = keys.Apply(k =>
            {
                if (k?.FunctionKeys != null && k.FunctionKeys.ContainsKey("default"))
                {
                    return k.FunctionKeys["default"];
                }
                else
                {
                    System.Console.WriteLine("No function Keys.");
                    return null;
                }
            });

            return dotnetFunctionApp;
        }

        // this fixes an issue
        // when the func app is initially created, there's an error thrown by this method
        // the func app must be deployed BEFORE this method calls.
        // therefore, we catch the error here the first time.
        private static async Task<ListWebAppHostKeysResult?> GetHostKeys((string, string, string) t)
        {
            try
            {
                return await ListWebAppHostKeys.InvokeAsync(new ListWebAppHostKeysArgs
                {
                    Name = t.Item1,
                    ResourceGroupName = t.Item2,
                });
            }
            catch
            {
                return null;
            }
        }

        private WebApp? CreatePythonFunctions(ResourceGroup rg,
                                                    Storage storage,
                                                    Component insights,
                                                    AppServicePlan plan)
        {
            var config = new Config();
            if (config.GetBoolean("deployPythonFunctions") == true)
            {
                var funcs = new WebApp("pythonJobs", new WebAppArgs
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
                            Value = "~4",
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
                        AlwaysOn = true,
                        LinuxFxVersion = "Python|3.7"
                    },
                });

                var keys = Output.Tuple(funcs.Name, funcs.ResourceGroup, Output.CreateSecret(""))
                   .Apply(GetHostKeys);

                this.PythonFunctionAppMasterKey = keys.Apply(k => k?.MasterKey);

                this.PythonFunctionAppDefaultKey = keys.Apply(k =>
                {
                    if (k?.FunctionKeys != null && k.FunctionKeys.ContainsKey("default"))
                    {
                        return k.FunctionKeys["default"];
                    }
                    else
                    {
                        System.Console.WriteLine("No function Keys.");
                        return null;
                    }
                });

                return funcs;
            }
            else
            {
                return null;
            }
        }
    }
}
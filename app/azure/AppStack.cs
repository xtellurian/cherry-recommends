using System.Collections.Generic;
using System.Threading.Tasks;
using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;

namespace SignalBox.Azure
{
    class AppStack : Stack
    {
        private string environment;

        public AppStack()
        {
            var config = new Config();
            this.environment = config.Require("environment");
            var networkConfig = new Config("network");
            var tags = new Dictionary<string, string>
            {
                {"Pulumi Stack", Pulumi.Deployment.Instance.StackName},
                {"Environment", environment}
            };

            var commonRgArgs = new ResourceGroupArgs
            {
                Tags = tags,
            };
            // Create Resource Groups
            var appRg = new ResourceGroup("application", commonRgArgs);
            var analyticsRg = new ResourceGroup("analytics", commonRgArgs);
            var databasesRg = new ResourceGroup("databases", commonRgArgs);

            var appInsights = new Pulumi.AzureNative.Insights.Component("Insights",
                new Pulumi.AzureNative.Insights.ComponentArgs
                {
                    ApplicationType = "web",
                    Kind = "web",
                    ResourceGroupName = appRg.Name,
                    Tags = tags,
                });

            var storage = new Storage(appRg);
            var multitenant = new MultitenantDatabaseComponent(databasesRg);

            // Create an Azure analytics environment
            var analytics = new AzureML(analyticsRg);
            var synapse = new AzureSynapse(analyticsRg);

            // create the app svcs
            var appSvc = new AppSvc(appRg, multitenant, storage, analytics, appInsights, tags);
            // set the stack outputs

            this.SqlServerAzureId = multitenant.Server.Id;
            this.SqlServerName = multitenant.ServerName;
            this.SqlServerUsername = Output.Create(multitenant.UserName);
            this.SqlServerPassword = multitenant.Password;
            this.SynapseUsername = Output.Create(synapse.UserName);
            this.SynapsePassword = synapse.Password;
            this.SynapsePrimaryStorageKey = synapse.PrimaryStorageKey;
            this.ElasticPoolName = multitenant.ElasticPool.Name;
            this.PrimaryStorageKey = analytics.PrimaryStorageKey;
            this.AppResourceGroup = appSvc.WebApp.ResourceGroup;
            this.CanonicalRootDomain = Output.Create(appSvc.CanonicalRootDomain);
            this.WebappName = appSvc.WebApp.Name;
            this.FunctionAppName = appSvc.FunctionApp.Name;
            this.DotnetFunctionAppName = appSvc.DotnetFunctionApp.Name;
            this.DotnetFunctionAppMasterKey = appSvc.DotnetFunctionAppMasterKey;
            this.DotnetFunctionAppDefaultKey = appSvc.DotnetFunctionAppDefaultKey;
            this.TenantDatabaseConnectionString = multitenant.TenantDbConnectionString;
            this.Multitenant = Output.Create(appSvc.Multitenant);

            // for DNS and SSL later in the deployment process
            // see app/deploy/DeploymentProcess.md
            this.NetworkSubdomain = Output.Create(networkConfig.Get("cherrySubdomain") ?? "");
            this.WebAppCustomDomainVerificationId = appSvc.WebApp.CustomDomainVerificationId;
            if (appSvc.CertificateOrder != null)
            {
                this.SslDomainVerificationToken = appSvc.CertificateOrder.DomainVerificationToken;
            }
            else
            {
                System.Console.WriteLine("SslDomainVerificationToken not created");
                this.SslDomainVerificationToken = Output.Create("");
            }
        }

        [Output]
        public Output<string> CanonicalRootDomain { get; set; }
        [Output]
        public Output<string> WebappName { get; set; }
        [Output]
        public Output<string> FunctionAppName { get; set; }
        [Output]
        public Output<string> DotnetFunctionAppName { get; set; }
        [Output]
        public Output<string?>? DotnetFunctionAppMasterKey { get; private set; }
        [Output]
        public Output<string?>? DotnetFunctionAppDefaultKey { get; private set; }

        // [Output]
        // public Output<string> FunctionAppDefaultKey { get; set;}
        [Output]
        public Output<string> AppResourceGroup { get; set; }

        [Output]
        public Output<string> PrimaryStorageKey { get; set; }
        [Output]
        public Output<string> TenantDatabaseConnectionString { get; private set; }
        [Output]
        public Output<string> ElasticPoolName { get; private set; }
        [Output]
        public Output<string> SqlServerAzureId { get; private set; }
        [Output]
        public Output<string> SqlServerName { get; private set; }
        [Output]
        public Output<string> SqlServerUsername { get; private set; }
        [Output]
        public Output<string> SqlServerPassword { get; private set; }
        [Output]
        public Output<string> SynapseUsername { get; private set; }
        [Output]
        public Output<string> SynapsePassword { get; private set; }
        [Output]
        public Output<string> SynapsePrimaryStorageKey { get; private set; }
        [Output]
        public Output<bool> Multitenant { get; private set; }
        [Output]
        public Output<string> NetworkSubdomain { get; private set; }
        [Output]
        public Output<string?> WebAppCustomDomainVerificationId { get; set; }
        [Output]
        public Output<string> SslDomainVerificationToken { get; private set; }

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
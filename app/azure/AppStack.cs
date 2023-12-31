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
            };

            var commonRgArgs = new ResourceGroupArgs
            {
                Tags = tags,
            };
            // Create Resource Groups
            var appRg = new ResourceGroup("application", commonRgArgs);
            var analyticsRg = new ResourceGroup("analytics", commonRgArgs);
            var databasesRg = new ResourceGroup("databases", commonRgArgs);
            var appObservation = new AppObservation(appRg, tags);
            var storage = new Storage(appRg);
            var multitenant = new MultitenantDatabaseComponent(databasesRg);
            var eventProcessing = new EventProcessing(appRg, tags);

            // Create an Azure analytics environment
            var synapse = new AzureSynapse(analyticsRg);
            var analytics = new AzureML(analyticsRg, synapse, multitenant);

            // create the app svcs
            var appSvc = new AppSvc(appRg, multitenant, storage, analytics, eventProcessing, appObservation, tags);
            // set the stack outputs

            this.SqlServerAzureId = multitenant.Server.Id;
            this.SqlServerName = multitenant.ServerName;
            this.SqlServerUsername = Output.Create(multitenant.UserName);
            this.SqlServerPassword = multitenant.Password;
            this.SqlServerAdminUserName = Output.Create(multitenant.AdminUserName);
            this.SqlServerAdminPassword = multitenant.AdminPassword;
            this.SqlServerReadUserName = Output.Create(multitenant.ReadUserName);
            this.SqlServerReadPassword = multitenant.ReadPassword;
            this.SynapseWorkspaceName = synapse.SynapseWorkspaceName;
            this.SynapseStorageAccountName = synapse.SynapseStorageAccountName;
            this.SynapseUsername = Output.Create(synapse.UserName);
            this.SynapsePassword = synapse.Password;
            this.SynapsePrimaryStorageKey = synapse.PrimaryStorageKey;
            this.ElasticPoolName = multitenant.ElasticPool.Name;
            this.AnalyticsKeyVaultName = analytics.AnalyticsKeyVaultName;
            this.PrimaryStorageKey = analytics.PrimaryStorageKey;
            this.AppResourceGroup = appSvc.WebApp.ResourceGroup;
            this.CanonicalRootDomain = Output.Create(appSvc.CanonicalRootDomain);
            this.WebappName = appSvc.WebApp.Name;
            this.PythonFunctionAppName = (appSvc.PythonFunctionApp == null) ? Output.Create("") : appSvc.PythonFunctionApp.Name;
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
        public Output<string> PythonFunctionAppName { get; set; }
        [Output]
        public Output<string> DotnetFunctionAppName { get; set; }
        [Output]
        public Output<string?>? DotnetFunctionAppMasterKey { get; private set; }
        [Output]
        public Output<string?>? DotnetFunctionAppDefaultKey { get; private set; }

        [Output]
        public Output<string> AppResourceGroup { get; set; }
        [Output]
        public Output<string> AnalyticsKeyVaultName { get; set; }
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
        public Output<string> SqlServerAdminUserName { get; private set; }
        [Output]
        public Output<string> SqlServerAdminPassword { get; private set; }
        [Output]
        public Output<string> SqlServerReadUserName { get; private set; }
        [Output]
        public Output<string> SqlServerReadPassword { get; private set; }
        [Output]
        public Output<string> SynapseWorkspaceName { get; set; }
        [Output]
        public Output<string> SynapseStorageAccountName { get; set; }
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
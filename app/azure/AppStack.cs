using System.Collections.Generic;
using System.Threading.Tasks;
using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;

namespace SignalBox.Azure
{
    class AppStack : Stack
    {
        public AppStack()
        {
            var config = new Config();
            var tags = new Dictionary<string, string>
            {
                {"Pulumi", "true"},
                {"Pulumi Stack", Pulumi.Deployment.Instance.StackName},
                {"Pulumi Project", Pulumi.Deployment.Instance.ProjectName},
                {"Environment", config.Require("environment")}
            };

            var commonRgArgs = new ResourceGroupArgs
            {
                Tags = tags,
            };
            // Create Resource Groups
            var appRg = new ResourceGroup("application", commonRgArgs);
            var analyticsRg = new ResourceGroup("analytics", commonRgArgs);

            var appInsights = new Pulumi.AzureNative.Insights.Component("Insights",
                new Pulumi.AzureNative.Insights.ComponentArgs
                {
                    ApplicationType = "web",
                    Kind = "web",
                    ResourceGroupName = appRg.Name,
                    Tags = tags
                });

            var storage = new Storage(appRg);
            var db = new DatabaseComponent(appRg);

            // Create an Azure analytics environment
            var analytics = new AzureML(analyticsRg, db);

            // create the app svcs
            var appSvc = new AppSvc(appRg, db, storage, analytics, appInsights);

            // set the stack outputs
            this.DatabaseConnectionString = db.DatabaseConnectionString;
            this.PrimaryStorageKey = analytics.PrimaryStorageKey;
            this.AppResourceGroup = appSvc.WebApp.ResourceGroup;
            this.WebappName = appSvc.WebApp.Name;
            this.FunctionAppName = appSvc.FunctionApp.Name;
            // this.FunctionAppDefaultKey = appSvc.FunctionAppDefaultKey;
        }

        [Output]
        public Output<string> WebappName { get; set; }
        [Output]
        public Output<string> FunctionAppName { get; set; }
        // [Output]
        // public Output<string> FunctionAppDefaultKey { get; set;}
        [Output]
        public Output<string> AppResourceGroup { get; set; }

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
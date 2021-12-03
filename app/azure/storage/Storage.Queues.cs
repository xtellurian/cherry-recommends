using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;

namespace SignalBox.Azure
{
    partial class Storage : ComponentWithStorage
    {

        protected void CreateQueues(ResourceGroup rg, StorageAccount storageAccount)
        {
            var trackedUserQueue = new Queue("newTrackedUsers", new QueueArgs
            {
                QueueName = SignalBox.Core.Constants.AzureQueueNames.NewTrackedUsers,
                AccountName = storageAccount.Name,
                ResourceGroupName = rg.Name
            });

            var trackedUserEventsQueue = new Queue("trackedUserEvents", new QueueArgs
            {
                QueueName = SignalBox.Core.Constants.AzureQueueNames.TrackedUserEvents,
                AccountName = storageAccount.Name,
                ResourceGroupName = rg.Name
            });

            var newTenantsQueue = new Queue("newTenantsQ", new QueueArgs
            {
                QueueName = SignalBox.Core.Constants.AzureQueueNames.NewTenants,
                AccountName = storageAccount.Name,
                ResourceGroupName = rg.Name
            });

            var newTenantMembershipsQueue = new Queue("newTntMembershipsQ", new QueueArgs
            {
                QueueName = SignalBox.Core.Constants.AzureQueueNames.NewTenantMemberships,
                AccountName = storageAccount.Name,
                ResourceGroupName = rg.Name
            });

            var runFeatureGeneratorsQueue = new Queue("runAllFeatGensQ", new QueueArgs
            {
                QueueName = SignalBox.Core.Constants.AzureQueueNames.RunAllFeatureGenerators,
                AccountName = storageAccount.Name,
                ResourceGroupName = rg.Name
            });

            var runSingleFeatureGeneratorQueue = new Queue("runFeatGenQ", new QueueArgs
            {
                QueueName = SignalBox.Core.Constants.AzureQueueNames.RunFeatureGenerator,
                AccountName = storageAccount.Name,
                ResourceGroupName = rg.Name
            });
        }
    }
}
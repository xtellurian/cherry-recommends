using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;

namespace SignalBox.Azure
{
    partial class Storage : ComponentWithStorage
    {

        protected void CreateQueues(ResourceGroup rg, StorageAccount storageAccount)
        {
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

            var runMetricGeneratorsQueue = new Queue("runAllFeatGensQ", new QueueArgs
            {
                QueueName = SignalBox.Core.Constants.AzureQueueNames.RunAllMetricGenerators,
                AccountName = storageAccount.Name,
                ResourceGroupName = rg.Name
            });

            var runSingleMetricGeneratorQueue = new Queue("runFeatGenQ", new QueueArgs
            {
                QueueName = SignalBox.Core.Constants.AzureQueueNames.RunMetricGenerator,
                AccountName = storageAccount.Name,
                ResourceGroupName = rg.Name
            });
            var runAllSegmentEnrolments = new Queue("runAllSegmentEnrolments", new QueueArgs
            {
                QueueName = SignalBox.Core.Constants.AzureQueueNames.RunAllSegmentEnrolmentRules,
                AccountName = storageAccount.Name,
                ResourceGroupName = rg.Name
            });

            var runSegmentEnrolments = new Queue("runSegmentEnrolment", new QueueArgs
            {
                QueueName = SignalBox.Core.Constants.AzureQueueNames.RunSegmentEnrolmentRule,
                AccountName = storageAccount.Name,
                ResourceGroupName = rg.Name
            });
        }
    }
}
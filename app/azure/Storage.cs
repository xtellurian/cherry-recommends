
using System.Threading.Tasks;
using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;

namespace SignalBox.Azure
{
    class Storage : ComponentWithStorage
    {
        private static InputMap<string> tags = new InputMap<string>
            {
                {"Component", "AzureML"}
            };

        public Storage(ResourceGroup rg)
        {
            var storageAccount = new StorageAccount("funcstor", new StorageAccountArgs
            {
                ResourceGroupName = rg.Name,
                Sku = new SkuArgs
                {
                    Name = SkuName.Standard_LRS
                },
                Kind = Kind.StorageV2,
                Tags = tags
            });

            var queueMessageBlobContainer = new BlobContainer("queueMessages", new BlobContainerArgs
            {
                AccountName = storageAccount.Name,
                ContainerName = "queue-messages",
                ResourceGroupName = rg.Name,
            });

            var trackedUserQueue = new Queue("newTrackedUsers", new QueueArgs
            {
                QueueName = "new-tracked-users",
                AccountName = storageAccount.Name,
                ResourceGroupName = rg.Name
            });
            
            var trackedUserEventsQueue = new Queue("trackedUserEvents", new QueueArgs
            {
                QueueName = "tracked-user-events",
                AccountName = storageAccount.Name,
                ResourceGroupName = rg.Name
            });

            this.StorageAccount = storageAccount;

            this.PrimaryStorageKey = Output.Tuple(rg.Name, storageAccount.Name).Apply(names =>
               Output.CreateSecret(GetStorageAccountPrimaryKey(names.Item1, names.Item2)));

            this.PrimaryConnectionString = Output.Tuple(rg.Name, storageAccount.Name).Apply(names =>
                Output.CreateSecret(GetStorageAccountPrimaryConnectionString(names.Item1, names.Item2)));
        }

        public StorageAccount StorageAccount { get; }
        public Output<string> PrimaryStorageKey { get; }
        public Output<string> PrimaryConnectionString { get; }


    }
}




using System.Collections.Generic;
using System.Threading.Tasks;
using Pulumi;
using Pulumi.AzureNative.EventHub;
using Pulumi.AzureNative.Resources;
using SignalBox.Core.Constants;

namespace SignalBox.Azure
{
    partial class EventProcessing : ComponentWithStorage
    {
        public EventProcessing(ResourceGroup rg, Dictionary<string, string> tags)
        {
            var ns = new Namespace("eventIngestNs", new NamespaceArgs
            {
                Location = rg.Location,
                ResourceGroupName = rg.Name,
                Sku = new Pulumi.AzureNative.EventHub.Inputs.SkuArgs
                {
                    Name = "Standard",
                    Tier = "Standard",
                },
                Tags = tags
            });

            var nsWrite = new NamespaceAuthorizationRule("nsWrite",
                new NamespaceAuthorizationRuleArgs
                {
                    AuthorizationRuleName = "write",
                    NamespaceName = ns.Name,
                    ResourceGroupName = rg.Name,
                    Rights =
                    {
                        "Send",
                    },
                });
            var nsRead = new NamespaceAuthorizationRule("nsRead",
                new NamespaceAuthorizationRuleArgs
                {
                    AuthorizationRuleName = "read",
                    NamespaceName = ns.Name,
                    ResourceGroupName = rg.Name,
                    Rights =
                    {
                        "Listen",
                    },
                });
            var nsReadWrite = new NamespaceAuthorizationRule("nsRW",
                new NamespaceAuthorizationRuleArgs
                {
                    AuthorizationRuleName = "readwrite",
                    NamespaceName = ns.Name,
                    ResourceGroupName = rg.Name,
                    Rights =
                    {
                        "Listen",
                        "Send",
                    },
                });


            var eventProcessingHub = new EventHub("rawEvents", new EventHubArgs
            {
                EventHubName = AzureEventhubNames.EventIngestion,
                MessageRetentionInDays = 7,
                NamespaceName = ns.Name,
                PartitionCount = 4,
                ResourceGroupName = rg.Name,
                Status = EntityStatus.Active,
            });

            var monitoringCg = new ConsumerGroup("monitorCg", new ConsumerGroupArgs
            {
                ConsumerGroupName = AzureEventhubConsumerGroups.Monitoring,
                EventHubName = eventProcessingHub.Name,
                NamespaceName = ns.Name,
                ResourceGroupName = rg.Name,
            });

            var customerHasUpdatedHub = new EventHub("customerUpdated", new EventHubArgs
            {
                EventHubName = AzureEventhubNames.CustomerHasUpdated,
                MessageRetentionInDays = 3,
                NamespaceName = ns.Name,
                PartitionCount = 4,
                ResourceGroupName = rg.Name,
                Status = EntityStatus.Active,
            });

            var monitorCustomerUpdatedCg = new ConsumerGroup("monitorCusUpdCg", new ConsumerGroupArgs
            {
                ConsumerGroupName = AzureEventhubConsumerGroups.Monitoring,
                EventHubName = customerHasUpdatedHub.Name,
                NamespaceName = ns.Name,
                ResourceGroupName = rg.Name,
            });

            this.EventProcessingHubName = eventProcessingHub.Name;
            this.CustomerHasUpdatedHubName = customerHasUpdatedHub.Name;

            this.PrimaryNamespaceWriteConnectionString = Output.Tuple(nsWrite.Name, ns.Name, rg.Name).Apply(args =>
                    Output.CreateSecret(GetNamespacePrimaryConnectionString(args.Item1, args.Item2, args.Item3)));

            this.PrimaryNamespaceReadConnectionString = Output.Tuple(nsRead.Name, ns.Name, rg.Name).Apply(args =>
                   Output.CreateSecret(GetNamespacePrimaryConnectionString(args.Item1, args.Item2, args.Item3)));
            this.PrimaryNamespaceReadWriteConnectionString = Output.Tuple(nsReadWrite.Name, ns.Name, rg.Name).Apply(args =>
                   Output.CreateSecret(GetNamespacePrimaryConnectionString(args.Item1, args.Item2, args.Item3)));


        }

        public Output<string> EventProcessingHubName { get; }
        public Output<string> CustomerHasUpdatedHubName { get; }
        public Output<string> PrimaryNamespaceReadConnectionString { get; }
        public Output<string> PrimaryNamespaceWriteConnectionString { get; }
        public Output<string> PrimaryNamespaceReadWriteConnectionString { get; }

        protected static async Task<string> GetNamespacePrimaryConnectionString(string authorizationRuleName,
                                                                                        string namespaceName,
                                                                                        string resourceGroupName)
        {
            var keys = await ListNamespaceKeys.InvokeAsync(new ListNamespaceKeysArgs
            {
                AuthorizationRuleName = authorizationRuleName,
                NamespaceName = namespaceName,
                ResourceGroupName = resourceGroupName

            });
            return keys.PrimaryConnectionString;
        }
        protected static async Task<string> GetEventhubPrimaryConnectionString(string authorizationRuleName,
                                                                                        string eventhubName,
                                                                                        string namespaceName,
                                                                                        string resourceGroupName)
        {
            var keys = await ListEventHubKeys.InvokeAsync(new ListEventHubKeysArgs
            {
                AuthorizationRuleName = authorizationRuleName,
                EventHubName = eventhubName,
                NamespaceName = namespaceName,
                ResourceGroupName = resourceGroupName

            });
            return keys.PrimaryConnectionString;
        }
    }
}
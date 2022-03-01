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


            var hub = new EventHub("rawEvents", new EventHubArgs
            {
                EventHubName = AzureEventhubNames.EventIngestion,
                MessageRetentionInDays = 4,
                NamespaceName = ns.Name,
                PartitionCount = 4,
                ResourceGroupName = rg.Name,
                Status = EntityStatus.Active,
            });

            var monitoringCg = new ConsumerGroup("monitorCg", new ConsumerGroupArgs
            {
                ConsumerGroupName = AzureEventhubConsumerGroups.Monitoring,
                EventHubName = hub.Name,
                NamespaceName = ns.Name,
                ResourceGroupName = rg.Name,
            });

            this.EventhubName = hub.Name;

            this.PrimaryNamespaceWriteConnectionString = Output.Tuple(nsWrite.Name, ns.Name, rg.Name).Apply(args =>
                    Output.CreateSecret(GetNamespacePrimaryConnectionString(args.Item1, args.Item2, args.Item3)));

            this.PrimaryNamespaceReadConnectionString = Output.Tuple(nsRead.Name, ns.Name, rg.Name).Apply(args =>
                   Output.CreateSecret(GetNamespacePrimaryConnectionString(args.Item1, args.Item2, args.Item3)));


        }

        public Output<string> EventhubName { get; }
        public Output<string> PrimaryNamespaceReadConnectionString { get; }
        public Output<string> PrimaryNamespaceWriteConnectionString { get; }

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
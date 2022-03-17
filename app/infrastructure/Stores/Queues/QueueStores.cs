using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Queues
{
    public class QueueStores : IQueueStores
    {
        private readonly INewTenantMembershipQueueStore newTenantMembershipQueueStore;
        private readonly IRunMetricGeneratorQueueStore runMetricGeneratorQueueStore;
        private readonly IRunSegmentEnrolmentRuleQueueStore runSegmentEnrolmentRuleQueueStore;

        public QueueStores(INewTenantMembershipQueueStore newTenantMembershipQueueStore,
                           IRunMetricGeneratorQueueStore runMetricGeneratorQueueStore,
                           IRunSegmentEnrolmentRuleQueueStore runSegmentEnrolmentRuleQueueStore)
        {
            this.newTenantMembershipQueueStore = newTenantMembershipQueueStore;
            this.runMetricGeneratorQueueStore = runMetricGeneratorQueueStore;
            this.runSegmentEnrolmentRuleQueueStore = runSegmentEnrolmentRuleQueueStore;
        }

        public async Task Enqueue<T>(T message) where T : IQueueMessage
        {
            if (message is NewTenantMembershipQueueMessage newTenantMembershipQueueMessage)
            {
                await newTenantMembershipQueueStore.Enqueue(newTenantMembershipQueueMessage);
            }
            else if (message is RunMetricGeneratorQueueMessage runMetricGeneratorQueueMessage)
            {
                await runMetricGeneratorQueueStore.Enqueue(runMetricGeneratorQueueMessage);
            }
            else if (message is RunSegmentEnrolmentRuleQueueMessage runSegmentEnrolmentRuleQueueMessage)
            {
                await runSegmentEnrolmentRuleQueueStore.Enqueue(runSegmentEnrolmentRuleQueueMessage);
            }
        }
    }
}
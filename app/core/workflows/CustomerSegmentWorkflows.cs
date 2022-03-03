using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SignalBox.Core.Workflows
{
    public class CustomerSegmentWorkflows : IWorkflow
    {
        private readonly ILogger<CustomerSegmentWorkflows> logger;
        private readonly ISegmentStore segmentStore;
        private readonly ICustomerStore customerStore;
        private readonly IStorageContext storageContext;

        public CustomerSegmentWorkflows(ILogger<CustomerSegmentWorkflows> logger,
                                ISegmentStore segmentStore,
                                ICustomerStore customerStore,
                                IStorageContext storageContext)
        {
            this.logger = logger;
            this.segmentStore = segmentStore;
            this.customerStore = customerStore;
            this.storageContext = storageContext;
        }
        public async Task<Segment> CreateSegment(string name)
        {
            logger.LogDebug($"Creating Segment with name {name}");
            var segment = await segmentStore.Create(new Segment(name));
            logger.LogDebug($"Created Segment with id {segment.Id}");
            await storageContext.SaveChanges();
            return segment;
        }
    }
}
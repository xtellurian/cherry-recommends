using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SignalBox.Core.Workflows
{
    public class CustomerSegmentWorkflows : IWorkflow, ICustomerSegmentWorkflow
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

        public async Task<CustomerSegment> AddToSegment(Segment segment, Customer customer)
        {
            var customerSegment = await segmentStore.AddCustomer(segment, customer);
            logger.LogInformation("Added customer {customerId} to segment {segmentId}", customer.Id, segment.Id);
            await storageContext.SaveChanges();
            return customerSegment;
        }

        public async Task RemoveFromSegment(Segment segment, Customer customer)
        {
            var customerSegment = await segmentStore.RemoveCustomer(segment, customer);
            logger.LogInformation("Removed customer {customerId} from segment {segmentId}", customer.Id, segment.Id);
            await storageContext.SaveChanges();
        }
    }
}
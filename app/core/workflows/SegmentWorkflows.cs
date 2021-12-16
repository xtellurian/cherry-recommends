using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SignalBox.Core.Workflows
{
    public class SegmentWorkflows : IWorkflow
    {
        private readonly ILogger<SegmentWorkflows> logger;
        private readonly ISegmentStore segmentStore;
        private readonly ICustomerStore customerStore;
        private readonly IStorageContext storageContext;

        public SegmentWorkflows(ILogger<SegmentWorkflows> logger,
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

        public Task ProcessRule(Rule rule, List<CustomerEvent> events)
        {
            throw new System.NotImplementedException();
            // var segment = await segmentStore.Read(rule.SegmentId);
            // var ruleEvents = events.Where(_ => _.Key == rule.EventKey);
            // if (ruleEvents.Any())
            // {
            //     await ProcessEventsForSegmentingRule(segment, ruleEvents);
            //     await segmentStore.Update(segment);
            //     await storageContext.SaveChanges();
            // }
        }

        private async Task ProcessEventsForSegmentingRule(Segment segment, IEnumerable<CustomerEvent> events)
        {
            foreach (var e in events)
            {
                Customer customer;
                if (await customerStore.ExistsFromCommonId(e.CommonUserId))
                {
                    customer = await customerStore.ReadFromCommonId(e.CommonUserId);
                }
                else
                {
                    // means the user doesn't exist yet. create them.
                    customer = await customerStore.Create(new Customer(e.CommonUserId));
                }

                segment.InSegment.Add(customer);
            }
        }
    }
}
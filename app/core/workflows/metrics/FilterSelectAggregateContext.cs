using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalBox.Core.Metrics.Generators
{
    public class FilterSelectAggregateContext
    {
        private readonly ICustomerEventStore eventStore;

        public FilterSelectAggregateContext(Customer customer, Metric metric, ICustomerEventStore eventStore)
        {
            Customer = customer;
            Metric = metric;
            this.eventStore = eventStore;
        }

        public List<FilterSelectAggregateStep> Steps { get; set; }
        public Customer Customer { get; set; }
        public Metric Feature => Metric;
        public Metric Metric { get; }
        public List<CustomerEvent> Events { get; private set; }

        public async Task LoadEventsIntoContext(FilterStep filter = null)
        {
            var isAllEvents = filter?.EventTypeMatch == null;
            var result = await eventStore.ReadEventsForUser(Customer, new EventQueryOptions
            {
                Filter = _ => isAllEvents || (_.EventType == filter.EventTypeMatch)
            });

            this.Events = result.ToList();
        }
    }
}
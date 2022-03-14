using System;
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

        public FilterSelectAggregateContext(Business business, Metric metric, ICustomerEventStore eventStore)
        {
            Business = business;
            Metric = metric;
            this.eventStore = eventStore;
        }

        public List<FilterSelectAggregateStep> Steps { get; set; }
        public Customer Customer { get; set; }
        public Business Business { get; set; }
        public Metric Feature => Metric;
        public Metric Metric { get; }
        public List<CustomerEvent> Events { get; private set; }

        public async Task LoadEventsIntoContext(FilterStep filter = null, DateTimeOffset? since = null)
        {
            if (Customer != null)
            {
                var isAllEvents = filter?.EventTypeMatch == null;
                var result = await eventStore.ReadEventsForUser(Customer, new EventQueryOptions
                {
                    Filter = _ => isAllEvents || (_.EventType == filter.EventTypeMatch)
                }, since);

                this.Events = result.ToList();
            }
            else
            {
                this.Events = new List<CustomerEvent>();
            }
        }
        public async Task LoadBusinessEventsIntoContext(FilterStep filter = null, DateTimeOffset? since = null)
        {
            if (Business != null)
            {
                var isAllEvents = filter?.EventTypeMatch == null;
                var result = await eventStore.ReadEventsForBusiness(Business, new EventQueryOptions
                {
                    Filter = _ => isAllEvents || (_.EventType == filter.EventTypeMatch)
                }, since);

                this.Events = result.ToList();
            }
            else
            {
                this.Events = new List<CustomerEvent>();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SignalBox.Core.Workflows
{
    public class CustomerEventsWorkflows : IWorkflow, ICustomerEventsWorkflow
    {
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ITelemetry telemetry;
        private readonly ILogger<CustomerEventsWorkflows> logger;
        private readonly ICustomerWorkflow customerWorkflow;
        private readonly IEnvironmentProvider environmentProvider;
        private readonly IIntegratedSystemStore integratedSystemStore;
        private readonly ICustomerEventStore customerEventStore;
        private readonly IBusinessWorkflow businessWorkflow;
        private readonly ICustomerEventIngestor eventIngestor;
        private readonly IOfferWorkflow offerWorkflow;

        public JsonSerializerOptions SerializerOptions => new JsonSerializerOptions();
        public CustomerEventsWorkflows(
            IDateTimeProvider dateTimeProvider,
            ITelemetry telemetry,
            ILogger<CustomerEventsWorkflows> logger,
            ICustomerWorkflow customerWorkflow,
            IEnvironmentProvider environmentProvider,
            IIntegratedSystemStore integratedSystemStore,
            ICustomerEventStore customerEventStore,
            IBusinessWorkflow businessWorkflow,
            ICustomerEventIngestor eventIngestor,
            IOfferWorkflow offerWorkflow)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.telemetry = telemetry;
            this.logger = logger;
            this.customerWorkflow = customerWorkflow;
            this.environmentProvider = environmentProvider;
            this.integratedSystemStore = integratedSystemStore;
            this.customerEventStore = customerEventStore;
            this.businessWorkflow = businessWorkflow;
            this.eventIngestor = eventIngestor;
            this.offerWorkflow = offerWorkflow;
        }

        public async Task<EventLoggingResponse> Ingest(IEnumerable<CustomerEventInput> input)
        {
            // ingest by default
            if (eventIngestor.CanIngest)
            {
                var eventCount = input.Count();
                telemetry.TrackEvent("CustomerEventsWorkflows.Ingest", new Dictionary<string, string> { { "eventCount", eventCount.ToString() } });
                await eventIngestor.Ingest(input);
                return new EventLoggingResponse
                {
                    EventsEnqueued = eventCount
                };
            }
            else
            {
                // process directly
                logger.LogWarning("Event Ingestor is not available for ingestion. Processing events directly.");
                return await ProcessEvents(input);
            }
        }
        public async Task<EventLoggingResponse> ProcessEvents(IEnumerable<CustomerEventInput> input)
        {
            try
            {
                logger.LogInformation("Processing Events");
                var stopwatch = telemetry.NewStopwatch(true);

                var customers = await CreateOrGetCustomers(input);
                await SetCustomerProperties(input, customers);
                var events = await CreateCustomerEvents(input, customers);

                // save changes
                await customerEventStore.Context.SaveChanges();

                // check if we should add any customer to a business
                await AddToBusinesses(input, customers);

                // log some useful metrics
                var minTimestamp = events.Min(_ => _.Timestamp);
                telemetry.TrackMetric("CustomerEventsWorkflows.ProcessEvents.ElapsedMilliseconds", stopwatch.ElapsedMilliseconds);
                telemetry.TrackMetric("CustomerEventsWorkflows.ProcessEvents.TimestampDelta", (dateTimeProvider.Now - minTimestamp).TotalMilliseconds);
                return new EventLoggingResponse { EventsProcessed = events.Count() };
            }
            catch (Exception ex)
            {
                telemetry.TrackException(ex);
                throw new AggregateException("Error Processing Events", ex);
            }
        }

        private async Task SetCustomerProperties(IEnumerable<CustomerEventInput> input, IEnumerable<Customer> customers)
        {
            foreach (var customer in customers)
            {
                var identifyEvents = input
                    .Where(_ => _.CustomerId == customer.CustomerId && _.Kind == EventKinds.Identify)
                    .ToList();
                if (identifyEvents.Any())
                {
                    customer.Properties ??= new DynamicPropertyDictionary();
                    foreach (var e in identifyEvents)
                    {
                        customer.Properties.Merge(e.Properties);
                    }
                }
                foreach (var k in customer.Properties.Keys)
                {
                    switch (k)
                    {
                        case "firstName":
                            customer.Name = customer.Properties[k]?.ToString();
                            break;
                        case "email":
                            customer.Email = customer.Properties[k]?.ToString();
                            break;
                        default:
                            break;
                    }
                }
            }

            await customerWorkflow.UpdateAndSave(customers);
        }

        private async Task AddToBusinesses(IEnumerable<CustomerEventInput> input, IEnumerable<Customer> customers)
        {
            if (input.Any(_ => _.Kind == EventKinds.AddToBusiness))
            {
                foreach (var e in input.Where(_ => _.Kind == EventKinds.AddToBusiness))
                {
                    await businessWorkflow.AddToBusiness(e.BusinessCommonId, customers.First(_ => _.CustomerId == e.CustomerId), e.Properties);
                }
            }
        }

        private async Task<IEnumerable<Customer>> CreateOrGetCustomers(IEnumerable<CustomerEventInput> input)
        {
            var lastUpdated = dateTimeProvider.Now;
            var customers = await customerWorkflow.CreateOrUpdate(input.Select(_ => new PendingCustomer(_.CustomerId, _.EnvironmentId, null, false)).Distinct());
            if (customers.Any())
            {
                foreach (var u in customers)
                {
                    u.LastUpdated = lastUpdated;
                }
            }

            return customers;
        }

        private async Task<IEnumerable<CustomerEvent>> CreateCustomerEvents(IEnumerable<CustomerEventInput> input, IEnumerable<Customer> customers)
        {
            var events = new List<CustomerEvent>();
            foreach (var d in input)
            {
                // make sure you set this before loading the integrated system.
                if (d.EnvironmentId != null)
                {
                    environmentProvider.SetOverride(d.EnvironmentId.Value);
                }

                IntegratedSystem sourceSystem = null;
                if (d.SourceSystemId != null)
                {
                    sourceSystem = await integratedSystemStore.Read(d.SourceSystemId.Value);
                }

                var customer = customers.First(_ => _.CommonId == d.CustomerId);
                customer.LastUpdated = dateTimeProvider.Now; // user has been updated.
                var customerEvent = new CustomerEvent(customer,
                                                d.EventId,
                                                d.Timestamp ?? dateTimeProvider.Now,
                                                sourceSystem,
                                                d.Kind,
                                                d.EventType,
                                                d.Properties,
                                                d.RecommendationCorrelatorId);
                events.Add(customerEvent);

                // Check to see if an offer was redeemed
                await offerWorkflow.UpdateOffer(customerEvent);
            }

            return await customerEventStore.AddRange(events);
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Metrics.Destinations;

namespace SignalBox.Core.Workflows
{
    public class MetricWorkflows : MetricWorkflowBase, IWorkflow
    {
        private readonly IIntegratedSystemStore systemStore;
        private readonly ICustomerStore trackedUserStore;


        public MetricWorkflows(IMetricStore metricStore,
                                   IHistoricCustomerMetricStore customerMetricStore,
                                   IIntegratedSystemStore systemStore,
                                   RecommenderTriggersWorkflows triggersWorkflows,
                                   HubspotPushWorkflows hubspotPushWorkflows,
                                   IWebhookSenderClient webhookSenderClient,
                                   ICustomerStore trackedUserStore,
                                   ITelemetry telemetry,
                                   ILogger<MetricWorkflows> logger)
                                   : base(metricStore, customerMetricStore, triggersWorkflows, hubspotPushWorkflows, webhookSenderClient, telemetry, logger)
        {
            this.systemStore = systemStore;
            this.trackedUserStore = trackedUserStore;
        }

        public async Task<Metric> CreateMetric(string commonId, string name)
        {
            var metric = await metricStore.Create(new Metric(commonId, name));
            await metricStore.Context.SaveChanges();
            return metric;
        }

        public async Task<Paginated<Customer>> GetCustomers(Metric metric, int page)
        {
            return await metricStore.QueryCustomers(page, metric.Id);
        }

        public async Task<HistoricCustomerMetric> ReadCustomerMetric(Customer customer, string metricCommonId, int? version = null)
        {
            var metric = await metricStore.ReadFromCommonId(metricCommonId);
            return await customerMetricStore.ReadCustomerMetric(customer, metric, version);
        }

        // ------ ADD/REMOVE DESTINATIONS -----
        public async Task<MetricDestinationBase> RemoveDestination(Metric metric, long destinationId)
        {
            await metricStore.LoadMany(metric, _ => _.Destinations);
            var destination = metric.Destinations.FirstOrDefault(_ => _.Id == destinationId);
            if (destination == null)
            {
                throw new BadRequestException($"Destination Id {destinationId} is not a destination of Recommender Id {metric.Id}");
            }

            metric.Destinations.Remove(destination);
            await metricStore.Context.SaveChanges();
            return destination;
        }

        public async Task<MetricDestinationBase> AddDestination(Metric metric,
                                                    long systemId,
                                                    string destinationType,
                                                    string endpoint = null,
                                                    string propertyName = null)
        {
            var maxDestinations = 5;
            var system = await systemStore.Read(systemId);
            await metricStore.LoadMany(metric, _ => _.Destinations);

            if (metric.Destinations.Count > maxDestinations)
            {
                throw new BadRequestException($"The maximum number of destinations is {maxDestinations}");
            }

            MetricDestinationBase destination;
            switch (destinationType)
            {
                case null:
                    throw new BadRequestException("DestinationType cannot be null");
                case MetricDestinationBase.WebhookDestinationType:
                    destination = new WebhookMetricDestination(metric, system, endpoint);
                    break;
                case MetricDestinationBase.HubspotContactPropertyDestinationType:
                    destination = new HubspotContactPropertyMetricDestination(metric, system, propertyName);
                    break;
                default:
                    throw new BadRequestException($"DestinationType {destinationType} is an unknown type");

            }


            metric.Destinations.Add(destination);
            await metricStore.Context.SaveChanges();

            if (destination.Discriminator == nameof(MetricDestinationBase))
            {
                metric.Destinations.Remove(destination);
                await metricStore.Context.SaveChanges();

                throw new ConfigurationException($"Could not create destination of type {destination.GetType().Name}. You may need a database migration");
            }

            return destination;
        }
    }
}
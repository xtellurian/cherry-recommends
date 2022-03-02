using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Metrics;
using SignalBox.Core.Metrics.Destinations;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/Features")]
    [Route("api/[controller]")]
    public class MetricsController : CommonEntityControllerBase<Metric>
    {
        private readonly ILogger<MetricsController> logger;
        private readonly IHistoricCustomerMetricStore customerMetricStore;
        private readonly MetricWorkflows workflows;
        private readonly MetricGeneratorWorkflows generatorWorkflows;

        public MetricsController(
            ILogger<MetricsController> logger,
            IMetricStore store,
            IHistoricCustomerMetricStore customerMetricStore,
            MetricWorkflows workflows,
            MetricGeneratorWorkflows generatorWorkflows) : base(store)
        {
            this.logger = logger;
            this.customerMetricStore = customerMetricStore;
            this.workflows = workflows;
            this.generatorWorkflows = generatorWorkflows;
        }

        /// <summary>Returned a paginated list of items for this resource.</summary>
        [HttpGet]
        public override async Task<Paginated<Metric>> Query([FromQuery] PaginateRequest p, [FromQuery] SearchEntities q)
        {
            if (Enum.TryParse<MetricScopes>(q.Scope, ignoreCase: true, out var metricScope))
            {
                return await store.Query(new EntityStoreQueryOptions<Metric>(p.Page, _ => _.Scope == metricScope));
            }
            else
            {
                return await base.Query(p, q);
            }
        }

        /// <summary>Creates a new generic Metric that can used on any Customer.</summary>
        [HttpPost]
        public async Task<Metric> CreateMetric([FromBody] CreateMetric dto)
        {
            dto.Validate();
            return await workflows.CreateMetric(dto.CommonId, dto.Name, dto.ValueType, dto.Scope);
        }

        /// <summary>Get's the Customers associated with a Metric.</summary>
        [HttpGet("{id}/TrackedUsers")]
        [HttpGet("{id}/Customers")]
        public async Task<Paginated<Customer>> GetAssociatedCustomers(string id, [FromQuery] PaginateRequest p)
        {
            var metric = await base.GetResource(id);
            return await workflows.GetCustomers(metric, p.Page);
        }

        /// <summary>Get's the Generators associated with the Feature.</summary>
        [HttpGet("{id}/Generators")]
        public async Task<Paginated<MetricGenerator>> GetGenerators(string id, [FromQuery] PaginateRequest p)
        {
            var metric = await base.GetResource(id);
            return await generatorWorkflows.GetGenerators(p.Page, metric);
        }

        /// <summary>Get's the latest Customer Metrics(values) for a Metric.</summary>
        [HttpGet("{id}/TrackedUserFeatures")]
        [HttpGet("{id}/CustomerMetrics")]
        public async Task<Paginated<HistoricCustomerMetric>> GetLatestCustomerMetrics(string id, [FromQuery] PaginateRequest p)
        {
            var metric = await base.GetResource(id);
            var customers = await workflows.GetCustomers(metric, p.Page);
            var metricValues = new List<HistoricCustomerMetric>();
            foreach (var trackedUser in customers.Items)
            {
                metricValues.Add(await workflows.ReadCustomerMetric(trackedUser, metric.CommonId));
            }

            return new Paginated<HistoricCustomerMetric>(metricValues, customers.Pagination.PageCount, customers.Pagination.TotalItemCount, customers.Pagination.PageNumber);
        }

        [HttpGet("{id}/AggregateMetricValuesNumeric")]
        public async Task<IEnumerable<CustomerMetricWeeklyNumericAggregate>> AggregateMetricValuesNumeric(string id)
        {
            var metric = await base.GetResource(id);
            var aggregateMetricValues = await customerMetricStore.GetAggregateMetricValuesNumeric(metric);
            return aggregateMetricValues;
        }

        [HttpGet("{id}/AggregateMetricValuesString")]
        public async Task<IEnumerable<CustomerMetricWeeklyStringAggregate>> AggregateMetricValuesString(string id)
        {
            var metric = await base.GetResource(id);
            var aggregateMetricValues = await customerMetricStore.GetAggregateMetricValuesString(metric);
            return aggregateMetricValues;
        }

        [HttpGet("{id}/NumericMetricBinValues")]
        public async Task<IEnumerable<MetricDailyBinValueNumeric>> NumericMetricBinValues(string id, int? binCount)
        {
            var metric = await base.GetResource(id);
            var numericBinValues = await customerMetricStore.GetMetricBinValuesNumeric(metric, binCount);
            return numericBinValues;
        }

        [HttpGet("{id}/CategoricalMetricBinValues")]
        public async Task<IEnumerable<MetricDailyBinValueString>> CategoricalMetricBinValues(string id)
        {
            var metric = await base.GetResource(id);
            var categoricalBinValues = await customerMetricStore.GetMetricBinValuesString(metric);
            return categoricalBinValues;
        }


        [HttpGet("{id}/Destinations")]
        public async Task<IEnumerable<MetricDestinationBase>> GetDestinations(string id)
        {
            var metric = await base.GetResource(id);
            await store.LoadMany(metric, _ => _.Destinations);
            return metric.Destinations;
        }

        [HttpGet("{id}/ExportCustomers")]
        public async Task<IActionResult> GetExportCustomers(string id)
        {
            var metric = await base.GetResource(id);
            var exportCustomers = await workflows.GetExportCustomers(metric);
            return File(exportCustomers, "text/csv",
                $"{metric.Name}_customers.csv".ToLower());
        }

        [HttpPost("{id}/Destinations/")]
        public async Task<MetricDestinationBase> AddDestination(string id, CreateDestinationDto dto)
        {
            var metric = await base.GetResource(id);
            dto.Validate();
            var d = await workflows.AddDestination(metric, dto.IntegratedSystemId, dto.DestinationType,
                endpoint: dto.Endpoint,
                propertyName: dto.PropertyName);
            return d;
        }

        [HttpDelete("{id}/Destinations/{destinationId}")]
        public async Task<MetricDestinationBase> RemoveDestination(string id, long destinationId)
        {
            var metric = await base.GetResource(id);
            var d = await workflows.RemoveDestination(metric, destinationId);
            return d;
        }

        protected override Task<(bool, string)> CanDelete(Metric entity)
        {
            return Task.FromResult((true, ""));
        }
    }
}

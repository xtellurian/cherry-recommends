using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Security;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/TrackedUsers")]
    [Route("api/Customers")]
    public class CustomerMetricsController : SignalBoxControllerBase
    {
        private readonly MetricWorkflows workflows;
        private readonly IHistoricCustomerMetricStore customerMetricStore;
        private readonly ICustomerStore customerStore;

        public CustomerMetricsController(MetricWorkflows workflows,
                                             IHistoricCustomerMetricStore customerMetricStore,
                                             ICustomerStore customerStore)
        {
            this.workflows = workflows;
            this.customerMetricStore = customerMetricStore;
            this.customerStore = customerStore;
        }

        /// <summary>Returns a list of metrics available for a customer.</summary>
        [HttpGet("{id}/features")] // backwards compat
        [HttpGet("{id}/Metrics")]
        public async Task<IEnumerable<Metric>> AvailableMetrics(string id, bool? useInternalId = null)
        {
            var customer = await LoadCustomer(customerStore, id, useInternalId);
            return await customerMetricStore.GetMetricsFor(customer);
        }

        /// <summary>Creates a new set of metric values on a user. You probably shouldn't set this manually.</summary>
        [HttpPost("{id}/features/{metricCommonId}")]
        [HttpPost("{id}/Metrics/{metricCommonId}")]
        [Authorize(Policies.AdminOnlyPolicyName)]
        public async Task<HistoricCustomerMetric> Create(string id,
                                         string metricCommonId,
                                         [FromBody] CreateCustomerMetric dto,
                                         [FromQuery] bool? useInternalId = null,
                                         bool? forceIncrementVersion = null)
        {
            var customer = await LoadCustomer(customerStore, id, useInternalId);
            return await workflows.CreateMetricOnCustomer(customer, metricCommonId, dto.Value, forceIncrementVersion);
        }

        /// <summary>Returns the value set in the metric.</summary>
        [HttpGet("{id}/features/{metricCommonId}")]
        [HttpGet("{id}/Metrics/{metricCommonId}")]
        public async Task<HistoricCustomerMetric> CustomerMetric(string id,
                                         string metricCommonId,
                                         [FromQuery] bool? useInternalId = null,
                                         [FromQuery] int? version = null)
        {
            var customer = await LoadCustomer(customerStore, id, useInternalId);
            return await workflows.ReadCustomerMetric(customer, metricCommonId, version);
        }
    }
}

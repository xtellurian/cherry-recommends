using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Segments;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

#nullable enable
namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/Segments")]
    public class SegmentMetricEnrolmentRulesController : SignalBoxControllerBase
    {
        private readonly ILogger<SegmentMetricEnrolmentRulesController> logger;
        private readonly IMetricEnrolmentRuleStore enrolmentRuleStore;
        private readonly IMetricStore metricStore;
        private readonly ISegmentStore segmentStore;
        private readonly IQueueStores queueStores;
        private readonly ITenantProvider tenantProvider;
        private readonly SegmentEnrolmentWorkflow workflow;

        public SegmentMetricEnrolmentRulesController(ILogger<SegmentMetricEnrolmentRulesController> logger,
                                               IMetricEnrolmentRuleStore enrolmentRuleStore,
                                               IMetricStore metricStore,
                                               ISegmentStore segmentStore,
                                               IQueueStores queueStores,
                                               ITenantProvider tenantProvider,
                                               SegmentEnrolmentWorkflow workflow)
        {
            this.logger = logger;
            this.enrolmentRuleStore = enrolmentRuleStore;
            this.metricStore = metricStore;
            this.segmentStore = segmentStore;
            this.queueStores = queueStores;
            this.tenantProvider = tenantProvider;
            this.workflow = workflow;
        }

        /// <summary>Adds a metric based enrolment rule for the Segment.</summary>
        [HttpPost("{id}/MetricEnrolmentRules")]
        public async Task<MetricEnrolmentRule> CreateMetricEnrolmentRule(long id, [FromBody] CreateMetricEnrolmentRuleDto dto)
        {
            dto.Validate();
            var metric = await metricStore.Read(dto.MetricId ?? throw new BadRequestException("Metric ID cannot be null"));
            if (metric.Scope != Core.Metrics.MetricScopes.Customer)
            {
                throw new BadRequestException("Metric Scope must be Customer");
            }
            if (metric.ValueType == Core.Metrics.MetricValueType.Numeric && dto.NumericPredicate == null)
            {
                throw new BadRequestException($"Metric {metric.Id} is Numeric but NumericPredicate was null");
            }
            if (metric.ValueType == Core.Metrics.MetricValueType.Categorical && dto.CategoricalPredicate == null)
            {
                throw new BadRequestException($"Metric {metric.Id} is Categorical but CategoricalPredicate was null");
            }

            var segment = await segmentStore.Read(id);
            logger.LogInformation("Creating a metric enrolment rule for segment {segmentId} on metric {metricId}", segment.Id, metric.Id);
            var rule = await enrolmentRuleStore.Create(new MetricEnrolmentRule
            {
                Metric = metric,
                MetricId = metric.Id,
                Segment = segment,
                SegmentId = segment.Id,
                NumericPredicate = dto.NumericPredicate,
                CategoricalPredicate = dto.CategoricalPredicate,
            });
            await enrolmentRuleStore.Context.SaveChanges();
            return rule;
        }

        /// <summary>Gets all metric enrolment rules.</summary>
        [HttpGet("{id}/MetricEnrolmentRules")]
        public async Task<Paginated<MetricEnrolmentRule>> GetMetricEnrolmentRules(long id, [FromQuery] PaginateRequest p)
        {
            var segment = await segmentStore.Read(id);
            return await enrolmentRuleStore.Query(new EntityStoreQueryOptions<MetricEnrolmentRule>(p.Page, _ => _.SegmentId == segment.Id));
        }

        /// <summary>Deletes a Metric Enrolment Rule.</summary>
        [HttpDelete("{id}/MetricEnrolmentRules/{ruleId}")]
        public async Task<DeleteResponse> DeleteMetricEnrolmentRule(long id, long ruleId)
        {
            var segment = await segmentStore.Read(id);
            var rule = await enrolmentRuleStore.Read(ruleId);
            if (rule.SegmentId != segment.Id)
            {
                throw new BadRequestException($"Rule {ruleId} is not assigned to segment {segment.Id}");
            }

            var success = await enrolmentRuleStore.Remove(ruleId);
            await enrolmentRuleStore.Context.SaveChanges();
            return new DeleteResponse(ruleId, $"api/Segments/{id}/MetricEnrolmentRules/{ruleId}", success);
        }

        /// <summary>Runs the rule.</summary>
        [HttpPost("{id}/MetricEnrolmentRules/{ruleId}")]
        public async Task<EnrolmentReport> RunMetricEnrolmentRule(long id, long ruleId, bool preview = false)
        {
            var segment = await segmentStore.Read(id);
            var rule = await enrolmentRuleStore.Read(ruleId);
            if (rule.SegmentId != segment.Id)
            {
                throw new BadRequestException($"Rule {rule.Id} is not assigned to Segment {segment.Id}");
            }

            logger.LogInformation("Rule {ruleId} was manually run for segment {segmentId}", rule.Id, segment.Id);
            return await workflow.RunEnrolmentRule(rule, preview);
        }

        /// <summary>Get the rule.</summary>
        [HttpGet("{id}/MetricEnrolmentRules/{ruleId}")]
        public async Task<EnrolmentRule> GetMetricEnrolmentRule(long id, long ruleId)
        {
            var segment = await segmentStore.Read(id);
            var rule = await enrolmentRuleStore.Read(ruleId);
            if (rule.SegmentId != segment.Id)
            {
                throw new BadRequestException($"Rule {rule.Id} is not assigned to Segment {segment.Id}");
            }

            return rule;
        }

        /// <summary>Triggers the enrolmnet rule to run.</summary>
        [HttpPost("{id}/MetricEnrolmentRules/{ruleId}/Trigger")]
        public async Task EnqueueMetricEnrolmentRule(long id, long ruleId, bool preview = false)
        {
            var segment = await segmentStore.Read(id);
            var rule = await enrolmentRuleStore.Read(ruleId);
            if (rule.SegmentId != segment.Id)
            {
                throw new BadRequestException($"Rule {rule.Id} is not assigned to Segment {segment.Id}");
            }

            logger.LogInformation("Rule {ruleId} was manually triggered for segment {segmentId}", rule.Id, segment.Id);
            await queueStores.Enqueue(new RunSegmentEnrolmentRuleQueueMessage(tenantProvider.RequestedTenantName, rule.Id, segment.EnvironmentId));
        }
    }
}

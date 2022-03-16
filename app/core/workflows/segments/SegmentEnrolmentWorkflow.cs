using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LinqKit;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Segments;

namespace SignalBox.Core.Workflows
{
    public class SegmentEnrolmentWorkflow
    {
        private readonly IEnrolmentRuleStore enrolmentRuleStore;
        private readonly ICustomerSegmentWorkflow customerSegmentWorkflow;
        private readonly ICustomerStore customerStore;
        private readonly IHistoricCustomerMetricStore historicCustomerMetricStore;
        private readonly ILogger<SegmentEnrolmentWorkflow> logger;

        /// <summary>
        /// This is the root workflow for enrolling into segments.
        /// </summary>
        /// <param name="enrolmentRuleStore"></param>
        /// <param name="metricEnrolmentRuleWorkflow"></param>
        /// <param name="customerSegmentWorkflow"></param>
        /// <param name="logger"></param>
        public SegmentEnrolmentWorkflow(IEnrolmentRuleStore enrolmentRuleStore,
                                        ICustomerSegmentWorkflow customerSegmentWorkflow,
                                        ICustomerStore customerStore,
                                        IHistoricCustomerMetricStore historicCustomerMetricStore,
                                        ILogger<SegmentEnrolmentWorkflow> logger)
        {
            this.enrolmentRuleStore = enrolmentRuleStore;
            this.customerSegmentWorkflow = customerSegmentWorkflow;
            this.customerStore = customerStore;
            this.historicCustomerMetricStore = historicCustomerMetricStore;
            this.logger = logger;
        }
        public async Task<IEnumerable<EnrolmentReport>> RunAllEnrolmentRules()
        {
            var reports = new List<EnrolmentReport>();
            await foreach (var rule in enrolmentRuleStore.Iterate())
            {
                reports.Add(await RunEnrolmentRule(rule, false));
            }
            return reports;
        }

        public async Task<EnrolmentReport> RunEnrolmentRule(EnrolmentRule rule, bool preview)
        {
            if (rule is MetricEnrolmentRule metricEnrolmentRule)
            {
                return await RunMetricEnrolment(metricEnrolmentRule, preview);
            }
            else
            {
                logger.LogWarning("Unkown enrolment rule type, id=", rule.Id);
                return new EnrolmentReport();
            }
        }

        /// <summary>
        /// Long running task. Runs a rule against all customers, and adds them to a segment if required.
        /// </summary>
        /// <param name="rule">The rule to run.</param>
        /// <returns></returns>
        private async Task<EnrolmentReport> RunMetricEnrolment(MetricEnrolmentRule rule, bool preview)
        {
            if (rule.MetricId == null)
            {
                throw new WorkflowException($"MetricId is null for rule {rule.Id}");
            }
            var report = new EnrolmentReport(rule);
            if (rule.NumericPredicate != null)
            {
                Expression<Func<LatestMetric, bool>> selectPredicate =
                    (latestMetric) => rule.NumericPredicate.ToExpression().Invoke(latestMetric.NumericValue);

                await foreach (var latestValue in historicCustomerMetricStore.IterateLatest(rule.MetricId.Value, selectPredicate.Expand()))
                {
                    if (latestValue.CustomerId.HasValue)
                    {
                        // these are the latest metric values that meet the requirements.
                        await enrolmentRuleStore.Load(rule, _ => _.Segment);
                        var customer = await customerStore.Read(latestValue.CustomerId.Value);
                        if (!preview)
                        {
                            // actually add to segment if not preview
                            await customerSegmentWorkflow.AddToSegment(rule.Segment, customer);
                        }
                        report.CustomersEnrolled += 1;
                    }
                }
            }
            return report;
        }
    }
}
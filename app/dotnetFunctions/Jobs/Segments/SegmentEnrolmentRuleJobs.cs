using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Constants;
using SignalBox.Core.Segments;
using SignalBox.Core.Workflows;

namespace SignalBox.Functions
{
    public class SegmentEnrolmentRuleJobs
    {
        private const string JobName = "SegmentEnrolmentRules";
        private readonly SegmentEnrolmentWorkflow enrolmentWorkflow;
        private readonly IEnrolmentRuleStore enrolmentRuleStore;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IEnvironmentProvider environmentProvider;


        public SegmentEnrolmentRuleJobs(
                                    SegmentEnrolmentWorkflow enrolmentWorkflow,
                                    IEnrolmentRuleStore enrolmentRuleStore,
                                    IDateTimeProvider dateTimeProvider,
                                   IEnvironmentProvider environmentProvider)
        {
            this.enrolmentWorkflow = enrolmentWorkflow;
            this.enrolmentRuleStore = enrolmentRuleStore;
            this.dateTimeProvider = dateTimeProvider;
            this.environmentProvider = environmentProvider;

        }

        [Function($"Run__{JobName}_FanOutForTenant")]
        [QueueOutput(AzureQueueNames.RunSegmentEnrolmentRule)]
        public async Task<IEnumerable<RunSegmentEnrolmentRuleQueueMessage>> FanOutSegmentEnrolmentRules(
           [QueueTrigger(AzureQueueNames.RunAllSegmentEnrolmentRules)] RunAllSegmentEnrolmentRulesQueueMessage message, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(FanOutSegmentEnrolmentRules));

            logger.LogInformation("Fanning out for job {jobName}", JobName);
            var rules = new List<EnrolmentRule>();
            await foreach (var rule in enrolmentRuleStore.Iterate())
            {
                await enrolmentRuleStore.Load(rule, _ => _.Segment);
                rule.LastEnqueued = dateTimeProvider.Now;
                rules.Add(rule);
            }
            await enrolmentRuleStore.Context.SaveChanges();

            return rules.Select(_ => new RunSegmentEnrolmentRuleQueueMessage(message.TenantName, _.Id, _.Segment.EnvironmentId));
        }

        // Runs one enrolment rule
        [Function($"Run_{JobName}")]
        public async Task RunSegmentEnrolment(
            [QueueTrigger(AzureQueueNames.RunSegmentEnrolmentRule)] RunSegmentEnrolmentRuleQueueMessage message, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(RunSegmentEnrolment));

            logger.LogInformation($"Running job {JobName}");
            var rule = await enrolmentRuleStore.Read(message.RuleId);
            rule.LastEnqueued = dateTimeProvider.Now;
            await enrolmentRuleStore.Context.SaveChanges();

            await enrolmentWorkflow.RunEnrolmentRule(rule, preview: false);
        }
    }
}

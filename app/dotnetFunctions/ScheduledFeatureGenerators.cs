using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Workflows;

namespace dotnetFunctions
{
    public class ScheduledFeatureGenerators
    {
        private readonly FeatureGeneratorWorkflows workflows;

        public ScheduledFeatureGenerators(FeatureGeneratorWorkflows workflows)
        {
            this.workflows = workflows;
        }

        [Function("ScheduledFeatureGenerators")]
        // should run once per day at 1300 UTC (approx midnight Australia time)
        public async Task Run([TimerTrigger("0 0 13 * * * ")] MyInfo myTimer, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(ScheduledFeatureGenerators));

            logger.LogInformation("Starting to run all feature generators.");
            try
            {
                var totalWrites = await workflows.RunAllFeatureGenerators();
                logger.LogInformation($"Wrote {totalWrites}", new { completedAt = DateTimeOffset.UtcNow });
            }
            catch (System.Exception ex)
            {
                logger.LogError("Error when running all workflows", ex);
                logger.LogError(ex.Message);

            }
        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}

using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Metrics;

namespace SignalBox.Core.Workflows
{
#nullable enable
    public class JoinTwoMetricsWorkflow : IWorkflow
    {
        private readonly IGlobalMetricValueStore globalMetricValueStore;
        private readonly ILogger<JoinTwoMetricsWorkflow> logger;

        public JoinTwoMetricsWorkflow(IGlobalMetricValueStore globalMetricValueStore,
                                      ILogger<JoinTwoMetricsWorkflow> logger)
        {
            this.globalMetricValueStore = globalMetricValueStore;
            this.logger = logger;
        }

        public async Task RunJoinTwoMetricWorkflow(MetricGenerator generator)
        {
            if (generator.JoinTwoMetrics == null)
            {
                throw new WorkflowException($"Metric Generator {generator.Id} has null JoinTwoMetrics");
            }

            switch (generator.JoinTwoMetrics?.JoinType)
            {
                case JoinType.Divide:
                    await DivideMetrics(generator);
                    break;
                default:
                    throw new WorkflowException($"Unknown JoinTwoMetrics JoinType {generator.JoinTwoMetrics?.JoinType}");
            }
        }

        private async Task DivideMetrics(MetricGenerator generator)
        {
            if (generator.JoinTwoMetrics == null)
            {
                throw new WorkflowException("JoinTwoMetrics cannot be null here");
            }
            // get metric 1 value
            logger.LogInformation("Dividing two metrics");
            var metric1Value = await globalMetricValueStore.LatestMetricValue(generator.JoinTwoMetrics.Metric1);
            var metric2Value = await globalMetricValueStore.LatestMetricValue(generator.JoinTwoMetrics.Metric2);
            if (metric1Value == null || metric2Value == null)
            {
                logger.LogWarning($"An input global metric value was null. Metric 1 = {metric1Value?.Value}; Metric 2 = {metric2Value?.Value}");
                return;
            }

            var currentValue = await globalMetricValueStore.LatestMetricValue(generator.Metric);
            if (metric1Value.NumericValue.HasValue && metric2Value.NumericValue.HasValue && metric2Value.NumericValue != 0)
            {
                var result = metric1Value.NumericValue.Value / metric2Value.NumericValue.Value;
                logger.LogInformation("Metric Generator {generatorId} produced result value {result}", generator.Id, result);
                // save the value if it's new
                if (currentValue == null || currentValue.NumericValue != result)
                {
                    var version = (currentValue?.Version ?? 0) + 1;
                    await globalMetricValueStore.Create(new GlobalMetricValue(generator.Metric, version, result));
                    await globalMetricValueStore.Context.SaveChanges();
                }
            }
            else
            {
                logger.LogWarning("Generator {generatorId} could not be completed.", generator.Id);
            }
        }
    }
}
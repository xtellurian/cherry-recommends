using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Metrics;

namespace SignalBox.Core.Workflows
{
    public class AggregateCustomerMetricWorkflow : IWorkflow
    {
        private readonly IHistoricCustomerMetricStore historicCustomerMetricStore;
        private readonly IGlobalMetricValueStore globalMetricValueStore;
        private readonly ILogger<AggregateCustomerMetricWorkflow> logger;

        public AggregateCustomerMetricWorkflow(IHistoricCustomerMetricStore historicCustomerMetricStore,
                                               IGlobalMetricValueStore globalMetricValueStore,
                                               ILogger<AggregateCustomerMetricWorkflow> logger)
        {
            this.historicCustomerMetricStore = historicCustomerMetricStore;
            this.globalMetricValueStore = globalMetricValueStore;
            this.logger = logger;
        }

        public async Task RunAggregateCustomerMetricWorkflow(MetricGenerator generator)
        {
            logger.LogInformation("Starting RunAggregateCustomerMetricWorkflow");
            if (generator.AggregateCustomerMetric == null)
            {
                throw new WorkflowException($"Metric Generator {generator.Id} has null AggregateCustomerMetric");
            }

            var definition = generator.AggregateCustomerMetric;
            double? result;
            switch (generator.Metric.ValueType)
            {
                case MetricValueType.Numeric:
                default:
                    result = await CalculateForNumericInput(definition);
                    break;
                case MetricValueType.Categorical:
                    result = await CalculateForCategoricalInput(definition);
                    break;
            }

            if (result == null)
            {
                logger.LogWarning("Generator {generatorId} for metric {metricId} produced null. ", generator.Id, generator.MetricId);
                return;
            }

            var latest = await globalMetricValueStore.LatestMetricValue(generator.Metric);
            if (result.HasValue && (latest == null || latest.NumericValue != result))
            {
                logger.LogInformation("Updating metric {metricId}", generator.MetricId);
                var version = (latest?.Version ?? 0) + 1;
                // then this value should be written to the database
                await globalMetricValueStore.Create(new GlobalMetricValue(generator.Metric, version, result.Value));
                await globalMetricValueStore.Context.SaveChanges();
            }
            else
            {
                logger.LogInformation("Not updating aggregate metric {metricId}", generator.MetricId);
            }
        }

        private async Task<double?> CalculateForNumericInput(AggregateCustomerMetric definition)
        {
            double result;
            var values = await historicCustomerMetricStore.GetAggregateMetricValuesNumeric(definition.Metric, 0);
            var latestAggregation = values.OrderBy(_ => _.FirstOfWeek).LastOrDefault();
            if (latestAggregation == null)
            {
                logger.LogWarning("GetAggregateMetricValuesNumeric produced empty for Metric {metricId}", definition.MetricId);
                return null;
            }

            switch (definition.AggregationType)
            {
                case AggregationTypes.Sum:
                    result = latestAggregation.WeeklyDistinctCustomerCount * latestAggregation.WeeklyMeanNumericValue;
                    break;
                case AggregationTypes.Mean:
                default:
                    result = latestAggregation.WeeklyMeanNumericValue;
                    break;
            }

            return result;
        }
        private async Task<double?> CalculateForCategoricalInput(AggregateCustomerMetric definition)
        {
            double result;
            var values = await historicCustomerMetricStore.GetAggregateMetricValuesString(definition.Metric, 0);

            var latestAggregation = values.Where(_ => _.StringValue == definition.CategoricalValue).OrderBy(_ => _.FirstOfWeek).LastOrDefault();
            if (latestAggregation == null)
            {
                logger.LogWarning("GetAggregateMetricValuesString produced empty for Metric {metricId}", definition.MetricId);
                return null;
            }
            switch (definition.AggregationType)
            {
                case AggregationTypes.Sum:
                default:
                    result = latestAggregation.WeeklyDistinctCustomerCount;
                    break;
                case AggregationTypes.Mean:
                    throw new WorkflowException("Cannot take the mean of a categorical aggregation");
            }

            return result;
        }
    }
}
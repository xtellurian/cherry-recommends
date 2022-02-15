using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core.Metrics;

namespace SignalBox.Core.Workflows
{
    public class AggregateCustomerMetricWorkflow : IWorkflow
    {
        private readonly IHistoricCustomerMetricStore historicCustomerMetricStore;
        private readonly IGlobalMetricValueStore globalMetricValueStore;

        public AggregateCustomerMetricWorkflow(IHistoricCustomerMetricStore historicCustomerMetricStore,
                                               IGlobalMetricValueStore globalMetricValueStore)
        {
            this.historicCustomerMetricStore = historicCustomerMetricStore;
            this.globalMetricValueStore = globalMetricValueStore;
        }

        public async Task RunAggregateCustomerMetricWorkflow(MetricGenerator generator)
        {
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
                    result = await CalculateForNumericInput(generator, definition);
                    break;
                case MetricValueType.Categorical:
                    result = await CalculateForCategoricalInput(generator, definition);
                    break;
            }

            var latest = await globalMetricValueStore.LatestMetricValue(generator.Metric);
            if (result == null)
            {
                return;
            }

            if (result.HasValue && (latest == null || latest.NumericValue != result))
            {
                var version = (latest?.Version ?? 0) + 1;
                // then this value should be written to the database
                await globalMetricValueStore.Create(new GlobalMetricValue(generator.Metric, version, result.Value));
                await globalMetricValueStore.Context.SaveChanges();
            }
        }

        private async Task<double?> CalculateForNumericInput(MetricGenerator generator, AggregateCustomerMetric definition)
        {
            double result;
            var values = await historicCustomerMetricStore.GetAggregateMetricValuesNumeric(generator.Metric, 0);
            var latestAggregation = values.OrderBy(_ => _.FirstOfWeek).LastOrDefault();
            if (latestAggregation == null)
            {
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
        private async Task<double?> CalculateForCategoricalInput(MetricGenerator generator, AggregateCustomerMetric definition)
        {
            double result;
            var values = await historicCustomerMetricStore.GetAggregateMetricValuesString(generator.Metric, 0);

            var latestAggregation = values.Where(_ => _.StringValue == definition.CategoricalValue).OrderBy(_ => _.FirstOfWeek).LastOrDefault();
            if (latestAggregation == null)
            {
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Metrics;
using SignalBox.Core.Metrics.Generators;

namespace SignalBox.Core.Workflows
{
#nullable enable
    public partial class MetricGeneratorWorkflows : MetricWorkflowBase, IWorkflow
    {
        protected const int MaxSubsetSize = 25;
        protected async Task<MetricGeneratorRunSummary> RunFilterSelectAggregateGenerator(MetricGenerator generator)
        {
            logger.LogInformation($"Running FilterSelectAggregate generator: {generator.Id} for Metric {generator.MetricId}");
            await metricGeneratorStore.Load(generator, _ => _.Metric);
            var steps = new List<FilterSelectAggregateStep>(generator.FilterSelectAggregateSteps).OrderBy(_ => _.Order);
            logger.LogInformation($"There are {steps.Count()} steps in generator {generator.Id}");
            var summary = new MetricGeneratorRunSummary(0);

            await foreach (var customer in customerStore.Iterate())
            {
                var context = new FilterSelectAggregateContext(customer, generator.Metric, trackedUserEventStore);
                logger.LogInformation($"Running generator {generator.Id} for metric {generator.MetricId} and Customer {customer.Id}.");
                DateTimeOffset since = DateTimeOffset.MinValue;
                if (generator.TimeWindow == MetricGeneratorTimeWindow.SevenDays)
                {
                    since = DateTimeOffset.Now.AddDays(-7);
                }
                else if (generator.TimeWindow == MetricGeneratorTimeWindow.ThirtyDays)
                {
                    since = DateTimeOffset.Now.AddDays(-30);
                }

                try
                {
                    var value = await GenerateMetricValueForCustomer(steps, context, since);
                    if (value != null)
                    {
                        summary.TotalWrites += 1;
                        await metricStore.Context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                    throw new WorkflowException($"Error running Filter Select Aggregate generator {generator.Id}", ex);
                }
            }

            return summary;
        }

        private async Task<HistoricCustomerMetric?> GenerateMetricValueForCustomer(IOrderedEnumerable<FilterSelectAggregateStep> steps, FilterSelectAggregateContext context, DateTimeOffset? since = null)
        {
            // step 1 (filter if exists)
            // todo: this is breaking 
            // There is already an open DataReader associated with this Connection which must be closed first.
            await context.LoadEventsIntoContext(steps.FirstOrDefault(_ => _.Filter != null)?.Filter, since);

            // step 2 (select the property that we care about)
            var select = steps.FirstOrDefault(_ => _.Select != null)?.Select;
            var values = context.Events.Select(_ =>
            {
                if (select?.PropertyNameMatch == null)
                {
                    // then we're going to return 1, because we're counting events.
                    return 1;
                }
                else if (_.Properties.TryGetValue(select.PropertyNameMatch, out var v))
                {
                    return v;
                }
                else
                {
                    return null;
                }
            }).Where(_ => _ != null);

            // get the numeric values as doubles

            // step 3 - handle aggregation;
            var numericValues = values.Select(ToDoubleSafe).Where(_ => _ != null).Select(_ => (double)_!);
            if (numericValues.Any())
            {
                var agg = steps.FirstOrDefault(_ => _.Aggregate != null)?.Aggregate;
                double finalValue;
                switch (agg?.AggregationType)
                {
                    case AggregationTypes.Mean:
                        finalValue = numericValues.Average();
                        break;
                    case AggregationTypes.Sum:
                    default:
                        finalValue = numericValues.Sum();
                        break;
                }

                return await base.CreateMetricOnUser(context.Customer, context.Metric.CommonId, finalValue, false);
            }
            else
            {
                return null;
            }

        }

        private double? ToDoubleSafe(object? o)
        {
            if (o == null)
            {
                return null;
            }
            else if (o is double)
            {
                return (double)o;
            }
            else if (double.TryParse(o.ToString(), out var d))
            {
                return d;
            }
            else
            {
                return null;
            }
        }

    }


}
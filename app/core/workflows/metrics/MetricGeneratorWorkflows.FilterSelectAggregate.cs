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

            await foreach (var customer in customerStore.Iterate(new EntityStoreIterateOptions<Customer> { ChangeTracking = ChangeTrackingOptions.NoTrackingWithIdentityResolution }))
            {
                var context = new FilterSelectAggregateContext(customer, generator.Metric, trackedUserEventStore)
                {
                    Steps = steps.ToList(),
                };
                logger.LogInformation($"Running generator {generator.Id} for metric {generator.MetricId} and Customer {customer.Id}.");
                DateTimeOffset since = DateTimeOffset.Now.DateTimeSince(generator.TimeWindow ?? MetricGeneratorTimeWindow.AllTime);

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
            var finalValue = await aggregateWorkflow.RunFilterSelectAggregateWorkflow(context, since);
            if (finalValue != null)
            {
                return await base.CreateMetricOnCustomer(context.Customer, context.Metric.CommonId, finalValue, false);
            }

            return null;
        }
    }


}
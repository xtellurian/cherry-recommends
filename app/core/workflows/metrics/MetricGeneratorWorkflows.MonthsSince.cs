using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Metrics.Generators;

namespace SignalBox.Core.Workflows
{
    public partial class MetricGeneratorWorkflows : MetricWorkflowBase, IWorkflow
    {
        protected async Task<MetricGeneratorRunSummary> RunMonthsSinceEarliestEventGenerator(MetricGenerator generator)
        {
            var now = dateTimeProvider.Now;
            var totalWrites = 0;
            await foreach (var customer in customerStore.Iterate())
            {
                try
                {
                    var minTimestamp = await trackedUserEventStore.Min(_ => _.TrackedUserId == customer.Id, _ => _.Timestamp);
                    var delta = CalcDeltaMonths(minTimestamp, now);
                    await CreateMetricOnUser(customer, generator.Metric.CommonId, delta, false);
                    totalWrites++;
                }
                catch (InvalidStorageAccessException)
                {
                    logger.LogInformation($"Skipping tracked user {customer.Id} - has no min timestamp");
                }
                catch (System.Exception ex)
                {
                    logger.LogError($"Something went wrong during feature gen", ex);
                    logger.LogError(ex.GetType().ToString());
                    logger.LogError(ex.Message);
                    throw new WorkflowException("Error creating feature", ex);
                }
            }

            logger.LogInformation($"Finished feauture generator workflow with {totalWrites} total writes");

            return new MetricGeneratorRunSummary(totalWrites);
        }
        private int CalcDeltaMonths(DateTimeOffset startDate, DateTimeOffset endDate) => ((endDate.Year - startDate.Year) * 12) + endDate.Month - startDate.Month;

    }


}
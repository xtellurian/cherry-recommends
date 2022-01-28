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
            var page = 1;
            var hasNextPage = true;
            var now = dateTimeProvider.Now;
            var totalWrites = 0;
            while (hasNextPage)
            {
                var query = await customerStore.Query(page++);
                hasNextPage = query.Pagination.HasNextPage;
                logger.LogInformation($"Page {query.Pagination.PageNumber} of {query.Pagination.PageCount} tracked user pages");
                foreach (var tu in query.Items)
                {
                    try
                    {
                        var minTimestamp = await trackedUserEventStore.Min(_ => _.TrackedUserId == tu.Id, _ => _.Timestamp);
                        var delta = CalcDeltaMonths(minTimestamp, now);
                        await CreateMetricOnUser(tu, generator.Metric.CommonId, delta, false);
                        totalWrites++;
                    }
                    catch (InvalidStorageAccessException)
                    {
                        logger.LogInformation($"Skipping tracked user {tu.Id} - has no min timestamp");
                    }
                    catch (System.Exception ex)
                    {
                        logger.LogError($"Something went wrong during feature gen. Page Number = {query.Pagination.PageNumber}", ex);
                        logger.LogError(ex.GetType().ToString());
                        logger.LogError(ex.Message);
                        throw new WorkflowException("Error creating feature", ex);
                    }
                }
            }

            logger.LogInformation($"Finished feauture generator workflow with {totalWrites} total writes");

            return new MetricGeneratorRunSummary(totalWrites);
        }
        private int CalcDeltaMonths(DateTimeOffset startDate, DateTimeOffset endDate) => ((endDate.Year - startDate.Year) * 12) + endDate.Month - startDate.Month;

    }


}
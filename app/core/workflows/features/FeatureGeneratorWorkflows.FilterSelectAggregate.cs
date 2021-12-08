using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Features;
using SignalBox.Core.Features.Generators;

namespace SignalBox.Core.Workflows
{
#nullable enable
    public partial class FeatureGeneratorWorkflows : FeatureWorkflowBase, IWorkflow
    {
        protected const int MaxSubsetSize = 25;
        protected async Task<FeatureGeneratorRunSummary> RunFilterSelectAggregateGenerator(FeatureGenerator generator)
        {
            logger.LogInformation($"Running FilterSelectAggregate generator: {generator.Id} for Feature {generator.FeatureId}");
            await featureGeneratorStore.Load(generator, _ => _.Feature);
            var steps = new List<FilterSelectAggregateStep>(generator.FilterSelectAggregateSteps).OrderBy(_ => _.Order);
            logger.LogInformation($"There are {steps.Count()} steps in generator {generator.Id}");
            var summary = new FeatureGeneratorRunSummary(0);

            await foreach (var user in trackedUserStore.Iterate())
            {
                var context = new FilterSelectAggregateContext(user, generator.Feature, trackedUserEventStore);
                logger.LogInformation($"Running generator {generator.Id} for feature {generator.FeatureId} and Tracked User {user.Id}.");
                try
                {
                    var value = await GenerateFeatureValueForTrackedUser(steps, context);
                    if (value != null)
                    {
                        summary.TotalWrites += 1;
                        await featureStore.Context.SaveChanges();
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

        private async Task<HistoricTrackedUserFeature?> GenerateFeatureValueForTrackedUser(IOrderedEnumerable<FilterSelectAggregateStep> steps, FilterSelectAggregateContext context)
        {
            // step 1 (filter if exists)
            // todo: this is breaking 
            // There is already an open DataReader associated with this Connection which must be closed first.
            await context.LoadEventsIntoContext(steps.FirstOrDefault(_ => _.Filter != null)?.Filter);

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

                return await base.CreateFeatureOnUser(context.TrackedUser, context.Feature.CommonId, finalValue, false);
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
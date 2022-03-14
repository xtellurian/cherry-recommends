using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Metrics;
using SignalBox.Core.Metrics.Generators;

namespace SignalBox.Core.Workflows
{
#nullable enable
    public class FilterSelectAggregateWorkflow
    {
        private readonly ILogger<FilterSelectAggregateWorkflow> logger;

        public FilterSelectAggregateWorkflow(ILogger<FilterSelectAggregateWorkflow> logger)
        {
            this.logger = logger;
        }

        public async Task<double?> RunFilterSelectAggregateWorkflow(FilterSelectAggregateContext context, DateTimeOffset? since = null)
        {
            // step 1 (filter)
            switch (context.Metric.Scope)
            {
                case MetricScopes.Customer:
                    await context.LoadEventsIntoContext(context.Steps.FirstOrDefault(_ => _.Filter != null)?.Filter, since);
                    break;
                case MetricScopes.Business:
                    await context.LoadBusinessEventsIntoContext(context.Steps.FirstOrDefault(_ => _.Filter != null)?.Filter, since);
                    break;
                default:
                    throw new BadRequestException($"{context.Metric.Scope} is an unhandled metric scope");
            }

            double finalValue;
            // step 2 (select the property that we care about)
            var select = context.Steps.FirstOrDefault(_ => _.Select != null)?.Select;
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
                var agg = context.Steps.FirstOrDefault(_ => _.Aggregate != null)?.Aggregate;
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

                return finalValue;
            }
            else
            {
                return null;
            }
        }

        private static double? ToDoubleSafe(object? o)
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
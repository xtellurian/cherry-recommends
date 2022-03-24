using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommendations.Destinations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core.Workflows
{
    public abstract class RecommenderInvokationWorkflowBase<T> where T : RecommenderEntityBase
    {
        private readonly IRecommenderStore<T> store;
        private readonly IHistoricCustomerMetricStore customerMetricStore;
        private readonly IWebhookSenderClient webhookSender;
        private readonly IDateTimeProvider dateTimeProvider;

        public RecommenderInvokationWorkflowBase(
                                                 IRecommenderStore<T> store,
                                                 IHistoricCustomerMetricStore customerMetricStore,
                                                 IWebhookSenderClient webhookSender,
                                                 IDateTimeProvider dateTimeProvider)
        {
            this.store = store;
            this.customerMetricStore = customerMetricStore;
            this.webhookSender = webhookSender;
            this.dateTimeProvider = dateTimeProvider;
        }

#nullable enable
        public async Task<Paginated<InvokationLogEntry>> QueryInvokationLogs(IPaginate paginate, RecommenderEntityBase recommender)
        {
            return await store.QueryInvokationLogs(paginate, recommender.Id);
        }

        protected async Task SendToDestinations(T recommender, RecommendingContext context, RecommendationEntity recommendation)
        {
            await store.LoadMany(recommender, _ => _.RecommendationDestinations);
            context.LogMessage($"Discovered {recommender.RecommendationDestinations.Count} destinations");
            foreach (var d in recommender.RecommendationDestinations)
            {
                if (d is WebhookDestination webhookDestination)
                {
                    await webhookSender.Send(webhookDestination, recommendation);
                    context.LogMessage($"Send to Webhook endpoint: {webhookDestination.Endpoint}");
                }
                else if (d is SegmentSourceFunctionDestination segDestination)
                {
                    await webhookSender.Send(segDestination, recommendation);
                    context.LogMessage($"Send to Segment endpoint: {segDestination.Endpoint}");
                }
                else
                {
                    context.LogMessage($"Warning: Cannot handle destination Type {d.DestinationType}");
                }
            }
        }

        protected async Task<InvokationLogEntry> StartTrackInvokation(T recommender, IModelInput input, bool? saveOnComplete = true)
        {
            var entry = new InvokationLogEntry(recommender, dateTimeProvider.Now);
            var errorsWillBe = recommender.ShouldThrowOnBadInput() ? "thrown" : "silently handled.";
            entry.LogMessage($"Recomending for tracked user: {input.CommonUserId}. Errors will be {errorsWillBe} ");
            entry.LogMessage($"There are {input.Arguments?.Count ?? 0} input arguments.");
            recommender.RecommenderInvokationLogs ??= new List<InvokationLogEntry>();
            recommender.RecommenderInvokationLogs.Add(entry);
            if (saveOnComplete == true)
            {
                await store.Context.SaveChanges();
            }
            return entry;
        }

        protected async Task<RecommendingContext> EndTrackInvokation(RecommendingContext context,
                                                                    bool success,
                                                                    string? message = null,
                                                                    string? modelResponse = null,
                                                                    bool? saveOnComplete = true)
        {
            if (context == null)
            {
                throw new System.NullReferenceException("RecommendingContext must not be null");
            }
            context.InvokationLog.InvokeEnded = dateTimeProvider.Now;
            context.InvokationLog.Success = success;
            context.InvokationLog.LogMessage(message);
            context.InvokationLog.Correlator = context.Correlator;
            context.InvokationLog.Customer = context.Customer;
            context.InvokationLog.Business = context.Business;
            context.InvokationLog.ModelResponse = modelResponse;
            context.InvokationLog.Status = "Complete";
            if (saveOnComplete == true)
            {
                await store.Context.SaveChanges();
            }
            return context;
        }

        protected async Task<IDictionary<string, object>> GetMetrics(T recommender, RecommendingContext context)
        {
            await store.LoadMany(recommender, _ => _.LearningFeatures);
            var metricValues = new List<HistoricCustomerMetric>();
            context.LogMessage($"Recommender has {recommender.LearningFeatures.Count} Learning Metrics.");
            foreach (var metric in recommender.LearningFeatures)
            {
                if (context.Customer != null)
                {
                    if (await customerMetricStore.MetricExists(context.Customer, metric))
                    {
                        metricValues.Add(await customerMetricStore.ReadCustomerMetric(context.Customer, metric));
                    }
                }
                else if (context.Business != null)
                {
                    // TODO: insert business metrics here
                }
            }

            var result = metricValues
                .Where(_ => _.Value != null)
                .ToDictionary(_ => _.Metric.CommonId, _ => _.Value ?? "");

            context.LogMessage($"Discovered {result.Count} Metric values.");
            return result;
        }

        /// <summary>
        /// It meant to throw in some situations.
        /// </summary>
        protected void CheckArgument(RecommenderEntityBase recommender,
                                    RecommenderArgument arg,
                                    IModelInput input,
                                    RecommendingContext context)
        {
            input.Arguments ??= new Dictionary<string, object>(); // ensure no null refs here
            if (!input.Arguments.ContainsKey(arg.CommonId))
            {
                // argument is missing
                if (arg.IsRequired && recommender.ShouldThrowOnBadInput())
                {
                    throw new BadRequestException("Missing recommender argument",
                        $"The argument {arg.CommonId} is required, and the recommender is set to throw on errors.");
                }
                else
                {
                    context.InvokationLog.LogMessage($"Using default value ({arg.DefaultArgumentValue}) for argument {arg.CommonId}");
                    input.Arguments[arg.CommonId] = arg.DefaultArgumentValue;
                }
            }
            else
            {
                // incoming argument exists. check the type.
                var val = input.Arguments[arg.CommonId]?.ToString();
                if (val == null && arg.IsRequired && recommender.ShouldThrowOnBadInput())
                {
                    throw new BadRequestException("Null recommender argument",
                        $"The argument {arg.CommonId} is null, and the recommender is set to throw on errors.");
                }
                else if (arg.ArgumentType == ArgumentTypes.Numerical)
                {
                    // try and parse as a number
                    if (!double.TryParse(val, out _))
                    {
                        // the value was bad.
                        if (recommender.ShouldThrowOnBadInput())
                        {
                            context.InvokationLog.LogMessage($"The argument {arg.CommonId} should be numeric, and the recommender is set to throw on errors.");
                            throw new BadRequestException("Bad recommender argument",
                                $"The argument {arg.CommonId} should be numeric, and the recommender is set to throw on errors.");
                        }
                        else
                        {
                            // try and set the value to the default
                            context.InvokationLog.LogMessage($"Using default value ({arg.DefaultArgumentValue}) for argument {arg.CommonId}");
                            input.Arguments[arg.CommonId] = arg.DefaultArgumentValue;
                        }
                    }
                }
                else
                {
                    context.InvokationLog.LogMessage($"Categorical arguments are not validated");
                }
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core.Workflows
{
    public abstract class RecommenderInvokationWorkflowBase<T> where T : RecommenderEntityBase
    {
        private readonly IStorageContext storageContext;
        private readonly IRecommenderStore<T> store;
        private readonly IHistoricTrackedUserFeatureStore trackedUserFeatureStore;
        private readonly IDateTimeProvider dateTimeProvider;

        public RecommenderInvokationWorkflowBase(IStorageContext storageContext,
                                                 IRecommenderStore<T> store,
                                                 IHistoricTrackedUserFeatureStore trackedUserFeatureStore,
                                                 IDateTimeProvider dateTimeProvider)
        {
            this.storageContext = storageContext;
            this.store = store;
            this.trackedUserFeatureStore = trackedUserFeatureStore;
            this.dateTimeProvider = dateTimeProvider;
        }

#nullable enable
        public async Task<Paginated<InvokationLogEntry>> QueryInvokationLogs(RecommenderEntityBase recommender, int page)
        {
            return await store.QueryInvokationLogs(recommender.Id, page);
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
                await storageContext.SaveChanges();
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
            context.InvokationLog.TrackedUser = context.TrackedUser;
            context.InvokationLog.ModelResponse = modelResponse;
            context.InvokationLog.Status = "Complete";
            if (saveOnComplete == true)
            {
                await storageContext.SaveChanges();
            }
            return context;
        }

        protected async Task<IDictionary<string, object>> GetFeatures(RecommendingContext context)
        {
            var features = await trackedUserFeatureStore.ReadAllLatestFeatures(context.TrackedUser);
            var result = features
                .Where(_ => _.Value != null)
                .ToDictionary(_ => _.Feature.CommonId, _ => _.Value ?? "");

            context.InvokationLog.LogMessage($"Discovered {result.Count} features");
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
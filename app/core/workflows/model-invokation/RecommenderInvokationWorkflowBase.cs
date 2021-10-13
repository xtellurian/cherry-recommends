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
            if(context == null)
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
    }
}
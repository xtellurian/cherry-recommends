using System.Collections.Generic;
using System.Threading.Tasks;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core.Workflows
{
    public abstract class RecommenderInvokationWorkflowBase<T> where T : RecommenderEntityBase
    {
        private readonly IStorageContext storageContext;
        private readonly IRecommenderStore<T> store;
        private readonly IDateTimeProvider dateTimeProvider;

        public RecommenderInvokationWorkflowBase(IStorageContext storageContext,
                                                 IRecommenderStore<T> store,
                                                 IDateTimeProvider dateTimeProvider)
        {
            this.storageContext = storageContext;
            this.store = store;
            this.dateTimeProvider = dateTimeProvider;
        }

#nullable enable
        public async Task<Paginated<InvokationLogEntry>> QueryInvokationLogs(RecommenderEntityBase recommender, int page)
        {
            return await store.QueryInvokationLogs(recommender.Id, page);
        }

        public async Task<InvokationLogEntry> StartTrackInvokation(T recommender, string userId, bool? saveOnComplete = true)
        {
            var entry = new InvokationLogEntry(recommender, dateTimeProvider.Now);
            entry.LogMessage($"Recomending for tracked user: {userId}");
            recommender.RecommenderInvokationLogs ??= new List<InvokationLogEntry>();
            recommender.RecommenderInvokationLogs.Add(entry);
            if (saveOnComplete == true)
            {
                await storageContext.SaveChanges();
            }
            return entry;
        }

        public async Task<InvokationLogEntry> EndTrackInvokation(InvokationLogEntry entry,
                                                                 bool success,
                                                                 TrackedUser trackedUser,
                                                                 RecommendationCorrelator? correlator,
                                                                 string? message = null,
                                                                 string? modelResponse = null,
                                                                 bool? saveOnComplete = true)
        {
            entry.InvokeEnded = dateTimeProvider.Now;
            entry.Success = success;
            entry.LogMessage(message);
            entry.Correlator = correlator;
            entry.TrackedUser = trackedUser;
            entry.ModelResponse = modelResponse;
            entry.Status = "Complete";
            if (saveOnComplete == true)
            {
                await storageContext.SaveChanges();
            }
            return entry;
        }
    }
}
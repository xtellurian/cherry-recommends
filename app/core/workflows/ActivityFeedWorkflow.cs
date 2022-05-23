using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core.Workflows
{
    public class ActivityFeedWorkflow : IWorkflow, IActivityFeedWorkflow
    {
        private readonly ILogger<ActivityFeedWorkflow> logger;
        private readonly ICustomerEventStore eventStore;
        private readonly IItemsRecommendationStore promoRecommendationStore;
        private readonly IParameterSetRecommendationStore parameterSetRecommendationStore;

        public ActivityFeedWorkflow(ILogger<ActivityFeedWorkflow> logger,
                                    ICustomerEventStore eventStore,
                                    IItemsRecommendationStore promoRecommendationStore,
                                    IParameterSetRecommendationStore parameterSetRecommendationStore)
        {
            this.logger = logger;
            this.eventStore = eventStore;
            this.promoRecommendationStore = promoRecommendationStore;
            this.parameterSetRecommendationStore = parameterSetRecommendationStore;
        }

        public async Task<IEnumerable<ActivityFeedEntity>> GetActivityFeedEntities(IPaginate p)
        {
            var activityFeed = new List<ActivityFeedEntity>();

            // events
            var events = await eventStore.Latest(p);
            var eventsFeed = new ActivityFeedEntity()
            {
                ActivityKind = ActivityKinds.Event,
                ActivityItems = new Paginated<object>(events.Items, 1, events.Items.Count(), 1)
            };
            activityFeed.Add(eventsFeed);

            // recommendations
            var parameterSetRecommendations = await parameterSetRecommendationStore.Query(
                new EntityStoreQueryOptions<ParameterSetRecommendation>(p.Page));
            var itemsRecommendations = await promoRecommendationStore.Query(
                new EntityStoreQueryOptions<ItemsRecommendation>(p.Page));
            var recommendations = new List<RecommendationEntity>();

            recommendations.AddRange(parameterSetRecommendations?.Items);
            recommendations.AddRange(itemsRecommendations?.Items);

            var toSerializeProperly = new List<object>(recommendations.OrderByDescending(_ => _.Created));
            var paginatedRecommendations = new Paginated<object>(toSerializeProperly, 1, toSerializeProperly.Count, 1);

            // add to activity feed
            var recommendationsFeed = new ActivityFeedEntity()
            {
                ActivityKind = ActivityKinds.Recommendation,
                ActivityItems = paginatedRecommendations
            };

            activityFeed.Add(recommendationsFeed);

            return activityFeed;
        }
    }
}
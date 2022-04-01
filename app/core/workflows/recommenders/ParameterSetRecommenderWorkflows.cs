using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;

#nullable enable
namespace SignalBox.Core.Workflows
{
    public class ParameterSetRecommenderWorkflows : RecommenderWorkflowBase<ParameterSetRecommender>
    {
        private readonly IStorageContext storageContext;
        private readonly IParameterSetRecommendationStore recommendationStore;
        private readonly IModelRegistrationStore modelRegistrationStore;
        private readonly IParameterStore parameterStore;

        public ParameterSetRecommenderWorkflows(
            IStorageContext storageContext,
            IParameterSetRecommenderStore store,
            IParameterSetRecommendationStore recommendationStore,
            IMetricStore metricStore,
            ISegmentStore segmentStore,
            IIntegratedSystemStore systemStore,
            IModelRegistrationStore modelRegistrationStore,
            IRecommenderReportImageWorkflow reportImageWorkflows,
            IParameterStore parameterStore,
            IChannelStore channelStore) : base(store, systemStore, metricStore, segmentStore, channelStore, reportImageWorkflows)
        {
            this.storageContext = storageContext;
            this.recommendationStore = recommendationStore;
            this.modelRegistrationStore = modelRegistrationStore;
            this.parameterStore = parameterStore;
        }

        public async Task<ParameterSetRecommender> CloneParameterSetRecommender(CreateCommonEntityModel common, ParameterSetRecommender from)
        {
            await store.LoadMany(from, _ => _.Parameters);
            return await this.CreateParameterSetRecommender(common, from.Parameters.Select(_ => _.CommonId), from.ParameterBounds, from.Arguments, from.Settings ?? new RecommenderSettings());
        }

        public async Task<RecommenderStatistics> CalculateStatistics(ParameterSetRecommender recommender)
        {
            var stats = new RecommenderStatistics();
            stats.NumberCustomersRecommended = await recommendationStore.CountUniqueCustomers(recommender.Id);
            stats.NumberInvokations = await recommendationStore.CountRecommendations(recommender.Id);
            return stats;
        }
        public async Task<ParameterSetRecommender> CreateParameterSetRecommender(CreateCommonEntityModel common,
                                                                                 IEnumerable<string> parameterCommonIds,
                                                                                 IEnumerable<ParameterBounds> bounds,
                                                                                 IEnumerable<RecommenderArgument>? arguments,
                                                                                 RecommenderSettings settings)
        {
            var parameters = new List<Parameter>();
            foreach (var p in parameterCommonIds)
            {
                parameters.Add(await parameterStore.ReadFromCommonId(p));
            }

            // validate the args
            if (arguments.Any(_ => string.IsNullOrEmpty(_.CommonId)))
            {
                throw new BadRequestException("Arguments require an identifier");
            }

            // validate the bounds
            if (bounds.Any(_ => string.IsNullOrEmpty(_.CommonId)))
            {
                throw new BadRequestException("Bounds require an reference identifier");
            }

            var recommender = await store.Create(new ParameterSetRecommender(common.CommonId, common.Name, parameters, bounds, arguments, settings));
            await storageContext.SaveChanges();
            return recommender;
        }

        public async Task<Paginated<ParameterSetRecommendation>> QueryRecommendations(long recommenderId, IPaginate paginate)
        {
            return await recommendationStore.QueryForRecommender(paginate, recommenderId);
        }

        public async Task<ModelRegistration> LinkRegisteredModel(ParameterSetRecommender recommender, long modelId)
        {
            var model = await modelRegistrationStore.Read(modelId);
            if (model.ModelType == ModelTypes.ParameterSetRecommenderV1)
            {
                recommender.ModelRegistration = model;
                await storageContext.SaveChanges();
                return model;
            }
            else
            {
                throw new BadRequestException($"Model of type {model.ModelType} can't be linked to an Parameter Recommender");
            }
        }
    }
}
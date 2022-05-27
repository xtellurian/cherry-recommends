using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Campaigns;

#nullable enable
namespace SignalBox.Core.Workflows
{
    public class ParameterSetCampaignWorkflows : CampaignWorkflowBase<ParameterSetCampaign>
    {
        private readonly IStorageContext storageContext;
        private readonly IParameterSetRecommendationStore recommendationStore;
        private readonly IModelRegistrationStore modelRegistrationStore;
        private readonly IParameterStore parameterStore;

        public ParameterSetCampaignWorkflows(
            IStorageContext storageContext,
            IParameterSetCampaignStore store,
            IParameterSetRecommendationStore recommendationStore,
            IModelRegistrationStore modelRegistrationStore,
            IRecommenderReportImageWorkflow reportImageWorkflows,
            IParameterStore parameterStore,
            IStoreCollection storeCollection) : base(store, storeCollection, reportImageWorkflows)
        {
            this.storageContext = storageContext;
            this.recommendationStore = recommendationStore;
            this.modelRegistrationStore = modelRegistrationStore;
            this.parameterStore = parameterStore;
        }

        public async Task<ParameterSetCampaign> CloneParameterSetCampaign(CreateCommonEntityModel common, ParameterSetCampaign from)
        {
            await store.LoadMany(from, _ => _.Parameters);
            return await this.CreateParameterSetCampaign(common, from.Parameters.Select(_ => _.CommonId), from.ParameterBounds, from.Arguments, from.Settings ?? new CampaignSettings());
        }

        public async Task<CampaignStatistics> CalculateStatistics(ParameterSetCampaign recommender)
        {
            var stats = new CampaignStatistics();
            stats.NumberCustomersRecommended = await recommendationStore.CountUniqueCustomers(recommender.Id);
            stats.NumberInvokations = await recommendationStore.CountRecommendations(recommender.Id);
            return stats;
        }
        public async Task<ParameterSetCampaign> CreateParameterSetCampaign(CreateCommonEntityModel common,
                                                                                 IEnumerable<string> parameterCommonIds,
                                                                                 IEnumerable<ParameterBounds> bounds,
                                                                                 IEnumerable<CampaignArgument>? arguments,
                                                                                 CampaignSettings settings)
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

            var recommender = await store.Create(new ParameterSetCampaign(common.CommonId, common.Name, parameters, bounds, arguments, settings));
            await storageContext.SaveChanges();
            return recommender;
        }

        public async Task<Paginated<ParameterSetRecommendation>> QueryRecommendations(long recommenderId, IPaginate paginate)
        {
            return await recommendationStore.QueryForRecommender(paginate, recommenderId);
        }

        public async Task<ModelRegistration> LinkRegisteredModel(ParameterSetCampaign recommender, long modelId)
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
                throw new BadRequestException($"Model of type {model.ModelType} can't be linked to an Parameter Campaign");
            }
        }
    }
}
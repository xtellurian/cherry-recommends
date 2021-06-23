using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core.Recommenders;

#nullable enable
namespace SignalBox.Core.Workflows
{
    public class ParameterSetRecommenderWorkflows
    {
        private readonly IStorageContext storageContext;
        private readonly IParameterSetRecommenderStore store;
        private readonly IParameterSetRecommendationStore recommendationStore;
        private readonly IParameterStore parameterStore;
        private readonly IRecommender<ParameterSetRecommendation> recommender;

        public ParameterSetRecommenderWorkflows(IStorageContext storageContext,
                                                IParameterSetRecommenderStore store,
                                               IParameterSetRecommendationStore recommendationStore,
                                                IParameterStore parameterStore,
                                                IRecommender<ParameterSetRecommendation> recommender)
        {
            this.storageContext = storageContext;
            this.store = store;
            this.recommendationStore = recommendationStore;
            this.parameterStore = parameterStore;
            this.recommender = recommender;
        }

        public async Task<ParameterSetRecommender> CreateParameterSetRecommender(CreateCommonEntityModel common,
                                                                                 IEnumerable<string> parameterCommonIds,
                                                                                 IEnumerable<ParameterBounds> bounds,
                                                                                 IEnumerable<RecommenderArgument> arguments)
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

            var recommender = await store.Create(new ParameterSetRecommender(common.CommonId, common.Name, parameters, bounds, arguments));
            await storageContext.SaveChanges();
            return recommender;
        }

        public async Task<ParameterSetRecommendation> GetParameterSetRecommendation(string commonId, Dictionary<string, object> args)
        {
            var recommendation = await recommender.Recommend(new RecommendationRequestArguments(args));
            // save the recommmendation before we return it.
            recommendation = await recommendationStore.Create(recommendation);
            await storageContext.SaveChanges();
            return recommendation;
        }
    }
}
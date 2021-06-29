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
        private readonly IModelRegistrationStore modelRegistrationStore;
        private readonly IParameterStore parameterStore;

        public ParameterSetRecommenderWorkflows(IStorageContext storageContext,
                                                IParameterSetRecommenderStore store,
                                                IModelRegistrationStore modelRegistrationStore,
                                                IParameterStore parameterStore)
        {
            this.storageContext = storageContext;
            this.store = store;
            this.modelRegistrationStore = modelRegistrationStore;
            this.parameterStore = parameterStore;
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

        public async Task<ModelRegistration> LinkRegisteredModel(ParameterSetRecommender recommender, long modelId)
        {
            var model = await modelRegistrationStore.Read(modelId);
            recommender.ModelRegistration = model;
            await storageContext.SaveChanges();
            return model;
        }
    }
}
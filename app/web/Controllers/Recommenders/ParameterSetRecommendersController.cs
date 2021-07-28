
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/recommenders/[controller]")]
    public class ParameterSetRecommendersController : RecommenderControllerBase<ParameterSetRecommender>
    {
        private readonly ILogger<ParameterSetRecommendersController> logger;
        private readonly ParameterSetRecommenderInvokationWorkflows invokationWorkflows;
        private readonly ParameterSetRecommenderWorkflows workflows;

        public ParameterSetRecommendersController(ILogger<ParameterSetRecommendersController> logger,
                                                 IParameterSetRecommenderStore store,
                                                 ParameterSetRecommenderInvokationWorkflows invokationWorkflows,
                                                 ParameterSetRecommenderWorkflows workflows) : base(store, invokationWorkflows)
        {
            this.logger = logger;
            this.invokationWorkflows = invokationWorkflows;
            this.workflows = workflows;
        }

        [HttpPost]
        public async Task<ParameterSetRecommender> Create(CreateParameterSetRecommender dto)
        {
            var c = new CreateCommonEntityModel(dto.CommonId, dto.Name);
            var arguments = dto.Arguments.Select(a => new RecommenderArgument
            {
                ArgumentType = a.ArgumentType,
                CommonId = a.CommonId,
                DefaultValue = new DefaultArgumentValue(a.ArgumentType, a.DefaultValue)
            });
            return await workflows.CreateParameterSetRecommender(c, dto.Parameters, dto.Bounds, arguments,
                 new RecommenderErrorHandling { ThrowOnBadInput = dto.ThrowOnBadInput });
        }
        
        [HttpPost("{id}/ModelRegistration")]
        public async Task<ModelRegistration> LinkModel(string id, LinkModel dto)
        {
            var parameterSetRecommender = await base.GetResource(id);
            return await workflows.LinkRegisteredModel(parameterSetRecommender, dto.ModelId);
        }

        [HttpGet("{id}/ModelRegistration")]
        public async Task<ModelRegistration> GetLinkModel(string id)
        {
            var parameterSetRecommender = await base.GetResource(id);
            return parameterSetRecommender.ModelRegistration ??
                throw new EntityNotFoundException(typeof(ModelRegistration), id, null);

        }

        /// <summary>Invoke a model with some payload. Id is the recommender Id.</summary>
        [HttpPost("{id}/invoke")]
        public async Task<ParameterSetRecommenderModelOutputV1> InvokeModel(string id, string version, [FromBody] ParameterSetRecommenderModelInputV1 input, bool? useInternalId = null)
        {
            var recommender = await base.GetResource(id, useInternalId);
            return await invokationWorkflows.InvokeParameterSetRecommender(recommender, version, input);
        }

        /// <summary>Get the latest recommendations made by a recommender.</summary>
        [HttpGet("{id}/recommendations")]
        public async Task<Paginated<ParameterSetRecommendation>> GetRecommendations(long id, [FromQuery] PaginateRequest p)
        {
            return await workflows.QueryRecommendations(id, p.Page);
        }

        protected override Task<(bool, string)> CanDelete(ParameterSetRecommender entity)
        {
            return Task.FromResult((true, ""));
        }
    }
}
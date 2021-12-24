
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;
using SignalBox.Core.Workflows;
using SignalBox.Infrastructure.Dto;
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
                                                 ParameterSetRecommenderWorkflows workflows) : base(store, workflows, invokationWorkflows)
        {
            this.logger = logger;
            this.invokationWorkflows = invokationWorkflows;
            this.workflows = workflows;
        }

        [HttpPost]
        public async Task<ParameterSetRecommender> Create(CreateParameterSetRecommender dto)
        {
            var c = new CreateCommonEntityModel(dto.CommonId, dto.Name);
            if (dto.CloneFromId.HasValue)
            {
                // then clone from existing.
                var from = await store.Read(dto.CloneFromId.Value);
                return await workflows.CloneParameterSetRecommender(c, from);
            }

            return await workflows.CreateParameterSetRecommender(c,
                dto.Parameters,
                dto.Bounds,
                dto.Arguments.ToCoreRepresentation(),
                dto.Settings.ToCoreRepresentation());
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
                throw new EntityNotFoundException(typeof(ModelRegistration), id);

        }

        /// <summary>Get summary statistics about the recommender.</summary>
        [HttpGet("{id}/Statistics")]
        public async Task<RecommenderStatistics> GetStatistics(string id, bool? useInternalId = null)
        {
            var recommender = await base.GetEntity(id, useInternalId);
            return await workflows.CalculateStatistics(recommender);
        }

        /// <summary>Invoke a model with some payload. Id is the recommender Id.</summary>
        [HttpPost("{id}/invoke")]
        [AllowApiKey]
        [EnableCors(CorsPolicies.WebApiKeyPolicy)]
        public async Task<ParameterSetRecommendationDto> InvokeModel(string id,
                                                                    [FromBody] ModelInputDto input,
                                                                    bool? useInternalId = null)
        {
            ValidateInvokationDto(input);
            var recommender = await base.GetResource(id, useInternalId);
            var convertedInput = new ParameterSetRecommenderModelInputV1
            {
                Arguments = input.Arguments,
                CustomerId = input.GetCustomerId() ?? System.Guid.NewGuid().ToString()
            };
            var recommendation = await invokationWorkflows.InvokeParameterSetRecommender(recommender, convertedInput);
            return new ParameterSetRecommendationDto(recommendation, recommendation.GetOutput<ParameterSetRecommenderModelOutputV1>().RecommendedParameters);
        }

        /// <summary>Get the latest recommendations made by a recommender.</summary>
        [HttpGet("{id}/recommendations")]
        public async Task<Paginated<ParameterSetRecommendation>> GetRecommendations(long id, [FromQuery] PaginateRequest p)
        {
            return await workflows.QueryRecommendations(id, p.Page, p.PageSize);
        }

        protected override Task<(bool, string)> CanDelete(ParameterSetRecommender entity)
        {
            return Task.FromResult((true, ""));
        }
    }
}
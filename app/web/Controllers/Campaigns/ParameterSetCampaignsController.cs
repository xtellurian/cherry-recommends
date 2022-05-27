
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Campaigns;
using SignalBox.Core.Workflows;
using SignalBox.Infrastructure.Dto;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/recommenders/ParameterSetRecommenders")]
    [Route("api/campaigns/[controller]")]
    public class ParameterSetCampaignsController : CampaignsControllerBase<ParameterSetCampaign>
    {
        private readonly ILogger<ParameterSetCampaignsController> logger;
        private readonly ParameterSetCampaignInvokationWorkflows invokationWorkflows;
        private readonly ParameterSetCampaignWorkflows workflows;

        public ParameterSetCampaignsController(ILogger<ParameterSetCampaignsController> logger,
                                                 IParameterSetCampaignStore store,
                                                 IAudienceStore audienceStore,
                                                 ParameterSetCampaignInvokationWorkflows invokationWorkflows,
                                                 ParameterSetCampaignWorkflows workflows) : base(store, audienceStore, workflows, invokationWorkflows)
        {
            this.logger = logger;
            this.invokationWorkflows = invokationWorkflows;
            this.workflows = workflows;
        }

        [HttpPost]
        public async Task<ParameterSetCampaign> Create(CreateParameterSetCampaign dto)
        {
            var c = new CreateCommonEntityModel(dto.CommonId, dto.Name);
            if (dto.CloneFromId.HasValue)
            {
                // then clone from existing.
                var from = await store.Read(dto.CloneFromId.Value);
                return await workflows.CloneParameterSetCampaign(c, from);
            }

            return await workflows.CreateParameterSetCampaign(c,
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
        public async Task<CampaignStatistics> GetStatistics(string id, bool? useInternalId = null)
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
            var recommendation = await invokationWorkflows.InvokeParameterSetCampaign(recommender, convertedInput);
            return new ParameterSetRecommendationDto(recommendation, recommendation.GetOutput<ParameterSetRecommenderModelOutputV1>().RecommendedParameters);
        }

        /// <summary>Get the latest recommendations made by a recommender.</summary>
        [HttpGet("{id}/recommendations")]
        public async Task<Paginated<ParameterSetRecommendation>> GetRecommendations(long id, [FromQuery] PaginateRequest p)
        {
            return await workflows.QueryRecommendations(id, p);
        }

        protected override Task<(bool, string)> CanDelete(ParameterSetCampaign entity)
        {
            return Task.FromResult((true, ""));
        }
    }
}
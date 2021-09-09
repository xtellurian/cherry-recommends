
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;
using SignalBox.Web.Dto.RecommenderInputs;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/recommenders/[controller]")]
    public class ItemsRecommendersController : RecommenderControllerBase<ItemsRecommender>
    {
        private readonly ILogger<ItemsRecommendersController> logger;
        private readonly ItemsRecommenderInvokationWorkflows invokationWorkflows;
        private readonly ItemsRecommenderWorkflows workflows;

        public ItemsRecommendersController(ILogger<ItemsRecommendersController> logger,
                                                 IItemsRecommenderStore store,
                                                 ItemsRecommenderInvokationWorkflows invokationWorkflows,
                                                 ItemsRecommenderWorkflows workflows) : base(store, invokationWorkflows)
        {
            this.logger = logger;
            this.invokationWorkflows = invokationWorkflows;
            this.workflows = workflows;
        }

        /// <summary>Returns the resource with this Id.</summary>
        [HttpGet("{id}")]
        public override async Task<ItemsRecommender> GetResource(string id, bool? useInternalId = null)
        {
            var recommender = await base.GetEntity(id, useInternalId);
            await store.Load(recommender, _ => _.DefaultItem);
            await store.LoadMany(recommender, _ => _.Items);
            return recommender;
        }

        /// <summary>Creates a new items recommender.</summary>
        [HttpPost]
        public async Task<ItemsRecommender> Create(CreateItemsRecommender dto, bool? useInternalId = null)
        {
            var c = new CreateCommonEntityModel(dto.CommonId, dto.Name);
            if (dto.CloneFromId.HasValue)
            {
                // then clone from existing.
                var from = await store.Read(dto.CloneFromId.Value);
                return await workflows.CloneItemsRecommender(c, from);
            }
            return await workflows.CreateItemsRecommender(c,
                dto.DefaultItemId, dto.ItemIds,dto.NumberOfItemsToRecommend, dto.Arguments.ToCoreRepresentation(),
                new RecommenderErrorHandling { ThrowOnBadInput = dto.ThrowOnBadInput });
        }

        /// <summary>Sets the default item id.</summary>
        [HttpPost("{id}/DefaultItem")]
        public async Task<RecommendableItem> SetDefaultItem(string id, [FromBody] DefaultItemDto dto, bool? useInternalId = null)
        {
            var recommender = await GetEntity(id, useInternalId);
            return await workflows.SetDefaultItem(recommender, dto.ItemId);
        }

        /// <summary>Sets the default item id.</summary>
        [HttpGet("{id}/DefaultItem")]
        public async Task<RecommendableItem> GetDefaultItem(string id, bool? useInternalId = null)
        {
            var recommender = await GetEntity(id, useInternalId);
            await store.Load(recommender, _ => _.DefaultItem);
            return recommender.DefaultItem ?? throw new BadRequestException("Recommender has no default item");
        }

        /// <summary>Set the backing model information.</summary>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("{id}/ModelRegistration")]
        public async Task<ModelRegistration> LinkModel(string id, LinkModel dto)
        {
            var recommender = await base.GetResource(id);
            return await workflows.LinkRegisteredModel(recommender, dto.ModelId);
        }

        /// <summary>Get the backing model information.</summary>
        [HttpGet("{id}/ModelRegistration")]
        public async Task<ModelRegistration> GetLinkModel(string id)
        {
            var parameterSetRecommender = await base.GetResource(id);
            return parameterSetRecommender.ModelRegistration ??
                throw new EntityNotFoundException(typeof(ModelRegistration), id);

        }

        /// <summary>Invoke a model with some input. Id is the recommender Id.</summary>
        [HttpPost("{id}/invoke")]
        public async Task<ItemsRecommenderModelOutputV1> InvokeModel(
            string id,
            [FromBody] ModelInputDto input,
            bool? useInternalId = null)
        {
            var recommender = await base.GetResource(id, useInternalId);
            var convertedInput = new ModelInputDto(input.CommonUserId, input.Arguments);

            var recommendation = await invokationWorkflows.InvokeItemsRecommender(recommender, convertedInput);
            return recommendation.GetOutput<ItemsRecommenderModelOutputV1>();
        }

        /// <summary>Get the latest recommendations made by a recommender.</summary>
        [HttpGet("{id}/recommendations")]
        public async Task<Paginated<ItemsRecommendation>> GetRecommendations(long id, [FromQuery] PaginateRequest p)
        {
            return await workflows.QueryRecommendations(id, p.Page);
        }

        protected override Task<(bool, string)> CanDelete(ItemsRecommender entity)
        {
            return Task.FromResult((true, ""));
        }
    }
}
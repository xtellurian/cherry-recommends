
using System.Linq;
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
    [Route("api/recommenders/ItemsRecommenders")]
    [Route("api/recommenders/[controller]")]
    public class PromotionsRecommendersController : RecommenderControllerBase<ItemsRecommender>
    {
        private readonly ILogger<PromotionsRecommendersController> logger;
        private readonly ItemsRecommenderInvokationWorkflows invokationWorkflows;
        private readonly ItemsRecommenderPerformanceWorkflows performanceWorkflows;
        private readonly ItemsRecommenderWorkflows workflows;

        public PromotionsRecommendersController(ILogger<PromotionsRecommendersController> logger,
                                                 IItemsRecommenderStore store,
                                                 ItemsRecommenderInvokationWorkflows invokationWorkflows,
                                                 ItemsRecommenderPerformanceWorkflows performanceWorkflows, ItemsRecommenderWorkflows workflows) : base(store, workflows, invokationWorkflows)
        {
            this.logger = logger;
            this.invokationWorkflows = invokationWorkflows;
            this.performanceWorkflows = performanceWorkflows;
            this.workflows = workflows;
        }

        /// <summary>Returns the resource with this Id.</summary>
        [HttpGet("{id}")]
        public override async Task<ItemsRecommender> GetResource(string id, bool? useInternalId = null)
        {
            var recommender = await base.GetEntity(id, useInternalId);
            await store.Load(recommender, _ => _.BaselineItem);
            await store.Load(recommender, _ => _.TargetMetric);
            await store.LoadMany(recommender, _ => _.Items);
            return recommender;
        }

        /// <summary>Creates a new promotions recommender.</summary>
        [HttpPost]
        public async Task<ItemsRecommender> Create(CreatePromotionsRecommender dto, bool? useInternalId = null)
        {
            var c = new CreateCommonEntityModel(dto.CommonId, dto.Name);
            if (dto.CloneFromId.HasValue)
            {
                // then clone from existing.
                var from = await store.Read(dto.CloneFromId.Value);
                return await workflows.CloneItemsRecommender(c, from);
            }
            return await workflows.CreateItemsRecommender(c,
                dto.GetBaselinePromotionId(), dto.ItemIds, dto.NumberOfItemsToRecommend,
                dto.Arguments.ToCoreRepresentation(),
                dto.Settings.ToCoreRepresentation(),
                dto.UseAutoAi ?? false,
                dto.TargetMetricId,
                useInternalId: useInternalId);
        }

        /// <summary>Sets the baseline promotion for the recommender.</summary>
        [HttpPost("{id}/DefaultItem")]
        [HttpPost("{id}/BaselineItem")]
        [HttpPost("{id}/BaselinePromotion")]
        public async Task<RecommendableItem> SetBaselineItem(string id, [FromBody] BaselinePromotionDto dto, bool? useInternalId = null)
        {
            var recommender = await GetEntity(id, useInternalId);
            return await workflows.SetBaselineItem(recommender, dto.GetPromotionId());
        }

        /// <summary>Gets the baseline promotion for the recommender.</summary>
        [HttpGet("{id}/DefaultItem")]
        [HttpGet("{id}/BaselineItem")]
        [HttpGet("{id}/BaselinePromotion")]
        public async Task<RecommendableItem> GetBaselineItem(string id, bool? useInternalId = null)
        {
            var recommender = await GetEntity(id, useInternalId);
            await store.Load(recommender, _ => _.BaselineItem);
            return recommender.BaselineItem ?? throw new BadRequestException("Recommender has no baseline promotion");
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

        /// <summary>Get summary statistics about the recommender.</summary>
        [HttpGet("{id}/Statistics")]
        public async Task<RecommenderStatistics> GetStatistics(string id, bool? useInternalId = null)
        {
            var recommender = await base.GetEntity(id, useInternalId);
            return await workflows.CalculateStatistics(recommender);
        }

        /// <summary>Invoke a model with some input. Id is the recommender Id.</summary>
        [HttpPost("{id}/invoke")]
        [AllowApiKey]
        [EnableCors(CorsPolicies.WebApiKeyPolicy)]
        public async Task<PromotionsRecommendationDto> InvokeModel(
            string id,
            [FromBody] ModelInputDto input,
            bool? useInternalId = null)
        {
            ValidateInvokationDto(input);
            var recommender = await base.GetResource(id, useInternalId);
            var convertedInput = new ItemsModelInputDto(input.GetCustomerId(), input.Arguments);
            if (convertedInput.Items != null && convertedInput.Items.Any())
            {
                throw new BadRequestException($"Promotions must not be set externally");
            }

            var recommendation = await invokationWorkflows.InvokeItemsRecommender(recommender, convertedInput);
            return new PromotionsRecommendationDto(recommendation);
        }

        /// <summary>Get the latest recommendations made by a recommender.</summary>
        [HttpGet("{id}/Recommendations")]
        public async Task<Paginated<ItemsRecommendation>> GetRecommendations(string id, [FromQuery] PaginateRequest p, bool? useInternalId = null)
        {
            return await workflows.QueryRecommendations(id, p.Page, p.PageSize, useInternalId);
        }

        /// <summary>Get the promotions associated with a recommender.</summary>
        [HttpGet("{id}/Items")]
        [HttpGet("{id}/Promotions")]
        public async Task<Paginated<RecommendableItem>> GetItems(string id, [FromQuery] PaginateRequest p, bool? useInternalId = null)
        {
            return await workflows.QueryItems(id, p.Page, useInternalId);
        }

        /// <summary>Get the promotions associated with a recommender.</summary>
        [HttpPost("{id}/Items")]
        [HttpPost("{id}/Promotions")]
        public async Task<RecommendableItem> AddItem(string id, [FromBody] AddPromotionDto dto, bool? useInternalId = null)
        {
            var recommender = await GetEntity(id, useInternalId);
            if (dto.Id.HasValue)
            {
                return await workflows.AddItem(recommender, dto.Id.Value, useInternalId);
            }
            else if (string.IsNullOrEmpty(dto.CommonId))
            {
                throw new BadRequestException("Either Id or CommonId is required");
            }
            else
            {
                return await workflows.AddItem(recommender, dto.CommonId, useInternalId);
            }
        }

        /// <summary>Get the performance report of a recommender.</summary>
        [HttpGet("{id}/Performance/{reportId}")]
        public async Task<ItemsRecommenderPerformanceReport> GetPerformanceReport(string id, string reportId, bool? useInternalId = null)
        {
            var recommender = await GetEntity(id, useInternalId);
            if (reportId == "latest")
            {
                return await performanceWorkflows.GetOrCalculateLatestPerfomance(id, useInternalId);
            }
            else if (long.TryParse(reportId, out var reportInternalId))
            {
                var report = await performanceWorkflows.PerformanceReportStore.Read(reportInternalId);
                if (report.RecommenderId != recommender.Id)
                {
                    throw new EntityNotFoundException(typeof(ItemsRecommenderPerformanceReport), reportId, "Report is not for selected recommender");
                }
                else
                {
                    return report;
                }
            }
            else
            {
                return await performanceWorkflows.GetOrCalculateLatestPerfomance(id, useInternalId);
            }
        }

        /// <summary>Remove a promotion association with a recommender.</summary>
        [HttpDelete("{id}/Items/{itemId}")]
        [HttpDelete("{id}/Promotions/{itemId}")]
        public async Task<RecommendableItem> RemoveItem(string id, string itemId, bool? useInternalId = null)
        {
            var recommender = await GetEntity(id, useInternalId);
            return await workflows.RemoveItem(recommender, itemId, useInternalId);
        }

        protected override Task<(bool, string)> CanDelete(ItemsRecommender entity)
        {
            return Task.FromResult((true, ""));
        }
    }
}
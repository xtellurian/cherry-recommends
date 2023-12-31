
using System;
using System.Linq;
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
using System.Collections.Generic;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/recommenders/ItemsRecommenders")]
    [Route("api/recommenders/PromotionsRecommenders")]
    [Route("api/campaigns/[controller]")]
    public class PromotionsCampaignsController : CampaignsControllerBase<PromotionsCampaign>
    {
        private readonly ILogger<PromotionsCampaignsController> logger;
        private readonly PromotionsCampaignInvokationWorkflows invokationWorkflows;
        private readonly PromotionsCampaignPerformanceWorkflows performanceWorkflows;
        private readonly PromotionsCampaignWorkflows workflows;
        private readonly IOfferWorkflow offerWorkflow;
        private readonly IItemsRecommendationStore recommendationStore;

        public PromotionsCampaignsController(ILogger<PromotionsCampaignsController> logger,
                                                 IPromotionsCampaignStore store,
                                                 IAudienceStore audienceStore,
                                                 PromotionsCampaignInvokationWorkflows invokationWorkflows,
                                                 PromotionsCampaignPerformanceWorkflows performanceWorkflows,
                                                 PromotionsCampaignWorkflows workflows,
                                                 IOfferWorkflow offerWorkflow,
                                                 IItemsRecommendationStore recommendationStore)
                                                 : base(store, audienceStore, workflows, invokationWorkflows)

        {
            this.logger = logger;
            this.invokationWorkflows = invokationWorkflows;
            this.performanceWorkflows = performanceWorkflows;
            this.workflows = workflows;
            this.offerWorkflow = offerWorkflow;
            this.recommendationStore = recommendationStore;
        }

        /// <summary>Returns the resource with this Id.</summary>
        [HttpGet("{id}")]
        public override async Task<PromotionsCampaign> GetResource(string id, bool? useInternalId = null)
        {
            var campaign = await base.GetEntity(id, useInternalId);
            await store.Load(campaign, _ => _.BaselineItem);
            await store.Load(campaign, _ => _.TargetMetric);
            await store.Load(campaign, _ => _.Optimiser);
            await store.LoadMany(campaign, _ => _.Items);
            return campaign;
        }

        /// <summary>Creates a new promotions campaign.</summary>
        [HttpPost]
        public async Task<PromotionsCampaign> Create(CreatePromotionsCampaign dto, bool? useInternalId = null)
        {
            var c = new CreateCommonEntityModel(dto.CommonId, dto.Name);
            PromotionsCampaign r;
            if (dto.CloneFromId.HasValue)
            {
                // then clone from existing.
                var from = await store.Read(dto.CloneFromId.Value);
                r = await workflows.ClonePromotionsCampaign(c, from);
            }
            else
            {

                r = await workflows.CreatePromotionsCampaign(c,
                   dto.GetBaselinePromotionId(), dto.ItemIds, dto.SegmentIds, dto.ChannelIds, dto.NumberOfItemsToRecommend,
                   dto.Arguments.ToCoreRepresentation(),
                   dto.Settings.ToCoreRepresentation(),
                   dto.UseAutoAi ?? false,
                   dto.TargetMetricId,
                   dto.TargetType ?? PromotionCampaignTargetTypes.Customer,
                   useInternalId: useInternalId);
            }
            return r;
        }

        /// <summary>Sets the baseline promotion for the campaign.</summary>
        [HttpPost("{id}/DefaultItem")]
        [HttpPost("{id}/BaselineItem")]
        [HttpPost("{id}/BaselinePromotion")]
        public async Task<RecommendableItem> SetBaselineItem(string id, [FromBody] BaselinePromotionDto dto, bool? useInternalId = null)
        {
            var campaign = await GetEntity(id, useInternalId);
            return await workflows.SetBaselineItem(campaign, dto.GetPromotionId());
        }

        /// <summary>Gets the baseline promotion for the campaign.</summary>
        [HttpGet("{id}/DefaultItem")]
        [HttpGet("{id}/BaselineItem")]
        [HttpGet("{id}/BaselinePromotion")]
        public async Task<RecommendableItem> GetBaselineItem(string id, bool? useInternalId = null)
        {
            var campaign = await GetEntity(id, useInternalId);
            await store.Load(campaign, _ => _.BaselineItem);
            return campaign.BaselineItem ?? throw new BadRequestException("Campaign has no baseline promotion");
        }

        /// <summary>Set the backing model information.</summary>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("{id}/ModelRegistration")]
        public async Task<ModelRegistration> LinkModel(string id, LinkModel dto)
        {
            var campaign = await base.GetResource(id);
            return await workflows.LinkRegisteredModel(campaign, dto.ModelId);
        }

        /// <summary>Get the backing model information.</summary>
        [HttpGet("{id}/ModelRegistration")]
        public async Task<ModelRegistration> GetLinkModel(string id)
        {
            var parameterSetRecommender = await base.GetResource(id);
            return parameterSetRecommender.ModelRegistration ??
                throw new EntityNotFoundException(typeof(ModelRegistration), id);

        }

        /// <summary>Get summary statistics about the campaign.</summary>
        [HttpGet("{id}/Statistics")]
        public async Task<CampaignStatistics> GetStatistics(string id, bool? useInternalId = null)
        {
            var campaign = await base.GetEntity(id, useInternalId);
            return await workflows.CalculateStatistics(campaign);
        }

        /// <summary>Invoke a model with some input. Id is the campaign Id.</summary>
        [HttpPost("{id}/invoke")]
        [AllowApiKey]
        [EnableCors(CorsPolicies.WebApiKeyPolicy)]
        public async Task<PromotionsRecommendationDto> InvokeModel(
            string id,
            [FromBody] ModelInputDto input,
            bool? useInternalId = null)
        {
            ValidateInvokationDto(input);
            var campaign = await base.GetResource(id, useInternalId);
            var convertedInput = new ItemsModelInputDto(input.Arguments)
            {
                CustomerId = input.GetCustomerId(),
                BusinessId = input.BusinessId
            };

            if (convertedInput.Items != null && convertedInput.Items.Any())
            {
                throw new BadRequestException($"Promotions must not be set externally");
            }

            var recommendation = await invokationWorkflows.InvokePromotionsCampaign(campaign, convertedInput);
            return new PromotionsRecommendationDto(recommendation);
        }

        /// <summary>Get the latest recommendations made by a campaign.</summary>
        [HttpGet("{id}/Recommendations")]
        public async Task<Paginated<ItemsRecommendation>> GetRecommendations(string id, [FromQuery] PaginateRequest p, bool? useInternalId = null)
        {
            return await workflows.QueryRecommendations(id, p, useInternalId);
        }

        /// <summary>Get a recommendation made by a campaign.</summary>
        [HttpGet("Recommendations/{recommendationId}")]
        public async Task<ItemsRecommendation> GetRecommendation(long recommendationId, bool? useInternalId = null)
        {
            var recommendation = await recommendationStore.Read(recommendationId);
            await recommendationStore.LoadMany(recommendation, _ => _.Items);
            await recommendationStore.Load(recommendation, _ => _.Customer);
            await recommendationStore.Load(recommendation, _ => _.Business);
            await recommendationStore.Load(recommendation, _ => _.Offer);

            return recommendation;
        }

        /// <summary>Get the offers made by a campaign.</summary>
        [HttpGet("{id}/Offers")]
        public async Task<Paginated<Offer>> GetOffers(string id, [FromQuery] PaginateRequest p, bool? useInternalId = null)
        {
            var campaign = await base.GetResource(id, useInternalId);
            string qsOfferState = HttpContext.Request.Query["offerState"].ToString();
            OfferState? state = null;

            if (Enum.TryParse<OfferState>(qsOfferState, true, out OfferState parsedState))
            {
                state = parsedState;
            };

            return await offerWorkflow.QueryOffers(campaign, p, state);
        }

        /// <summary>Get the Average Revenue per Offer report.</summary>
        [HttpGet("{id}/ARPOReport")]
        public async Task<ARPOReportDto> GetARPOReport(string id, bool? useInternalId = null)
        {
            var campaign = await base.GetResource(id, useInternalId);
            var data = await offerWorkflow.QueryARPOReportData(campaign, DateTimePeriod.Weekly, 11); // 12 weeks ago minus 1 since 0 is current week
            var response = new ARPOReportDto
            {
                CampaignId = campaign.Id,
                Type = DateTimePeriod.Weekly,
                Data = data
            };

            return response;
        }

        /// <summary>Get the Average Purchase Value report.</summary>
        [HttpGet("{id}/APVReport")]
        public async Task<APVReportDto> GetAPVReport(string id, bool? useInternalId = null)
        {
            var campaign = await base.GetResource(id, useInternalId);
            var data = await offerWorkflow.QueryAPVReportData(campaign, DateTimePeriod.Weekly, 11); // 12 weeks ago minus 1 since 0 is current week
            var response = new APVReportDto
            {
                CampaignId = campaign.Id,
                Type = DateTimePeriod.Weekly,
                Data = data
            };

            return response;
        }
        /// <summary>Get the Average Revenue per Offer report.</summary>
        [HttpGet("{id}/PerformanceReport")]
        public async Task<PerformanceReportDto> GetPerformanceReport(string id, bool? useInternalId = null)
        {
            var campaign = await base.GetResource(id, useInternalId);
            var data = await offerWorkflow.QueryPerformanceReportData(campaign, DateTimePeriod.Weekly, 11); // 12 weeks ago minus 1 since 0 is current week
            var response = new PerformanceReportDto
            {
                CampaignId = campaign.Id,
                Type = DateTimePeriod.Weekly,
                Data = data
            };

            return response;
        }

        /// <summary>Get the Offer Conversion Rate report.</summary>
        [HttpGet("{id}/ConversionRateReport")]
        public async Task<OfferConversionRateReportDto> GetConversionRateReport(string id, bool? useInternalId = null)
        {
            var campaign = await base.GetResource(id, useInternalId);
            var data = await offerWorkflow.QueryConversionRateReportData(campaign, DateTimePeriod.Weekly, 11); // 12 weeks ago minus 1 since 0 is current week
            var response = new OfferConversionRateReportDto
            {
                CampaignId = campaign.Id,
                Type = DateTimePeriod.Weekly,
                Data = data
            };

            return response;
        }

        /// <summary>Get the Offer Sensitivity Curve report.</summary>
        [HttpGet("{id}/SensitivityCurveReport")]
        public async Task<OfferSensitivityCurveReportDto> GetSensitivityCurveReport([FromServices] IDateTimeProvider dateTimeProvider, string id, bool? useInternalId = null)
        {
            var campaign = await base.GetResource(id, useInternalId);
            DateTimeOffset dataSinceDate = dateTimeProvider.Now.ToUniversalTime().DateTimeSince(DateTimePeriod.Weekly, 11).FirstDayOfWeek(DayOfWeek.Monday); // 12 weeks ago minus 1 since 0 is current week
            var data = await offerWorkflow.QuerySensitivityCurveReportData(campaign, dataSinceDate);
            var response = new OfferSensitivityCurveReportDto
            {
                CampaignId = campaign.Id,
                DataSinceDate = dataSinceDate,
                Data = data
            };

            return response;
        }

        /// <summary>Get the promotions associated with a recommender.</summary>
        [HttpGet("{id}/Items")]
        [HttpGet("{id}/Promotions")]
        public async Task<Paginated<RecommendableItem>> GetItems(string id, [FromQuery] PaginateRequest p, bool? useInternalId = null)
        {
            return await workflows.QueryItems(p, id, useInternalId);
        }

        /// <summary>Get the promotions associated with a campaign.</summary>
        [HttpPost("{id}/Items")]
        [HttpPost("{id}/Promotions")]
        public async Task<RecommendableItem> AddItem(string id, [FromBody] AddPromotionDto dto, bool? useInternalId = null)
        {
            var campaign = await GetEntity(id, useInternalId);
            if (dto.Id.HasValue)
            {
                return await workflows.AddItem(campaign, dto.Id.Value, useInternalId);
            }
            else if (string.IsNullOrEmpty(dto.CommonId))
            {
                throw new BadRequestException("Either Id or CommonId is required");
            }
            else
            {
                return await workflows.AddItem(campaign, dto.CommonId, useInternalId);
            }
        }

        /// <summary>Get the performance report of a campaign.</summary>
        [HttpGet("{id}/Performance/{reportId}")]
        public async Task<ItemsRecommenderPerformanceReport> GetPerformanceReport(string id, string reportId, bool? useInternalId = null)
        {
            var campaign = await GetEntity(id, useInternalId);
            if (reportId == "latest")
            {
                return await performanceWorkflows.GetOrCalculateLatestPerfomance(id, useInternalId);
            }
            else if (long.TryParse(reportId, out var reportInternalId))
            {
                var report = await performanceWorkflows.PerformanceReportStore.Read(reportInternalId);
                if (report.RecommenderId != campaign.Id)
                {
                    throw new EntityNotFoundException(typeof(ItemsRecommenderPerformanceReport), reportId, "Report is not for selected campaign");
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

        /// <summary>Sets the UseOptimiser property of the campaign.</summary>
        [HttpPost("{id}/UseOptimiser")]
        public async Task<PromotionsCampaign> SetUseOptimiser(string id, [FromBody] UseOptimiserDto dto, bool? useInternalId = null)
        {
            var campaign = await store.GetEntity(id, useInternalId);
            campaign.UseOptimiser = dto.UseOptimiser;
            await store.Context.SaveChanges();
            return campaign;
        }

        /// <summary>Remove a promotion association with a campaign.</summary>
        [HttpDelete("{id}/Items/{itemId}")]
        [HttpDelete("{id}/Promotions/{itemId}")]
        public async Task<RecommendableItem> RemoveItem(string id, string itemId, bool? useInternalId = null)
        {
            var campaign = await GetEntity(id, useInternalId);
            return await workflows.RemoveItem(campaign, itemId, useInternalId);
        }

        protected override Task<(bool, string)> CanDelete(PromotionsCampaign entity)
        {
            return Task.FromResult((true, ""));
        }
    }
}
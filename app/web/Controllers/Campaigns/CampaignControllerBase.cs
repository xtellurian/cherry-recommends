
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalBox.Core;
using SignalBox.Core.Recommendations.Destinations;
using SignalBox.Core.Campaigns;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    public abstract class CampaignsControllerBase<T> : CommonEntityControllerBase<T> where T : CampaignEntityBase
    {
        private readonly CampaignInvokationWorkflowBase<T> invokationWorkflows;
        private readonly IAudienceStore audienceStore;
        private readonly CampaignWorkflowBase<T> workflows;

        protected CampaignsControllerBase(ICampaignStore<T> store,
                                            IAudienceStore audienceStore,
                                            CampaignWorkflowBase<T> workflows,
                                            CampaignInvokationWorkflowBase<T> invokationWorkflows) : base(store)
        {
            this.invokationWorkflows = invokationWorkflows;
            this.audienceStore = audienceStore;
            this.workflows = workflows;
        }

        protected virtual void ValidateInvokationDto(IModelInput dto)
        {
            if (dto.Metrics != null && dto.Metrics.Any())
            {
                throw new BadRequestException("Features cannot be set via the API");
            }
        }

        [HttpGet("{id}/InvokationLogs")]
        public async Task<Paginated<InvokationLogEntry>> GetInvokationLogs(string id, [FromQuery] PaginateRequest p, bool? useInternalId = null)
        {
            var campaign = await base.GetEntity(id, useInternalId);
            return await invokationWorkflows.QueryInvokationLogs(p, campaign);
        }

        [HttpPost("{id}/ErrorHandling")]
        public async Task<CampaignErrorHandling> SetErrorHandling(string id, CampaignErrorHandling dto)
        {
            var campaign = await base.GetResource(id);
            campaign.ErrorHandling = dto;
            campaign.Settings ??= new CampaignSettings();
            campaign.Settings.ThrowOnBadInput = dto.ThrowOnBadInput;
            await store.Update(campaign);
            await store.Context.SaveChanges();
            return campaign.ErrorHandling;
        }

        [HttpPost("{id}/Settings")]
        public async Task<CampaignSettings> SetSettings(string id, CampaignSettingsDto dto)
        {
            var campaign = await base.GetResource(id);
            bool prevEnabled = campaign.Settings.Enabled;
            campaign.Settings = dto.ToCoreRepresentation(campaign.Settings, true);

            // Re-enabling the recommender will delete the expiry date
            if (campaign.Settings.Enabled && !prevEnabled)
            {
                campaign.Settings.ExpiryDate = null;
            }

            await store.Update(campaign);
            await store.Context.SaveChanges();
            return campaign.Settings;
        }

        [HttpGet("{id}/Settings")]
        public async Task<CampaignSettings> GetSettings(string id)
        {
            var campaign = await base.GetResource(id);
            return campaign.Settings;
        }

        [HttpGet("{id}/ChoosePromotionArgumentRules")]
        public async Task<IEnumerable<ChoosePromotionArgumentRule>> GetChoosePromotionArgumentRules(string id, bool? useInternalId = null)
        {
            var campaign = await GetEntity(id, useInternalId);
            await store.LoadMany(campaign, _ => _.ArgumentRules);
            return campaign.ArgumentRules.AsDerived<ArgumentRule, ChoosePromotionArgumentRule>();
        }

        [HttpPost("{id}/ChoosePromotionArgumentRules")]
        public async Task<ArgumentRule> CreateChoosePromotionArgumentRule(string id, CreateChoosePromotionArgumentRuleDto dto, bool? useInternalId = null)
        {
            var campaign = await base.GetEntity(id, useInternalId);
            return await workflows.CreateChoosePromotionArgumentRule(campaign, dto.ArgumentId, dto.PromotionId, dto.ArgumentValue);
        }

        [HttpPost("{id}/ChoosePromotionArgumentRules/{ruleId}")]
        public async Task<ArgumentRule> UpdateChoosePromotionArgumentRule(string id, UpdateChoosePromotionArgumentRuleDto dto, long ruleId, bool? useInternalId = null)
        {
            var campaign = await base.GetEntity(id, useInternalId);
            return await workflows.UpdateChoosePromotionArgumentRule(campaign, ruleId, dto.PromotionId, dto.ArgumentValue);
        }

        [HttpGet("{id}/ChooseSegmentArgumentRules")]
        public async Task<IEnumerable<ChooseSegmentArgumentRule>> GetChooseSegmentArgumentRules(string id, bool? useInternalId = null)
        {
            var campaign = await GetEntity(id, useInternalId);
            await store.LoadMany(campaign, _ => _.ArgumentRules);
            return campaign.ArgumentRules.AsDerived<ArgumentRule, ChooseSegmentArgumentRule>();
        }

        [HttpPost("{id}/ChooseSegmentArgumentRules")]
        public async Task<ArgumentRule> CreateChooseSegmentArgumentRule(string id, CreateChooseSegmentArgumentRuleDto dto, bool? useInternalId = null)
        {
            var campaign = await base.GetEntity(id, useInternalId);
            return await workflows.CreateChooseSegmentArgumentRule(campaign, dto.ArgumentId, dto.SegmentId, dto.ArgumentValue);
        }

        [HttpPost("{id}/ChooseSegmentArgumentRules/{ruleId}")]
        public async Task<ArgumentRule> UpdateChooseSegmentArgumentRule(string id, UpdateChooseSegmentArgumentRuleDto dto, long ruleId, bool? useInternalId = null)
        {
            var campaign = await base.GetEntity(id, useInternalId);
            return await workflows.UpdateChooseSegmentArgumentRule(campaign, ruleId, dto.SegmentId, dto.ArgumentValue);
        }

        [HttpDelete("{id}/ArgumentRules/{ruleId}")]
        public async Task<DeleteResponse> DeleteArgumentRule(string id, long ruleId, bool? useInternalId = null)
        {
            var campaign = await GetEntity(id, useInternalId);
            await workflows.DeleteArgumentRule(campaign, ruleId);
            return new DeleteResponse(ruleId, Request.Path.Value, true);
        }

        [HttpGet("{id}/Arguments")]
        public async Task<IEnumerable<CampaignArgument>> GetArguments(string id, bool? useInternalId = null)
        {
            var campaign = await base.GetEntity(id, useInternalId);
            await store.LoadMany(campaign, _ => _.Arguments);
            return campaign.Arguments;
        }

        [HttpPost("{id}/Arguments")]
        public async Task<IEnumerable<CampaignArgument>> SetArguments(string id, IEnumerable<CreateOrUpdateCampaignArgument> dto, bool? useInternalId = null)
        {
            var campaign = await base.GetResource(id, useInternalId);
            await store.LoadMany(campaign, _ => _.Arguments);
            campaign.Arguments = dto.ToCoreRepresentation();
            await store.Update(campaign);
            await store.Context.SaveChanges();
            return campaign.Arguments;
        }

        [HttpGet("{id}/Destinations")]
        public async Task<IEnumerable<RecommendationDestinationBase>> GetDestinations(string id)
        {
            var campaign = await base.GetResource(id);
            await store.LoadMany(campaign, _ => _.RecommendationDestinations);
            return campaign.RecommendationDestinations;
        }

        [HttpPost("{id}/Destinations/")]
        public async Task<RecommendationDestinationBase> AddDestination(string id, CreateDestinationDto dto)
        {
            var campaign = await base.GetResource(id);
            var d = await workflows.AddDestination(campaign, dto.IntegratedSystemId, dto.DestinationType, dto.Endpoint);
            return d;
        }

        [HttpDelete("{id}/Destinations/{destinationId}")]
        public async Task<CampaignEntityBase> RemoveDestination(string id, long destinationId)
        {
            var campaign = await base.GetResource(id);
            var d = await workflows.RemoveDestination(campaign, destinationId);
            return d;
        }

        [HttpGet("{id}/TriggerCollection")]
        public async Task<TriggerCollection> GetTrigger(string id, bool? useInternalId = null)
        {
            var campaign = await base.GetEntity(id, useInternalId);
            return campaign.TriggerCollection;
        }

        [HttpPost("{id}/TriggerCollection")]
        public async Task<TriggerCollection> SetTrigger(string id, SetTriggersDto dto, bool? useInternalId = null)
        {
            var campaign = await base.GetResource(id, useInternalId);
            campaign.TriggerCollection ??= new TriggerCollection();
            campaign.TriggerCollection = dto.ToCoreRepresentation(campaign.TriggerCollection);
            await store.Update(campaign);
            await store.Context.SaveChanges();
            return campaign.TriggerCollection;
        }

        [HttpGet("{id}/LearningFeatures")] // backwards compat
        [HttpGet("{id}/LearningMetrics")]
        public async Task<IEnumerable<Metric>> GetLearningMetrics(string id, bool? useInternalId = null)
        {
            var campaign = await base.GetEntity(id, useInternalId);
            await store.LoadMany(campaign, _ => _.LearningFeatures);
            return campaign.LearningFeatures;
        }

        [HttpPost("{id}/LearningFeatures")]
        [HttpPost("{id}/LearningMetrics")]
        public async Task<IEnumerable<Metric>> SetLearningMetrics(string id, SetLearningMetrics dto, bool? useInternalId = null)
        {
            var campaign = await base.GetResource(id, useInternalId);
            dto.Validate();
            campaign = await workflows.SetLearningMetrics(campaign, dto.MetricIds, dto.UseInternalId);
            return campaign.LearningFeatures;
        }

        [HttpGet("{id}/ReportImage")]
        public async Task<FileResult> GetReportImage(string id, bool? useInternalId = null)
        {
            try
            {
                var campaign = await base.GetEntity(id, useInternalId);
                var fileBytes = await workflows.DownloadReportImage(campaign);
                return File(fileBytes, "image/png", "report.png");
            }
            catch (Azure.RequestFailedException requestFailedEx)
            {
                System.Console.WriteLine(requestFailedEx.Message);
                if (requestFailedEx.ErrorCode == "BlobNotFound")
                {
                    throw new ResourceNotFoundException(requestFailedEx);
                }
                else if (requestFailedEx.ErrorCode == "ContainerNotFound")
                {
                    throw new DependencyException("Container not found.", requestFailedEx);
                }
                else
                {
                    throw new WorkflowException("Error downloading report image.", requestFailedEx);
                }
            }
        }

        [HttpGet("{id}/Audience")]
        public async Task<Audience> GetAudience(string id)
        {
            var campaign = await base.GetResource(id);
            var audience = await audienceStore.GetAudience(campaign);

            if (!audience.Success)
            {
                throw audience.Exception;
            }

            return audience.Entity;
        }

        [HttpPost("{id}/Audience/Segments")]
        public async Task<Audience> AddAudienceSegment(string id, [FromBody] AddAudienceSegmentDto dto)
        {
            var campaign = await base.GetResource(id);
            return await workflows.AddAudienceSegment(campaign, dto.SegmentId);
        }

        [HttpDelete("{id}/Audience/Segments/{segmentId}")]
        public async Task<DeleteResponse> RemoveAudienceSegment(string id, long segmentId)
        {
            var campaign = await base.GetResource(id);
            await workflows.RemoveAudienceSegment(campaign, segmentId);
            return new DeleteResponse(segmentId, Request.Path.Value, true); ;
        }

        [HttpGet("{id}/Channels")]
        public async Task<IEnumerable<ChannelBase>> GetChannels(string id)
        {
            var campaign = await base.GetResource(id);
            await store.LoadMany(campaign, _ => _.Channels);
            return campaign.Channels;
        }

        [HttpPost("{id}/Channels/")]
        public async Task<ChannelBase> AddChannel(string id, AddCampaignChannelDto dto)
        {
            var campaign = await base.GetResource(id);
            var result = await workflows.AddChannel(campaign, dto.Id);
            return result;
        }

        [HttpDelete("{id}/Channels/{channelId}")]
        public async Task<CampaignEntityBase> RemoveChannel(string id, long channelId)
        {
            var campaign = await base.GetResource(id);
            var channel = await workflows.RemoveChannel(campaign, channelId);
            return campaign;
        }
    }
}

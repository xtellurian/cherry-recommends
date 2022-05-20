
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalBox.Core;
using SignalBox.Core.Recommendations.Destinations;
using SignalBox.Core.Recommenders;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    public abstract class RecommenderControllerBase<T> : CommonEntityControllerBase<T> where T : RecommenderEntityBase
    {
        private readonly RecommenderInvokationWorkflowBase<T> invokationWorkflows;
        private readonly IAudienceStore audienceStore;
        private readonly RecommenderWorkflowBase<T> workflows;

        protected RecommenderControllerBase(IRecommenderStore<T> store,
                                            IAudienceStore audienceStore,
                                            RecommenderWorkflowBase<T> workflows,
                                            RecommenderInvokationWorkflowBase<T> invokationWorkflows) : base(store)
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
            var recommender = await base.GetEntity(id, useInternalId);
            return await invokationWorkflows.QueryInvokationLogs(p, recommender);
        }

        [HttpPost("{id}/ErrorHandling")]
        public async Task<RecommenderErrorHandling> SetErrorHandling(string id, RecommenderErrorHandling dto)
        {
            var recommender = await base.GetResource(id);
            recommender.ErrorHandling = dto;
            recommender.Settings ??= new RecommenderSettings();
            recommender.Settings.ThrowOnBadInput = dto.ThrowOnBadInput;
            await store.Update(recommender);
            await store.Context.SaveChanges();
            return recommender.ErrorHandling;
        }

        [HttpPost("{id}/Settings")]
        public async Task<RecommenderSettings> SetSettings(string id, RecommenderSettingsDto dto)
        {
            var recommender = await base.GetResource(id);
            bool prevEnabled = recommender.Settings.Enabled;
            recommender.Settings = dto.ToCoreRepresentation(recommender.Settings, true);

            // Re-enabling the recommender will delete the expiry date
            if (recommender.Settings.Enabled && !prevEnabled)
            {
                recommender.Settings.ExpiryDate = null;
            }

            await store.Update(recommender);
            await store.Context.SaveChanges();
            return recommender.Settings;
        }

        [HttpGet("{id}/Settings")]
        public async Task<RecommenderSettings> GetSettings(string id)
        {
            var recommender = await base.GetResource(id);
            return recommender.Settings;
        }

        [HttpGet("{id}/Arguments")]
        public async Task<IEnumerable<CampaignArgument>> GetArguments(string id, bool? useInternalId = null)
        {
            var recommender = await base.GetEntity(id, useInternalId);
            await store.LoadMany(recommender, _ => _.Arguments);
            return recommender.Arguments;
        }

        [HttpPost("{id}/Arguments")]
        public async Task<IEnumerable<CampaignArgument>> SetArguments(string id, IEnumerable<CreateOrUpdateRecommenderArgument> dto, bool? useInternalId = null)
        {
            var recommender = await base.GetResource(id, useInternalId);
            await store.LoadMany(recommender, _ => _.Arguments);
            recommender.Arguments = dto.ToCoreRepresentation();
            await store.Update(recommender);
            await store.Context.SaveChanges();
            return recommender.Arguments;
        }

        [HttpGet("{id}/Destinations")]
        public async Task<IEnumerable<RecommendationDestinationBase>> GetDestinations(string id)
        {
            var recommender = await base.GetResource(id);
            await store.LoadMany(recommender, _ => _.RecommendationDestinations);
            return recommender.RecommendationDestinations;
        }

        [HttpPost("{id}/Destinations/")]
        public async Task<RecommendationDestinationBase> AddDestination(string id, CreateDestinationDto dto)
        {
            var recommender = await base.GetResource(id);
            var d = await workflows.AddDestination(recommender, dto.IntegratedSystemId, dto.DestinationType, dto.Endpoint);
            return d;
        }

        [HttpDelete("{id}/Destinations/{destinationId}")]
        public async Task<RecommenderEntityBase> RemoveDestination(string id, long destinationId)
        {
            var recommender = await base.GetResource(id);
            var d = await workflows.RemoveDestination(recommender, destinationId);
            return d;
        }

        [HttpGet("{id}/TriggerCollection")]
        public async Task<TriggerCollection> GetTrigger(string id, bool? useInternalId = null)
        {
            var recommender = await base.GetEntity(id, useInternalId);
            return recommender.TriggerCollection;
        }

        [HttpPost("{id}/TriggerCollection")]
        public async Task<TriggerCollection> SetTrigger(string id, SetTriggersDto dto, bool? useInternalId = null)
        {
            var recommender = await base.GetResource(id, useInternalId);
            recommender.TriggerCollection ??= new TriggerCollection();
            recommender.TriggerCollection = dto.ToCoreRepresentation(recommender.TriggerCollection);
            await store.Update(recommender);
            await store.Context.SaveChanges();
            return recommender.TriggerCollection;
        }

        [HttpGet("{id}/LearningFeatures")] // backwards compat
        [HttpGet("{id}/LearningMetrics")]
        public async Task<IEnumerable<Metric>> GetLearningMetrics(string id, bool? useInternalId = null)
        {
            var recommender = await base.GetEntity(id, useInternalId);
            await store.LoadMany(recommender, _ => _.LearningFeatures);
            return recommender.LearningFeatures;
        }

        [HttpPost("{id}/LearningFeatures")]
        [HttpPost("{id}/LearningMetrics")]
        public async Task<IEnumerable<Metric>> SetLearningMetrics(string id, SetLearningMetrics dto, bool? useInternalId = null)
        {
            var recommender = await base.GetResource(id, useInternalId);
            dto.Validate();
            recommender = await workflows.SetLearningMetrics(recommender, dto.MetricIds, dto.UseInternalId);
            return recommender.LearningFeatures;
        }

        [HttpGet("{id}/ReportImage")]
        public async Task<FileResult> GetReportImage(string id, bool? useInternalId = null)
        {
            try
            {
                var recommender = await base.GetEntity(id, useInternalId);
                var fileBytes = await workflows.DownloadReportImage(recommender);
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
            var recommender = await base.GetResource(id);
            var audience = await audienceStore.GetAudience(recommender);

            if (!audience.Success)
            {
                throw audience.Exception;
            }

            return audience.Entity;
        }

        [HttpGet("{id}/Channels")]
        public async Task<IEnumerable<ChannelBase>> GetChannels(string id)
        {
            var recommender = await base.GetResource(id);
            await store.LoadMany(recommender, _ => _.Channels);
            return recommender.Channels;
        }

        [HttpPost("{id}/Channels/")]
        public async Task<ChannelBase> AddChannel(string id, AddRecommenderChannelDto dto)
        {
            var recommender = await base.GetResource(id);
            var result = await workflows.AddChannel(recommender, dto.Id);
            return result;
        }

        [HttpDelete("{id}/Channels/{channelId}")]
        public async Task<RecommenderEntityBase> RemoveChannel(string id, long channelId)
        {
            var recommender = await base.GetResource(id);
            var channel = await workflows.RemoveChannel(recommender, channelId);
            return recommender;
        }
    }
}

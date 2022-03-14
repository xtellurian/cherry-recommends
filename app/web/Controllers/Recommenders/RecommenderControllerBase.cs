
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
        private readonly IRecommenderStore<T> recommenderStore;
        private readonly ISegmentStore segmentStore;
        private readonly RecommenderWorkflowBase<T> workflows;

        protected RecommenderControllerBase(IRecommenderStore<T> store,
                                            ISegmentStore segmentStore,
                                            RecommenderWorkflowBase<T> workflows,
                                            RecommenderInvokationWorkflowBase<T> invokationWorkflows) : base(store)
        {
            this.invokationWorkflows = invokationWorkflows;
            this.recommenderStore = store;
            this.segmentStore = segmentStore;
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
            return await invokationWorkflows.QueryInvokationLogs(recommender, p.Page);
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
            recommender.Settings = dto.ToCoreRepresentation(recommender.Settings, true);
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
        public async Task<IEnumerable<RecommenderArgument>> GetArguments(string id, bool? useInternalId = null)
        {
            var recommender = await base.GetEntity(id, useInternalId);
            return recommender.Arguments;
        }

        [HttpPost("{id}/Arguments")]
        public async Task<IEnumerable<RecommenderArgument>> SetArguments(string id, IEnumerable<CreateOrUpdateRecommenderArgument> dto, bool? useInternalId = null)
        {
            var recommender = await base.GetResource(id, useInternalId);
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
                    throw new ResourceNotFoundException();
                }
                else
                {
                    throw new WorkflowException("Error downloading report image");
                }
            }
        }

        [HttpGet("{id}/Segments")]
        public async Task<IEnumerable<Core.Segment>> GetSegments(string id)
        {
            var recommender = await base.GetResource(id);

            return await segmentStore.GetSegmentsByRecommender(recommender);
        }

        [HttpPost("{id}/Segments")]
        public async Task<IEnumerable<RecommenderSegment>> AddSegments(string id, [FromBody] IEnumerable<long> segmentIds)
        {
            var recommender = await base.GetResource(id);

            return await workflows.SetSegments(recommender, segmentIds);
        }

        [HttpDelete("{id}/Segments")]
        public async Task<IEnumerable<RecommenderSegment>> RemoveSegments(string id, [FromBody] IEnumerable<long> segmentIds)
        {
            var recommender = await base.GetResource(id);

            return await workflows.RemoveSegments(recommender, segmentIds);
        }

        [HttpDelete("{id}/Segments/{segmentId}")]
        public async Task<IEnumerable<RecommenderSegment>> RemoveSegment(string id, long segmentId)
        {
            var recommender = await base.GetResource(id);

            return await workflows.RemoveSegments(recommender, new long[1] { segmentId });
        }
    }
}

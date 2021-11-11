using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Features.Destinations;
using SignalBox.Core.Security;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class FeaturesController : CommonEntityControllerBase<Feature>
    {
        private readonly ILogger<FeaturesController> logger;
        private readonly FeatureWorkflows workflows;

        public FeaturesController(ILogger<FeaturesController> logger, IFeatureStore store, FeatureWorkflows workflows) : base(store)
        {
            this.logger = logger;
            this.workflows = workflows;
        }

        /// <summary>Creates a new generic Feature that can used on any tracked user.</summary>
        [HttpPost]
        [Authorize(Policies.AdminOnlyPolicyName)]
        public async Task<Feature> CreateFeature([FromBody] CreateFeatureMetadata dto)
        {
            return await workflows.CreateFeature(dto.CommonId, dto.Name);
        }

        /// <summary>Get's the tracked users associated with a feature.</summary>
        [HttpGet("{id}/TrackedUsers")]
        public async Task<Paginated<TrackedUser>> GetAssociatedTrackedUsers(string id, [FromQuery] PaginateRequest p)
        {
            var feature = await base.GetResource(id);
            return await workflows.GetTrackedUsers(feature, p.Page);
        }

        /// <summary>Get's the latest tracked user features (values) for a feature.</summary>
        [HttpGet("{id}/TrackedUserFeatures")]
        public async Task<Paginated<HistoricTrackedUserFeature>> GetLatestTrackedUserFeatures(string id, [FromQuery] PaginateRequest p)
        {
            var feature = await base.GetResource(id);
            var users = await workflows.GetTrackedUsers(feature, p.Page);
            var featureValues = new List<HistoricTrackedUserFeature>();
            foreach (var trackedUser in users.Items)
            {
                featureValues.Add(await workflows.ReadFeatureValues(trackedUser, feature.CommonId));
            }

            return new Paginated<HistoricTrackedUserFeature>(featureValues, users.Pagination.PageCount, users.Pagination.TotalItemCount, users.Pagination.PageNumber);
        }

        [HttpGet("{id}/Destinations")]
        public async Task<IEnumerable<FeatureDestinationBase>> GetDestinations(string id)
        {
            var feature = await base.GetResource(id);
            await store.LoadMany(feature, _ => _.Destinations);
            return feature.Destinations;
        }

        [HttpPost("{id}/Destinations/")]
        public async Task<FeatureDestinationBase> AddDestination(string id, CreateDestinationDto dto)
        {
            var feature = await base.GetResource(id);
            dto.Validate();
            var d = await workflows.AddDestination(feature, dto.IntegratedSystemId, dto.DestinationType, 
                endpoint: dto.Endpoint, 
                propertyName: dto.PropertyName);
            return d;
        }

        [HttpDelete("{id}/Destinations/{destinationId}")]
        public async Task<FeatureDestinationBase> RemoveDestination(string id, long destinationId)
        {
            var feature = await base.GetResource(id);
            var d = await workflows.RemoveDestination(feature, destinationId);
            return d;
        }

        protected override Task<(bool, string)> CanDelete(Feature entity)
        {
            return Task.FromResult((true, ""));
        }
    }
}

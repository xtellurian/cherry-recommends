using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
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

        protected override Task<(bool, string)> CanDelete(Feature entity)
        {
            return Task.FromResult((true, ""));
        }
    }
}

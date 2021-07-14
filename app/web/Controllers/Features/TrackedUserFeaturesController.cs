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
    [Route("api/trackedusers")]
    public class TrackedUserFeaturesController : SignalBoxControllerBase
    {
        private readonly ILogger<TrackedUserFeaturesController> _logger;
        private readonly FeatureWorkflows workflows;
        private readonly ITrackedUserFeatureStore trackedUserFeatureStore;
        private readonly ITrackedUserStore trackedUserStore;

        public TrackedUserFeaturesController(ILogger<TrackedUserFeaturesController> logger,
                                             FeatureWorkflows workflows,
                                             ITrackedUserFeatureStore trackedUserFeatureStore,
                                             ITrackedUserStore trackedUserStore)
        {
            _logger = logger;
            this.workflows = workflows;
            this.trackedUserFeatureStore = trackedUserFeatureStore;
            this.trackedUserStore = trackedUserStore;
        }

        /// <summary>Returns a list of features available for a tracked user.</summary>
        [HttpGet("{id}/features")]
        public async Task<IEnumerable<Feature>> AvailableFeatures(string id, bool? useInternalId = null)
        {
            var trackedUser = await LoadTrackedUser(trackedUserStore, id, useInternalId);
            return await trackedUserFeatureStore.GetFeaturesFor(trackedUser);
        }

        /// <summary>Creates a new set of feature values on a user. You probably shouldn't set this manually.</summary>
        [HttpPost("{id}/features/{featureCommonId}")]
        [Authorize(Policies.AdminOnlyPolicyName)]
        public async Task<TrackedUserFeature> Create(string id,
                                         string featureCommonId,
                                         [FromBody] CreateTrackedUserFeature dto,
                                         [FromQuery] bool? useInternalId = null)
        {
            var trackedUser = await LoadTrackedUser(trackedUserStore, id, useInternalId);
            return await workflows.CreateFeatureOnUser(trackedUser, featureCommonId, dto.Value);
        }

        /// <summary>Returns the value set in the feature.</summary>
        [HttpGet("{id}/features/{featureCommonId}")]
        public async Task<TrackedUserFeature> FeatureValue(string id,
                                         string featureCommonId,
                                         [FromQuery] bool? useInternalId = null,
                                         [FromQuery] int? version = null)
        {
            var trackedUser = await LoadTrackedUser(trackedUserStore, id, useInternalId);
            return await workflows.ReadFeatureValues(trackedUser, featureCommonId, version);
        }
    }
}

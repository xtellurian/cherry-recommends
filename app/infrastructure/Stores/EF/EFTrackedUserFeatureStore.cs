using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFTrackedUserFeatureStore : EFEntityStoreBase<TrackedUserFeature>, ITrackedUserFeatureStore
    {
        public EFTrackedUserFeatureStore(SignalBoxDbContext context) : base(context, c => c.TrackedUserFeatures)
        { }

        public async Task<int> CurrentMaximumFeatureVersion(TrackedUser trackedUser, Feature feature)
        {
            trackedUser = await context.TrackedUsers
               .Include(_ => _.TrackedUserFeatures)
               .ThenInclude(_ => _.Feature)
               .FirstAsync(_ => _.Id == trackedUser.Id);

            var existing = trackedUser.TrackedUserFeatures
                .Where(_ => _.FeatureId == feature.Id).ToList();

            return existing
                .Select(_ => _.Version)
                .DefaultIfEmpty(0)
                .Max();
        }

        public async Task<IEnumerable<Feature>> GetFeaturesFor(TrackedUser trackedUser)
        {
            trackedUser = await context.TrackedUsers
                .Include(_ => _.TrackedUserFeatures)
                .ThenInclude(_ => _.Feature)
                .FirstAsync(_ => _.Id == trackedUser.Id);

            return trackedUser.TrackedUserFeatures.Select(_ => _.Feature).ToList();
        }

        public async Task<TrackedUserFeature> ReadFeature(TrackedUser trackedUser, Feature feature, int? version = null)
        {
            version ??= await CurrentMaximumFeatureVersion(trackedUser, feature);
            trackedUser = await context.TrackedUsers
                .Include(_ => _.TrackedUserFeatures)
                .ThenInclude(_ => _.Feature)
                .FirstAsync(_ => _.Id == trackedUser.Id);

            return trackedUser.TrackedUserFeatures.First(_ => _.FeatureId == feature.Id && _.Version == version.Value);

        }

        public async Task<bool> FeatureExists(TrackedUser trackedUser, Feature feature, int? version = null)
        {
            version ??= await CurrentMaximumFeatureVersion(trackedUser, feature);
            trackedUser = await context.TrackedUsers
                .Include(_ => _.TrackedUserFeatures)
                .ThenInclude(_ => _.Feature)
                .FirstAsync(_ => _.Id == trackedUser.Id);

            return trackedUser.TrackedUserFeatures.Any(_ => _.FeatureId == feature.Id && _.Version == version.Value);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFHistoricTrackedUserFeatureStore : EFEntityStoreBase<HistoricTrackedUserFeature>, IHistoricTrackedUserFeatureStore
    {
        public EFHistoricTrackedUserFeatureStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, c => c.HistoricTrackedUserFeatures)
        { }

        public async Task<int> CurrentMaximumFeatureVersion(Customer customer, Feature feature)
        {
            var latest = await context.LatestFeatureVersions
                .Where(_ => _.FeatureId == feature.Id && _.TrackedUserId == customer.Id)
                .FirstOrDefaultAsync();
            return latest?.MaxVersion ?? 0;

            // trackedUser = await context.TrackedUsers
            //    .Include(_ => _.HistoricTrackedUserFeatures)
            //    .ThenInclude(_ => _.Feature)
            //    .FirstAsync(_ => _.Id == trackedUser.Id);

            // var existing = trackedUser.HistoricTrackedUserFeatures
            //     .Where(_ => _.FeatureId == feature.Id).ToList();

            // return existing
            //     .Select(_ => _.Version)
            //     .DefaultIfEmpty(0)
            //     .Max();
        }

        public async Task<IEnumerable<Feature>> GetFeaturesFor(Customer customer)
        {
            var features = await context.HistoricTrackedUserFeatures
                .Where(_ => _.TrackedUserId == customer.Id)
                .Include(_ => _.Feature)
                .Select(_ => _.Feature)
                .Distinct()
                .ToListAsync();

            return features;
        }

        public async Task<HistoricTrackedUserFeature> ReadFeature(Customer customer, Feature feature, int? version = null)
        {
            version ??= await CurrentMaximumFeatureVersion(customer, feature);
            customer = await context.Customers
                .Include(_ => _.HistoricTrackedUserFeatures)
                .ThenInclude(_ => _.Feature)
                .FirstAsync(_ => _.Id == customer.Id);

            return customer.HistoricTrackedUserFeatures.First(_ => _.FeatureId == feature.Id && _.Version == version.Value);
        }

        public async Task<bool> FeatureExists(Customer customer, Feature feature, int? version = null)
        {
            version ??= await CurrentMaximumFeatureVersion(customer, feature);
            customer = await context.Customers
                .Include(_ => _.HistoricTrackedUserFeatures)
                .ThenInclude(_ => _.Feature)
                .FirstAsync(_ => _.Id == customer.Id);

            return customer.HistoricTrackedUserFeatures.Any(_ => _.FeatureId == feature.Id && _.Version == version.Value);
        }
    }
}
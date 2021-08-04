using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SignalBox.Core.Workflows
{
    public class FeatureWorkflows
    {
        private readonly IFeatureStore featureStore;
        private readonly ITrackedUserFeatureStore trackedUserFeatureStore;
        private readonly ITrackedUserStore trackedUserStore;
        private readonly ILogger<FeatureWorkflows> logger;
        private readonly IStorageContext storageContext;

        public FeatureWorkflows(IFeatureStore featureStore,
                                   ITrackedUserFeatureStore trackedUserFeatureStore,
                                   ITrackedUserStore trackedUserStore,
                                   ILogger<FeatureWorkflows> logger,
                                   IStorageContext storageContext)
        {
            this.featureStore = featureStore;
            this.trackedUserFeatureStore = trackedUserFeatureStore;
            this.trackedUserStore = trackedUserStore;
            this.logger = logger;
            this.storageContext = storageContext;
        }

        public async Task<TrackedUserFeature> CreateFeatureOnUser(TrackedUser trackedUser,
                                                                   string featureCommonId,
                                                                   object value,
                                                                   bool? forceIncrementVersion)
        {
            Feature feature;
            if (await featureStore.ExistsFromCommonId(featureCommonId))
            {
                feature = await featureStore.ReadFromCommonId(featureCommonId);
            }
            else
            {
                throw new BadRequestException($"Feature {featureCommonId} does not exist");
            }

            var currentVersion = await trackedUserFeatureStore.CurrentMaximumFeatureVersion(trackedUser, feature);
            var newFeatureValue = GenerateFeatureValues(trackedUser, feature, value, currentVersion + 1);
            if (forceIncrementVersion == true || currentVersion == 0) // first time or incrementing
            {
                newFeatureValue = await trackedUserFeatureStore.Create(newFeatureValue);
                await storageContext.SaveChanges();
                return newFeatureValue;
            }
            else // check whether the value has changed before updating.
            {
                var currentFeatureValue = await trackedUserFeatureStore.ReadFeature(trackedUser, feature, currentVersion);
                if (!newFeatureValue.ValuesEqual(currentFeatureValue))
                {
                    // values aren't equal, create a new feature.
                    newFeatureValue = await trackedUserFeatureStore.Create(newFeatureValue);
                    await storageContext.SaveChanges();
                    return newFeatureValue;
                }
                else // values are equal, do don't create a new feature.
                {
                    logger.LogInformation("Skipping update to Feature. Values are equal");
                    return currentFeatureValue;
                }
            }
        }

        public async Task<Feature> CreateFeature(string commonId, string name)
        {
            var feature = await featureStore.Create(new Feature(commonId, name));
            await storageContext.SaveChanges();
            return feature;
        }

        public async Task<Paginated<TrackedUser>> GetTrackedUsers(Feature feature, int page)
        {
            return await featureStore.QueryTrackedUsers(page, feature.Id);
        }

        public async Task<TrackedUserFeature> ReadFeatureValues(TrackedUser trackedUser, string featureCommonId, int? version = null)
        {
            var feature = await featureStore.ReadFromCommonId(featureCommonId);
            return await trackedUserFeatureStore.ReadFeature(trackedUser, feature, version);
        }

        private TrackedUserFeature GenerateFeatureValues(TrackedUser user, Feature feature, object value, int version)
        {
            if (value == null)
            {
                throw new System.NullReferenceException("Feature value cannot be null");
            }
            else if (value is double f)
            {
                return new TrackedUserFeature(user, feature, f, version);
            }
            else if (value is int n)
            {
                return new TrackedUserFeature(user, feature, n, version);
            }
            else if (value is string s)
            {
                return new TrackedUserFeature(user, feature, s, version);
            }
            else if (value is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.String)
                {
                    return new TrackedUserFeature(user, feature, jsonElement.GetString(), version);
                }
                if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.Number)
                {
                    if (jsonElement.TryGetInt32(out var i))
                    {
                        return new TrackedUserFeature(user, feature, i, version);
                    }
                    else if (jsonElement.TryGetDouble(out var d))
                    {
                        return new TrackedUserFeature(user, feature, d, version);
                    }
                    else
                    {
                        throw new System.ArgumentException($"{value} JsonElement of ValueKind {jsonElement.ValueKind} is an unknown feature value type");
                    }
                }
            }

            throw new System.ArgumentException($"{value} of type {value.GetType()} is an unknown feature value type");
        }
    }
}
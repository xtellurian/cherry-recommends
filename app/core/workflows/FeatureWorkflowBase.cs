using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
namespace SignalBox.Core.Workflows
{
    public abstract class FeatureWorkflowBase
    {
        protected readonly IFeatureStore featureStore;
        protected readonly IHistoricTrackedUserFeatureStore trackedUserFeatureStore;
        protected readonly IStorageContext storageContext;
        protected readonly ILogger<FeatureWorkflowBase> logger;

        protected FeatureWorkflowBase(IFeatureStore featureStore,
                                   IHistoricTrackedUserFeatureStore trackedUserFeatureStore,
                                   IStorageContext storageContext,
                                   ILogger<FeatureWorkflowBase> logger)
        {
            this.featureStore = featureStore;
            this.trackedUserFeatureStore = trackedUserFeatureStore;
            this.storageContext = storageContext;
            this.logger = logger;
        }
        public async Task<HistoricTrackedUserFeature> CreateFeatureOnUser(TrackedUser trackedUser,
                                                                   string featureCommonId,
                                                                   object value,
                                                                   bool? forceIncrementVersion)
        {
            Feature feature;
            logger.LogInformation($"Creating feature on tracked user {trackedUser.Id}");
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

        private HistoricTrackedUserFeature GenerateFeatureValues(TrackedUser user, Feature feature, object value, int version)
        {
            if (value == null)
            {
                throw new System.NullReferenceException("Feature value cannot be null");
            }
            else if (value is double f)
            {
                return new HistoricTrackedUserFeature(user, feature, f, version);
            }
            else if (value is int n)
            {
                return new HistoricTrackedUserFeature(user, feature, n, version);
            }
            else if (value is string s)
            {
                return new HistoricTrackedUserFeature(user, feature, s, version);
            }
            else if (value is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.String)
                {
                    return new HistoricTrackedUserFeature(user, feature, jsonElement.GetString(), version);
                }
                if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.Number)
                {
                    if (jsonElement.TryGetInt32(out var i))
                    {
                        return new HistoricTrackedUserFeature(user, feature, i, version);
                    }
                    else if (jsonElement.TryGetDouble(out var d))
                    {
                        return new HistoricTrackedUserFeature(user, feature, d, version);
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
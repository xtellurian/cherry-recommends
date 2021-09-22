using System.Linq;
using System.Threading.Tasks;

namespace SignalBox.Core.Adapters.Hubspot
{
    public static class TrackedUserStoreHubspotExtensions
    {
#nullable enable
        public static async Task<TrackedUser> CreateOrUpdateFromHubspotContact(this ITrackedUserStore trackedUserStore, IntegratedSystem system, HubspotContact contact, string? commonIdPropertyName = null, string? propertyPrefix = null)
        {
            var commonId = string.Empty;
            if (string.IsNullOrEmpty(commonIdPropertyName))
            {
                commonId = contact.ObjectId;
            }
            else
            {
                if (!contact.Properties.TryGetValue(commonIdPropertyName, out commonId))
                {
                    throw new BadRequestException($"Hubspot Contact did not have property ${commonIdPropertyName} required for common id.");
                }
            }

            var newProperties = new DynamicPropertyDictionary(contact.Properties);
            newProperties.PrefixAllKeys(propertyPrefix);

            string? name = null;
            contact.Properties.TryGetValue("firstname", out name);
            TrackedUser tu;
            if (await trackedUserStore.ExistsFromCommonId(commonId))
            {
                tu = await trackedUserStore.ReadFromCommonId(commonId);
                tu.Properties ??= new DynamicPropertyDictionary();
                tu.Name ??= name;

                tu.Properties.Merge(newProperties);
                await trackedUserStore.Update(tu);
            }
            else
            {
                tu = await trackedUserStore.Create(new TrackedUser(commonId, name, newProperties));
            }

            await trackedUserStore.LoadMany(tu, _ => _.IntegratedSystemMaps);

            if (!tu.IntegratedSystemMaps.Any(_ => _.Id == system.Id))
            {
                tu.IntegratedSystemMaps.Add(new TrackedUserSystemMap(contact.ObjectId, system, tu));
            }

            return tu;
        }
    }
}
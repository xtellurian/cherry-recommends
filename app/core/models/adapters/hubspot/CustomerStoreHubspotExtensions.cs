using System.Linq;
using System.Threading.Tasks;

namespace SignalBox.Core.Adapters.Hubspot
{
    public static class CustomerStoreHubspotExtensions
    {
#nullable enable
        public static async Task<Customer> CreateOrUpdateFromHubspotContact(this ICustomerStore customerStore,
                                                                            IEnvironmentProvider environmentProvider,
                                                                            IntegratedSystem system,
                                                                            HubspotContact contact,
                                                                            string? commonIdPropertyName = null,
                                                                            string? propertyPrefix = null)
        {
            if (system.EnvironmentId != null)
            {
                // set the environment to the hubspot environment.
                // this causes any customer created to be in this env.
                environmentProvider.SetOverride(system.EnvironmentId.Value);
            }

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

            contact.Properties.TryGetValue("firstname", out string? name);
            Customer tu;
            if (await customerStore.ExistsFromCommonId(commonId))
            {
                tu = await customerStore.ReadFromCommonId(commonId);
                tu.Properties ??= new DynamicPropertyDictionary();
                tu.Name ??= name;

                tu.Properties.Merge(newProperties);
                await customerStore.Update(tu);
            }
            else
            {
                tu = await customerStore.Create(new Customer(commonId, name, newProperties));
            }

            await customerStore.LoadMany(tu, _ => _.IntegratedSystemMaps);

            if (!tu.IntegratedSystemMaps.Any(_ => _.Id == system.Id))
            {
                tu.IntegratedSystemMaps.Add(new TrackedUserSystemMap(contact.ObjectId, system, tu));
            }

            return tu;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalBox.Core.Adapters.Hubspot;
using SignalBox.Core.Integrations;
using SignalBox.Core.Integrations.Hubspot;

namespace SignalBox.Core.Workflows
{
    public class HubspotEtlWorkflows : HubspotWorkflowBase, IWorkflow
    {
        private readonly IEnvironmentProvider environmentProvider;
        private readonly ITrackedUserSystemMapStore systemMapStore;

        public HubspotEtlWorkflows(ILogger<HubspotEtlWorkflows> logger,
                                   IHubspotService hubspotService,
                                   IOptions<HubspotAppCredentials> hubspotCreds,
                                   IEnvironmentProvider environmentProvider,
                                   IIntegratedSystemStore integratedSystemStore,
                                   ITrackedUserSystemMapStore systemMapStore,
                                   ICustomerStore trackedUserStore,
                                   IDateTimeProvider dateTimeProvider)
        : base(logger, hubspotService, hubspotCreds, integratedSystemStore, trackedUserStore, dateTimeProvider)
        {
            this.environmentProvider = environmentProvider;
            this.systemMapStore = systemMapStore;
        }

        public async Task<HubspotEtlReport> RunHubspotContactEtlJob(IntegratedSystem integratedSystem)
        {
            if (integratedSystem.SystemType != IntegratedSystemTypes.Hubspot)
            {
                throw new System.ArgumentException($"IntegratedSystem {integratedSystem.Id} has SystemType {integratedSystem.SystemType}");
            }

            var report = new HubspotEtlReport(integratedSystem.CommonId);
            var cache = integratedSystem.GetCache<HubspotCache>();
            var propertyNames = cache.ConnectedContactProperties?.PropertyNames?.ToList();

            if (propertyNames != null && propertyNames.Any())
            {
                // do it once.
                var trackedUsers = await GetAndUpdateTrackedUsers(integratedSystem, null, propertyNames);
                report.NumberOfHubspotRequests += 1;
                report.NumberOfTrackedUsersUpdated += trackedUsers.Items.Count();
                await customerStore.Context.SaveChanges(); // todo: use customer workflow
                var after = trackedUsers.Pagination.Next?.After;
                // loop the above in case there are many
                while (!string.IsNullOrEmpty(after) && trackedUsers.Items.Any())
                {
                    trackedUsers = await GetAndUpdateTrackedUsers(integratedSystem, after, propertyNames);
                    report.NumberOfHubspotRequests += 1;
                    await customerStore.Context.SaveChanges();
                    after = trackedUsers.Pagination?.Next?.After;
                    report.NumberOfTrackedUsersUpdated += trackedUsers.Items.Count();
                }
            }
            else
            {
                logger.LogWarning($"Skipping ETL for system id {integratedSystem.Id} - no properties connected");
            }

            return report;
        }

        private async Task<Paginated<Customer>> GetAndUpdateTrackedUsers(IntegratedSystem system, string after, IList<string> properties)
        {
            await base.CheckAndRefreshCredentials(system);
            var cache = system.GetCache<HubspotCache>();
            var commonIdPropertyName = cache.WebhookBehaviour?.CommonUserIdPropertyName;
            var propertyPrefix = cache.WebhookBehaviour?.PropertyPrefix;
            var useObjectIdAsCommonId = string.IsNullOrEmpty(commonIdPropertyName);
            if (!useObjectIdAsCommonId)
            {
                if (properties?.Contains(cache.WebhookBehaviour.CommonUserIdPropertyName) == false)
                {
                    properties.Add(cache.WebhookBehaviour.CommonUserIdPropertyName);
                }
            }

            var hubspotContacts = await hubspotService.GetContacts(system, after, properties);
            var trackedUsers = new List<Customer>();
            foreach (var contact in hubspotContacts.Items)
            {
                var exists = await systemMapStore.ExistsInIntegratedSystem(system.Id, contact.ObjectId) == true;
                if (exists)
                {
                    var tu = await systemMapStore.ReadFromIntegratedSystem(system.Id, contact.ObjectId);
                    tu.Properties ??= new DynamicPropertyDictionary();
                    var newProperties = new DynamicPropertyDictionary(contact.Properties);
                    newProperties.PrefixAllKeys(propertyPrefix);
                    tu.Properties.Merge(newProperties);
                    trackedUsers.Add(tu);
                }
                else if (cache.WebhookBehaviour?.CreateUserIfNotExist == true)
                {
                    if (useObjectIdAsCommonId)
                    {
                        var tu = await customerStore.CreateOrUpdateFromHubspotContact(environmentProvider, system, contact, propertyPrefix: propertyPrefix);
                        trackedUsers.Add(tu);
                    }
                    else if (contact.Properties.ContainsKey(commonIdPropertyName) && !string.IsNullOrEmpty(contact.Properties[commonIdPropertyName]))
                    {
                        var tu = await customerStore.CreateOrUpdateFromHubspotContact(environmentProvider, system, contact, commonIdPropertyName, propertyPrefix: propertyPrefix);
                        trackedUsers.Add(tu);
                    }
                    else
                    {
                        logger.LogWarning($"Contact with Object Id {contact.ObjectId} has no value for property {commonIdPropertyName}");
                    }
                }
                else
                {
                    logger.LogWarning($"Not automatically creating a tracked user for hubspot contact {contact.ObjectId}");
                }
            }

            foreach (var tu in trackedUsers)
            {
                tu.LastUpdated = dateTimeProvider.Now;
            }

            return new Paginated<Customer>(trackedUsers, hubspotContacts.Pagination.Next.After);
        }
    }

    public class HubspotEtlReport
    {
        public HubspotEtlReport(string commonId)
        {
            this.HubspotSystemCommonId = commonId;
        }
        public int NumberOfHubspotRequests { get; set; }
        public string HubspotSystemCommonId { get; set; }
        public int NumberOfTrackedUsersUpdated { get; set; }
    }
}
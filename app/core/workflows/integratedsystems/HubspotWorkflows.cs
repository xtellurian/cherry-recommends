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
    public class HubspotWorkflows : HubspotWorkflowBase, IWorkflow
    {
        private readonly ITenantProvider tenantProvider;
        private readonly CustomerEventsWorkflows eventsWorkflows;
        private readonly ICustomerEventStore eventStore;
        private readonly IEnvironmentProvider environmentProvider;
        private readonly ITrackedUserSystemMapStore systemMapStore;
        private readonly IParameterSetRecommenderStore parameterSetRecommenderStore;
        private readonly IItemsRecommenderStore itemsRecommenderStore;
        private readonly IStorageContext storageContext;
        private readonly ITelemetry telemetry;

        public HubspotWorkflows(IIntegratedSystemStore integratedSystemStore,
                                ITenantProvider tenantProvider,
                                CustomerEventsWorkflows eventsWorkflows,
                                ICustomerStore trackedUserStore,
                                ICustomerEventStore eventStore,
                                IEnvironmentProvider environmentProvider,
                                ITrackedUserSystemMapStore systemMapStore,
                                IParameterSetRecommenderStore parameterSetRecommenderStore,
                                IItemsRecommenderStore itemsRecommenderStore,
                                IStorageContext storageContext,
                                IHubspotService hubspotService,
                                IDateTimeProvider dateTimeProvider,
                                ITelemetry telemetry,
                                ILogger<HubspotWorkflows> logger,
                                IOptions<HubspotAppCredentials> hubspotCreds)
                                : base(logger, hubspotService, hubspotCreds, integratedSystemStore, trackedUserStore, dateTimeProvider)
        {
            this.tenantProvider = tenantProvider;
            this.eventsWorkflows = eventsWorkflows;
            this.eventStore = eventStore;
            this.environmentProvider = environmentProvider;
            this.systemMapStore = systemMapStore;
            this.parameterSetRecommenderStore = parameterSetRecommenderStore;
            this.itemsRecommenderStore = itemsRecommenderStore;
            this.storageContext = storageContext;
            this.telemetry = telemetry;
        }

        public async Task<HubspotCache> GetCache(long integratedSystemId)
        {
            var system = await integratedSystemStore.Read(integratedSystemId);
            if (system.SystemType != IntegratedSystemTypes.Hubspot)
            {
                throw new BadRequestException($"{integratedSystemId} is not a Hubspot System.");
            }

            return system.GetCache<HubspotCache>();
        }

        public async Task<HubspotCache> UpdateCrmCardBehaviour(IntegratedSystem system, MetricCrmCardBehaviour behaviour)
        {
            if (behaviour.ExcludedFeatures != null && behaviour.ExcludedFeatures.Any(_ => _ == null))
            {
                behaviour.ExcludedFeatures = behaviour.ExcludedFeatures.Where(_ => _ != null).ToHashSet(); // remove nulls
            }

            if (behaviour.ParameterSetRecommenderId != null && behaviour.ItemsRecommenderId != null)
            {
                throw new BadRequestException("Must choose only ONE of a items recommender or parameter-set recommender");
            }

            var cache = system.GetCache<HubspotCache>();
            if (behaviour.HasRecommender())
            {
                if (behaviour.ParameterSetRecommenderId.HasValue &&
                    behaviour.ParameterSetRecommenderId != cache.MetricCrmCardBehaviour.ParameterSetRecommenderId)
                {
                    // recommender needs updating
                    if (!await parameterSetRecommenderStore.Exists(behaviour.ParameterSetRecommenderId.Value))
                    {
                        // doesn't exist - throw
                        throw new BadRequestException($"Parameter Set Recommender Id={behaviour.ParameterSetRecommenderId} doesnt exist");
                    }
                    else
                    {
                        // check this recommender will work.
                        var recommender = await parameterSetRecommenderStore.Read(behaviour.ParameterSetRecommenderId.Value);
                        if (recommender.Arguments.Any(_ => _.IsRequired))
                        {
                            throw new BadRequestException($"Hubspot Recommenders do not support required arguments");
                        }
                    }
                }

                else if (behaviour.ItemsRecommenderId.HasValue && behaviour.ItemsRecommenderId != cache.MetricCrmCardBehaviour.ItemsRecommenderId)
                {
                    if (!await itemsRecommenderStore.Exists(behaviour.ItemsRecommenderId.Value))
                    {
                        // doesn't exist - throw
                        throw new BadRequestException($"Items Recommender Id={behaviour.ItemsRecommenderId} doesnt exist");
                    }
                }
            }

            cache.MetricCrmCardBehaviour = behaviour;
            system.SetCache(cache);
            await storageContext.SaveChanges();

            return cache;
        }

        public async Task<IEnumerable<HubspotContactProperty>> LoadContactProperties(long integratedSystemId)
        {
            var system = await integratedSystemStore.Read(integratedSystemId);
            await CheckAndRefreshCredentials(system);
            return await hubspotService.GetContactProperties(system);
        }

        public async Task<Paginated<HubspotContact>> LoadContacts(long integratedSystemId)
        {
            var system = await integratedSystemStore.Read(integratedSystemId);
            await CheckAndRefreshCredentials(system);
            return await hubspotService.GetContacts(system);
        }

        public async Task<IEnumerable<HubspotEvent>> LoadContactEvents(long integratedSystemId, string trackedUserId = null, int? limit = null)
        {
            var system = await integratedSystemStore.Read(integratedSystemId);
            await CheckAndRefreshCredentials(system);
            long? userId = null;
            if (trackedUserId != null)
            {
                if (await customerStore.ExistsFromCommonId(trackedUserId))
                {
                    var customer = await customerStore.ReadFromCommonId(trackedUserId);
                    var map = await GetSystemMap(system, customer);
                    userId = int.Parse(map.UserId);
                }
                else
                {
                    if (int.TryParse(trackedUserId, out var id))
                    {
                        var customer = await customerStore.Read(id);
                        var map = await GetSystemMap(system, customer);
                        userId = int.Parse(map.UserId);
                    }
                    else
                    {
                        throw new BadRequestException($"Unknown Tracked User {trackedUserId}");
                    }
                }
            }

            return await hubspotService.GetContactEvents(system, dateTimeProvider.Now.AddMonths(-3).DateTime, null, userId, limit);
        }

        public async Task<IEnumerable<Customer>> GetAssociatedTrackedUsersFromTicket(string integratedSystemCommonId, string ticketId)
        {
            var system = await integratedSystemStore.ReadFromCommonId(integratedSystemCommonId);
            await CheckAndRefreshCredentials(system);
            var associations = await hubspotService.GetAssociatedContactsFromTicket(system, ticketId);
            var contactIds = associations
                .Where(_ => _.Type == "ticket_to_contact")
                .Select(_ => _.Id);
            var systemMaps = await systemMapStore.Query(1,  // first page
                    _ => _.TrackedUser,
                    _ => contactIds.Contains(_.UserId) && _.IntegratedSystemId == system.Id);

            return systemMaps.Items.Select(_ => _.TrackedUser);
        }

        public Task<HubspotAppCredentials> GetHubspotCredentials()
        {
            if (hubspotCreds.ClientId == null)
            {
                throw new WorkflowException("Hubspot integration not configured correctly.");
            }
            return Task.FromResult(hubspotCreds);
        }

        public async Task<EventLoggingResponse> HandleHubspotRecommendationOutcome(IntegratedSystem integratedSystem,
                                                                          long correlationId,
                                                                          string outcome,
                                                                          string userId,
                                                                          string userEmail,
                                                                          string associatedObjectId,
                                                                          string associatedObjectType)
        {
            var hubspotInfo = integratedSystem.GetCache<HubspotCache>();
            var behaviour = hubspotInfo.WebhookBehaviour ?? new HubspotTrackedUserLinkBehaviour();
            var eventId = System.Guid.NewGuid().ToString();
            var externalId = associatedObjectId; // mot true

            if (!string.IsNullOrEmpty(behaviour.CommonUserIdPropertyName))
            {
                var contact = await hubspotService.GetContact(integratedSystem, associatedObjectId, new List<string> { behaviour.CommonUserIdPropertyName });
                if (contact.Properties.ContainsKey(behaviour.CommonUserIdPropertyName) && !string.IsNullOrEmpty(contact.Properties[behaviour.CommonUserIdPropertyName]))
                {
                    externalId = contact.Properties[behaviour.CommonUserIdPropertyName];
                }
                else
                {
                    throw new BadRequestException($"Couldn't find user, Hubspot Property is: {behaviour.CommonUserIdPropertyName}");
                }
            }
            var customer = await systemMapStore.ReadFromIntegratedSystem(integratedSystem.Id, externalId); // might fix phoria issue
            // use the object ID
            double? outcomeFeedbackValue = null;
            if (outcome == "GOOD")
            {
                outcomeFeedbackValue = 0.8;
            }
            else if (outcome == "BAD")
            {
                outcomeFeedbackValue = -0.4;
            }

            return await eventsWorkflows.Ingest(new List<CustomerEventsWorkflows.CustomerEventInput>
                {
                    new CustomerEventsWorkflows.CustomerEventInput(
                        tenantName: tenantProvider.RequestedTenantName,
                        customer.CustomerId,
                        eventId,
                        timestamp: dateTimeProvider.Now,
                        environmentId: customer.EnvironmentId,
                        recommendationCorrelatorId: correlationId,
                        sourceSystemId: integratedSystem.Id,
                        EventKinds.ConsumeRecommendation,
                        "Direct Feedback",
                    new Dictionary<string, object>
                    {
                        {CustomerEvent.FEEDBACK, outcomeFeedbackValue},
                    })
                });

        }
        public async Task<Customer> HandleWebhookPayload(IntegratedSystem integratedSystem, HubspotWebhookPayload webhookPayload)
        {
            switch (webhookPayload.SubscriptionType)
            {
                case "contact.creation":
                    return await HandleContactCreated(integratedSystem, webhookPayload);
                case "contact.propertyChange":
                    return await HandleContactPropertyChanged(integratedSystem, webhookPayload);
                default:
                    logger.LogWarning("Unhandled Hubspot Webhook Payload");
                    return null;
            }
        }

        private async Task<Customer> HandleContactCreated(IntegratedSystem integratedSystem, HubspotWebhookPayload webhookPayload)
        {
            var hubspotInfo = integratedSystem.GetCache<HubspotCache>();
            var behaviour = hubspotInfo.WebhookBehaviour ?? new HubspotTrackedUserLinkBehaviour();
            var objectId = webhookPayload.ObjectId?.ToString();
            if (string.IsNullOrEmpty(behaviour.CommonUserIdPropertyName))
            {
                var commonUserId = webhookPayload.ObjectId?.ToString();
                // then use object ID
                if (await customerStore.ExistsFromCommonId(commonUserId))
                {
                    // for now, just throw with an error here, but log it.
                    logger.LogError($"Tracked User {commonUserId} already exists");
                    throw new BadRequestException($"Tracked User {commonUserId} already exists");
                }
                else
                {
                    return await CreateNewCustomer(integratedSystem, objectId, commonUserId);
                }
            }
            else if (webhookPayload.PropertyName == behaviour.CommonUserIdPropertyName)
            {
                return await CreateNewCustomer(integratedSystem, objectId, webhookPayload.PropertyValue);
            }
            else
            {
                var contact = await hubspotService.GetContact(integratedSystem, objectId, new List<string> { behaviour.CommonUserIdPropertyName });
                if (contact.Properties.ContainsKey(behaviour.CommonUserIdPropertyName) && !string.IsNullOrEmpty(contact.Properties[behaviour.CommonUserIdPropertyName]))
                {
                    return await CreateNewCustomer(integratedSystem, objectId, contact.Properties[behaviour.CommonUserIdPropertyName]);
                }
                else
                {
                    var telemetryDic = new Dictionary<string, string>
                {
                    { "objectId", webhookPayload.ObjectId?.ToString()},
                    { "portalId", webhookPayload.PortalId?.ToString()},
                    { "propertyName", webhookPayload.PropertyName?.ToString()},
                    { "propertyValue", webhookPayload.PropertyValue?.ToString()},
                };
                    telemetry.TrackEvent("Hubspot.Webhook.HandleContactCreated.Failed", telemetryDic);
                    throw new BadRequestException("Can't create user without a common Id");
                }
            }
        }

        private async Task<Customer> CreateNewCustomer(IntegratedSystem integratedSystem, string objectId, string commonUserId)
        {
            if (integratedSystem.EnvironmentId != null)
            {
                environmentProvider.SetOverride(integratedSystem.EnvironmentId.Value);
            }
            var customer = await customerStore.Create(new Customer(commonUserId, objectId));
            customer.IntegratedSystemMaps.Add(new TrackedUserSystemMap(objectId, integratedSystem, customer));
            await storageContext.SaveChanges();
            return customer;
        }

        private async Task<Customer> HandleContactPropertyChanged(IntegratedSystem integratedSystem, HubspotWebhookPayload webhookPayload)
        {
            var hubspotInfo = integratedSystem.GetCache<HubspotCache>();
            var behaviour = hubspotInfo.WebhookBehaviour ?? new HubspotTrackedUserLinkBehaviour();
            var objectId = webhookPayload.ObjectId?.ToString();

            Customer customer;
            var exists = await systemMapStore.ExistsInIntegratedSystem(integratedSystem.Id, objectId) == true;
            if ((behaviour.CreateUserIfNotExist == true) && !exists)
            {
                // create the customer.
                if (string.IsNullOrEmpty(behaviour.CommonUserIdPropertyName))
                {
                    customer = await CreateNewCustomer(integratedSystem, objectId, objectId);
                }
                else if (webhookPayload.PropertyName == behaviour.CommonUserIdPropertyName)
                {
                    return await CreateNewCustomer(integratedSystem, objectId, webhookPayload.PropertyValue);
                }
                else
                {
                    var contact = await hubspotService.GetContact(integratedSystem, objectId, new List<string> { behaviour.CommonUserIdPropertyName });
                    if (contact.Properties.ContainsKey(behaviour.CommonUserIdPropertyName) && !string.IsNullOrEmpty(contact.Properties[behaviour.CommonUserIdPropertyName]))
                    {
                        return await CreateNewCustomer(integratedSystem, objectId, contact.Properties[behaviour.CommonUserIdPropertyName]);
                    }
                    else
                    {
                        throw new BadRequestException($"Hubspot Contact {objectId} had no value for property {behaviour.CommonUserIdPropertyName}");
                    }
                }
            }
            else if ((behaviour.CreateUserIfNotExist == false) && !exists)
            {
                throw new WorkflowCancelledException($"HS Object ID {objectId} does not exist already, and CreateUserIfNotExist is false");
            }
            else
            {
                customer = await systemMapStore.ReadFromIntegratedSystem(integratedSystem.Id, objectId);
            }

            if (string.IsNullOrEmpty(customer.Name) && webhookPayload.PropertyName?.ToLower()?.Contains("firstname") == true)
            {
                customer.Name = webhookPayload.PropertyValue;
            }

            customer.Properties[$"{behaviour.PropertyPrefix}{webhookPayload.PropertyName}"] = webhookPayload.PropertyValue;

            // now create an event that tracks properties changing
            await eventStore.AddRange(new List<CustomerEvent>
            {
                new CustomerEvent(customer,
                    webhookPayload.EventId?.ToString(),
                    dateTimeProvider.Now,
                    integratedSystem,
                    EventKinds.PropertyUpdate,
                    webhookPayload.SubscriptionType, new Dictionary<string, object>
                    {
                        { $"{behaviour.PropertyPrefix}{webhookPayload.PropertyName}", webhookPayload.PropertyValue }
                    },
                    recommendationCorrelatorId: null)
                });
            await storageContext.SaveChanges();
            return customer;
        }

        public async Task SaveTokenFromCode(long integratedSystemId, string code, string redirectUri)
        {
            var integratedSystem = await integratedSystemStore.Read(integratedSystemId);

            try
            {
                var tokenResponse = await hubspotService.ExchangeCode(hubspotCreds.ClientId, hubspotCreds.ClientSecret, redirectUri, code);
                // get the details of the hubspot system
                var details = await hubspotService.GetAccountDetails(tokenResponse);
                integratedSystem.SetCache(new HubspotCache(details));

                integratedSystem.CommonId = details.PortalId.ToString();
                integratedSystem.TokenResponse = tokenResponse;
                integratedSystem.TokenResponseUpdated = dateTimeProvider.Now;
                integratedSystem.IntegrationStatus = IntegrationStatuses.OK;

                await integratedSystemStore.Update(integratedSystem);
                await storageContext.SaveChanges();
            }
            catch (System.Exception ex)
            {
                throw new WorkflowException("An error occurred when accessing Hubspot", ex);
            }
        }
    }
}


using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Adapters.Hubspot;
using SignalBox.Core.Integrations.Hubspot;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/integratedsystems/{id}/hubspot")]
    public class HubspotController : SignalBoxControllerBase
    {
        private readonly ILogger<HubspotController> logger;
        private readonly HubspotWorkflows hubspotWorkflows;
        private readonly IIntegratedSystemStore store;

        public HubspotController(ILogger<HubspotController> logger,
                                 HubspotWorkflows hubspotWorkflows,
                                 IIntegratedSystemStore store)
        {
            this.logger = logger;
            this.hubspotWorkflows = hubspotWorkflows;
            this.store = store;
        }

        [HttpGet("/api/hubspotappinfo")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<HubspotAppInformation> AppInformation()
        {
            var creds = await hubspotWorkflows.GetHubspotCredentials();
            return new HubspotAppInformation(creds.ClientId, creds.Scope);
        }

        [HttpGet("account")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<HubspotAccountDetails> AccountDetails(long id)
        {
            var cache = await hubspotWorkflows.GetCache(id);
            return cache?.AccountDetails ?? new HubspotAccountDetails();
        }

        [HttpGet("contactproperties")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IEnumerable<HubspotContactProperty>> ContactProperties(long id)
        {
            return await hubspotWorkflows.LoadContactProperties(id);
        }

        [HttpGet("contacts")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<Paginated<HubspotContact>> Contacts(long id)
        {
            return await hubspotWorkflows.LoadContacts(id);
        }

        [HttpGet("contact-events")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IEnumerable<HubspotEvent>> ContactEvents(long id, string trackedUserId = null, int? limit = null)
        {
            limit ??= 100; // default limit to 100
            return await hubspotWorkflows.LoadContactEvents(id, trackedUserId, limit);
        }

        [HttpPost("HubspotCode")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<object> SaveCode(long id, HubspotCode dto)
        {
            await hubspotWorkflows.SaveTokenFromCode(id, dto.Code, dto.RedirectUri);
            return new object();
        }

        [HttpGet("WebhookBehaviour")]
        [HttpGet("LinkBehaviour")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<HubspotTrackedUserLinkBehaviour> GetWebhookBehaviour(string id)
        {
            var system = await store.GetEntity(id);
            var cache = system.GetCache<HubspotCache>();
            if (cache?.WebhookBehaviour == null)
            {
                cache ??= new HubspotCache();
                cache.WebhookBehaviour ??= new HubspotTrackedUserLinkBehaviour();
                system.SetCache(cache);
                await store.Context.SaveChanges();
                logger.LogInformation("Updated cache since it was null");
            }

            return cache.WebhookBehaviour;
        }

        [HttpPost("WebhookBehaviour")]
        [HttpPost("LinkBehaviour")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<HubspotTrackedUserLinkBehaviour> SetWebhookBehaviour(string id, HubspotTrackedUserLinkBehaviour dto)
        {
            var system = await store.GetEntity(id);
            var cache = system.GetCache<HubspotCache>();
            cache.WebhookBehaviour = dto;
            system.SetCache(cache);
            await store.Context.SaveChanges();
            return cache.WebhookBehaviour;
        }

        [HttpGet("CrmCardBehaviour")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<FeatureCrmCardBehaviour> GetCrmCardBehaviour(string id)
        {
            var system = await store.GetEntity(id);
            var cache = system.GetCache<HubspotCache>();
            if (cache?.FeatureCrmCardBehaviour == null)
            {
                cache ??= new HubspotCache();
                cache.FeatureCrmCardBehaviour ??= new FeatureCrmCardBehaviour();
                system.SetCache(cache);
                await store.Context.SaveChanges();
                logger.LogInformation("Updated cache since it was null");
            }

            return cache.FeatureCrmCardBehaviour;
        }

        [HttpPost("CrmCardBehaviour")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<FeatureCrmCardBehaviour> SetCrmCardBehaviour(string id, FeatureCrmCardBehaviour dto)
        {
            var system = await store.GetEntity(id);
            var cache = await hubspotWorkflows.UpdateCrmCardBehaviour(system, dto);
            return cache.FeatureCrmCardBehaviour;
        }

        [HttpGet("ConnectedContactProperties")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<HubspotPropertyCollection> GetConnectedContactProperties(string id)
        {
            var system = await store.GetEntity(id);
            var cache = system.GetCache<HubspotCache>();
            if (cache?.ConnectedContactProperties == null)
            {
                cache ??= new HubspotCache();
                cache.ConnectedContactProperties ??= new HubspotPropertyCollection();
                system.SetCache(cache);
                await store.Context.SaveChanges();
                logger.LogInformation("Updated cache since it was null");
            }

            return cache.ConnectedContactProperties;
        }

        [HttpPost("ConnectedContactProperties")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<HubspotPropertyCollection> SetConnectedContactProperties(string id, HubspotPropertyCollection dto)
        {
            var system = await store.GetEntity(id);
            // validate properties
            var availableproperties = await hubspotWorkflows.LoadContactProperties(system.Id);
            var availablepropertyNames = availableproperties.Select(_ => _.Name);
            foreach (var n in dto.PropertyNames)
            {
                if (!availablepropertyNames.Any(_ => _ == n))
                {
                    throw new BadRequestException($"{n} is not a valid hubspot contact property name");
                }
            }

            var cache = system.GetCache<HubspotCache>();
            cache.ConnectedContactProperties = dto;
            system.SetCache(cache);
            await store.Context.SaveChanges();
            return cache.ConnectedContactProperties;
        }
    }
}
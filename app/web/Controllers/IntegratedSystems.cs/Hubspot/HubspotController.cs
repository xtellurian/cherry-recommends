
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Adapters.Hubspot;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/hubspotintegratedsystems")]
    public class HubspotController : SignalBoxControllerBase
    {
        private readonly ILogger<HubspotController> logger;
        private readonly HubspotWorkflows hubspotWorkflows;

        public HubspotController(ILogger<HubspotController> logger, HubspotWorkflows hubspotWorkflows)
        {
            this.logger = logger;
            this.hubspotWorkflows = hubspotWorkflows;
        }

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<HubspotAppInformation> AppInformation()
        {
            var creds = await hubspotWorkflows.GetHubspotCredentials();
            return new HubspotAppInformation(creds.ClientId, creds.Scope);
        }

        [HttpGet("{id}/account")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<HubspotAccountDetails> AccountDetails(long id)
        {
            var cache = await hubspotWorkflows.GetCache(id);
            return cache?.AccountDetails ?? new HubspotAccountDetails();
        }

        [HttpGet("{id}/contactproperties")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<object> ContactProperties(long id)
        {
            return await hubspotWorkflows.LoadContactProperties(id);
        }

        [HttpGet("{id}/contacts")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<object> Contacts(long id)
        {
            return await hubspotWorkflows.LoadContacts(id);
        }

        [HttpPost("{id}/HubspotCode")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<object> SaveCode(long id, HubspotCode dto)
        {
            await hubspotWorkflows.SaveTokenFromCode(id, dto.Code, dto.RedirectUri);
            return new object();
        }
    }
}
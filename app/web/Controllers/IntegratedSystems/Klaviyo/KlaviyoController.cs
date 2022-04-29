using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Adapters.Klaviyo;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/integratedsystems/{id}/klaviyo")]
    public class KlaviyoController : SignalBoxControllerBase
    {
        private readonly ILogger<KlaviyoController> logger;
        private readonly IKlaviyoSystemWorkflow workflow;
        private readonly IIntegratedSystemStore integratedSystemStore;

        public KlaviyoController(ILogger<KlaviyoController> logger,
                                 IKlaviyoSystemWorkflow workflow,
                                 IIntegratedSystemStore integratedSystemStore)
        {
            this.logger = logger;
            this.workflow = workflow;
            this.integratedSystemStore = integratedSystemStore;
        }

        /// <summary> Sets Klaviyo API Keys.</summary>
        [HttpPost("ApiKeys")]
        public async Task<IntegratedSystem> SetApiKeys(long id, [FromBody] SetKlaviyoApiKeysDto dto)
        {
            var system = await integratedSystemStore.Read(id);
            system = await workflow.SetApiKeys(system, dto.PublicKey, dto.PrivateKey);

            return system;
        }

        /// <summary> Gets Klaviyo Lists.</summary>
        [HttpGet("Lists")]
        public async Task<IEnumerable<KlaviyoList>> GetLists(long id)
        {
            var system = await integratedSystemStore.Read(id);
            var klaviyoLists = await workflow.GetLists(system);
            return klaviyoLists;
        }
    }
}
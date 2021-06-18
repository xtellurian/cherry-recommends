using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class IntegratedSystemsController : EntityControllerBase<IntegratedSystem>
    {
        private readonly IntegratedSystemWorkflows workflows;
        private readonly IWebhookReceiverStore webhookReceiverStore;

        public IntegratedSystemsController(IntegratedSystemWorkflows workflows,
                                           IIntegratedSystemStore store,
                                           IWebhookReceiverStore webhookReceiverStore) : base(store)
        {
            this.workflows = workflows;
            this.webhookReceiverStore = webhookReceiverStore;
        }

        /// <summary>Creates a new Integrated System.</summary>
        [HttpPost]
        public async Task<IntegratedSystem> CreateIntegratedSystem(CreateIntegratedSystemDto dto)
        {
            return await workflows.CreateIntegratedSystem(dto.Name, dto.SystemType);
        }

        /// <summary>Creates a new webhook receiver on an existing integrated system.</summary>
        [HttpPost("{id}/webhookreceivers")]
        public async Task<WebhookReceiver> CreateWebhookReceiver(long id, bool? useSharedSecret = null)
        {
            return await workflows.AddWebhookReceiver(id, useSharedSecret);
        }

        /// <summary>Returned a paginated list of webhook receivers for the given integrated system.</summary>
        [HttpGet("{id}/webhookreceivers")]
        public async Task<IEnumerable<WebhookReceiver>> QueryWebhookReceivers(long id)
        {
            return await webhookReceiverStore.GetReceiversForIntegratedSystem(id);
        }
    }
}
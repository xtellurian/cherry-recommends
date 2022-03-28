using System;
using System.Collections.Generic;
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
    public class IntegratedSystemsController : CommonEntityControllerBase<IntegratedSystem>
    {
        private readonly IntegratedSystemWorkflows workflows;
        private readonly IWebhookReceiverStore webhookReceiverStore;
        private readonly IShopifyAdminWorkflow shopifyAdminWorkflows;

        public IntegratedSystemsController(IntegratedSystemWorkflows workflows,
                                           IIntegratedSystemStore store,
                                           IWebhookReceiverStore webhookReceiverStore,
                                           IShopifyAdminWorkflow shopifyAdminWorkflows) : base(store)
        {
            this.workflows = workflows;
            this.webhookReceiverStore = webhookReceiverStore;
            this.shopifyAdminWorkflows = shopifyAdminWorkflows;
        }

        /// <summary>Creates a new Integrated System.</summary>
        [HttpPost]
        public async Task<IntegratedSystem> CreateIntegratedSystem(CreateIntegratedSystemDto dto)
        {
            return await workflows.CreateIntegratedSystem(dto.Name, dto.SystemType);
        }

        /// <summary>Creates a new Segment webhook receiver on an existing integrated system.</summary>
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

        /// <summary>Deletes the resource with this Id.</summary>
        [HttpDelete("{id}")]
        public override async Task<DeleteResponse> DeleteResource(long id)
        {
            var entity = await store.Read(id);
            var (canDelete, message) = await CanDelete(entity);
            if (canDelete)
            {
                if (entity.SystemType == IntegratedSystemTypes.Shopify && entity.IntegrationStatus == IntegrationStatuses.OK)
                {
                    // Uninstall Shopify app on entity delete 
                    // Uninstallation should happen here and not on the Shopify admin page
                    await shopifyAdminWorkflows.UninstallApp(entity, errorOnUninstall: true);
                }
                var result = await store.Remove(id);
                await store.Context.SaveChanges();
                return new DeleteResponse(id, Request.Path.Value, result);
            }
            else
            {
                throw new BadRequestException($"Delete error: {message}", message);
            }
        }

        protected override Task<(bool, string)> CanDelete(IntegratedSystem entity)
        {
            return Task.FromResult((true, ""));
        }

        /// <summary>Returned a paginated list of items for this resource.</summary>
        [HttpGet]
        public override async Task<Paginated<IntegratedSystem>> Query([FromQuery] PaginateRequest p, [FromQuery] SearchEntities q)
        {
            if (Enum.TryParse<IntegratedSystemTypes>(q.Scope, ignoreCase: true, out var systemType))
            {
                return await store.Query(new EntityStoreQueryOptions<IntegratedSystem>(p.Page, _ => _.SystemType == systemType));
            }
            else
            {
                return await base.Query(p, q);
            }
        }
    }
}
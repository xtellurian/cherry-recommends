using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class ChannelsController : EntityControllerBase<ChannelBase>
    {
        private readonly ILogger<ChannelsController> _logger;
        private readonly ChannelWorkflow workflow;
        private readonly IChannelStore channelStore;
        private readonly IWebhookChannelStore webhookChannelStore;
        private readonly IIntegratedSystemStore integratedSystemStore;

        public ChannelsController(ILogger<ChannelsController> logger,
                                    IChannelStore channelStore,
                                    IWebhookChannelStore webhookChannelStore,
                                    IIntegratedSystemStore integratedSystemStore,
                                    ChannelWorkflow workflow) : base(channelStore)
        {
            _logger = logger;
            this.workflow = workflow;
            this.channelStore = channelStore;
            this.webhookChannelStore = webhookChannelStore;
            this.integratedSystemStore = integratedSystemStore;
        }

        /// <summary>Creates a new Channel.</summary>
        [HttpPost]
        public async Task<ChannelBase> CreateChannel([FromBody] CreateChannelDto dto)
        {
            var integratedSystem = await integratedSystemStore.Read(dto.IntegratedSystemId);
            return await workflow.CreateChannel(dto.Name, dto.ChannelType, integratedSystem);
        }

        /// <summary>Returns the resource with this Id.</summary>
        [HttpGet("{id}")]
        public override async Task<ChannelBase> GetResource(long id)
        {
            var channel = await base.GetResource(id);
            await channelStore.Load(channel, _ => _.LinkedIntegratedSystem);
            return channel;
        }

        /// <summary> Updates endpoint of a channel.</summary>
        [HttpPost("{id}/endpoint")]
        public async Task<ChannelBase> UpdateEndpoint(long id, [FromBody] string endpoint)
        {
            var channel = await base.GetResource(id);
            channel = await workflow.UpdateChannelEndpoint(channel, endpoint);
            return channel;
        }
    }
}

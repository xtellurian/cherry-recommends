using System.Collections.Generic;
using System.Linq;
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
    public class TrackedUsersController : CommonEntityControllerBase<TrackedUser>
    {
        private readonly ILogger<TrackedUsersController> _logger;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly TrackedUserWorkflows workflows;
        private readonly ITrackedUserEventStore eventStore;

        public TrackedUsersController(ILogger<TrackedUsersController> logger,
                                      IDateTimeProvider dateTimeProvider,
                                      TrackedUserWorkflows workflows,
                                      ITrackedUserStore store,
                                      ITrackedUserEventStore eventStore) : base(store)
        {
            _logger = logger;
            this.dateTimeProvider = dateTimeProvider;
            this.workflows = workflows;
            this.eventStore = eventStore;
        }

        public override async Task<TrackedUser> GetResource(string id, bool? useInternalId = null)
        {
            if ((useInternalId == null || useInternalId == true) && int.TryParse(id, out var internalId))
            {
                return await store.Read(internalId, _ => _.IntegratedSystemMaps);
            }
            else if (useInternalId == true)
            {
                throw new BadRequestException("Internal Ids must be integers");
            }
            else
            {
                return await store.ReadFromCommonId(id);
            }
        }

        /// <summary>Returns a list of events for a given user.</summary>
        [HttpGet("{id}/events")]
        public async Task<Paginated<TrackedUserEvent>> GetEvents([FromQuery] PaginateRequest p, string id, bool? useInternalId = null)
        {
            var trackedUser = await store.GetEntity(id, useInternalId);
            return await eventStore.ReadEventsForUser(p.Page, trackedUser);
        }

        /// <summary> Updates the properties of a customer.</summary>
        [HttpPost("{id}/properties")]
        public override async Task<DynamicPropertyDictionary> SetProperties(string id, [FromBody] DynamicPropertyDictionary properties, bool? useInternalId = null)
        {
            var trackedUser = await base.GetResource(id);
            trackedUser = await workflows.MergeUpdateProperties(trackedUser, properties, null, saveOnComplete: true);
            return trackedUser.Properties;
        }

        /// <summary>Creates a new tracked user.</summary>
        [HttpPost]
        public async Task<object> CreateOrUpdate([FromBody] CreateOrUpdateTrackedUserDto dto)
        {
            return await workflows.CreateOrUpdateTrackedUser(dto.CommonUserId, dto.Name, dto.Properties,
                                                     dto.IntegratedSystemReference?.IntegratedSystemId,
                                                     dto.IntegratedSystemReference?.UserId);
        }

        /// <summary>Creates or updates a set of users with properties.</summary>
        [HttpPut]
        public async Task<object> CreateBatch([FromBody] BatchCreateOrUpdateUsersDto dto)
        {
            await workflows.CreateOrUpdateMultipleTrackedUsers(
                dto.Users.Select(u => new TrackedUserWorkflows.CreateOrUpdateTrackedUserModel(u.CommonUserId,
                                                                                         u.Name,
                                                                                         u.Properties,
                                                                                         u.IntegratedSystemReference?.IntegratedSystemId,
                                                                                         u.IntegratedSystemReference?.UserId)));
            return new object();
        }

        protected override Task<(bool, string)> CanDelete(TrackedUser entity)
        {
            return Task.FromResult((true, ""));
        }
    }
}

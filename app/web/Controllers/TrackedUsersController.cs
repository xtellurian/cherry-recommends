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
    public class TrackedUsersController : ControllerBase
    {
        private readonly ILogger<TrackedUsersController> _logger;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly TrackedUserWorkflows workflows;
        private readonly ITrackedUserStore userStore;
        private readonly ITrackedUserEventStore eventStore;

        public TrackedUsersController(ILogger<TrackedUsersController> logger,
                                      IDateTimeProvider dateTimeProvider,
                                      TrackedUserWorkflows workflows,
                                      ITrackedUserStore userStore,
                                      ITrackedUserEventStore eventStore)
        {
            _logger = logger;
            this.dateTimeProvider = dateTimeProvider;
            this.workflows = workflows;
            this.userStore = userStore;
            this.eventStore = eventStore;
        }

        [HttpGet("{id}")]
        public async Task<TrackedUser> Get(long id)
        {
            return await userStore.Read(id);
        }

        [HttpGet]
        public async Task<IEnumerable<TrackedUser>> GetList()
        {
            return await userStore.List();
        }

        [HttpGet("events")]
        public async Task<IEnumerable<TrackedUserEvent>> GetEvents(string trackedUserExternalId)
        {
            return await eventStore.ReadEventsForUser(trackedUserExternalId);
        }

        [HttpPost("query")]
        public async Task<IList<TrackedUser>> Create(TrackedUserQueryDto dto)
        {
            var users = new List<TrackedUser>();
            foreach (var id in dto.ExternalIds)
            {
                var user = await userStore.ReadFromExternalId(id);
                users.Add(user);
            }

            return users;
        }

        [HttpPost]
        public async Task<object> Create([FromBody] CreateTrackedUserDto dto)
        {
            return await workflows.CreateTrackedUser(dto.ExternalId, dto.Name);
        }

        [HttpPost("batch")]
        public async Task<object> CreateBatch([FromBody] CreateTrackedUsersDto dto)
        {
            await workflows.CreateMultipleTrackedUsers(
                dto.Users.Select(u => (u.ExternalId, u.Name)),
                dto.Events.Select(e => (e.TrackedUserExternalId, e.Key, e.LogicalValue, e.NumericValue)));

            return new object();
        }
    }
}

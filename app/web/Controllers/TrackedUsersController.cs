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
        public async Task<IEnumerable<TrackedUserEvent>> GetEvents(string commonUserId)
        {
            return await eventStore.ReadEventsForUser(commonUserId);
        }

        [HttpPost("query")]
        public async Task<IList<TrackedUser>> Create(TrackedUserQueryDto dto)
        {
            var users = new List<TrackedUser>();
            foreach (var id in dto.CommonUserIds)
            {
                var user = await userStore.ReadFromCommonUserId(id);
                users.Add(user);
            }

            return users;
        }

        [HttpPost]
        public async Task<object> Create([FromBody] CreateOrUpdateTrackedUserDto dto)
        {
            return await workflows.CreateTrackedUser(dto.CommonUserId, dto.Name, dto.Properties);
        }

        [HttpPut("{id}/properties")]
        public async Task<object> Create(string id, [FromBody] Dictionary<string, object> dto)
        {
            return await workflows.MergeTrackedUserProperties(id, dto);
        }

        [HttpPut]
        public async Task<object> CreateBatch([FromBody] BatchCreateOrUpdateUsersDto dto)
        {
            await workflows.CreateOrUpdateMultipleTrackedUsers(
                dto.Users.Select(u => (u.CommonUserId, u.Name, u.Properties)));
            return new object();
        }
    }
}

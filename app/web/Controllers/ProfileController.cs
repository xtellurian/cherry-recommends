using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Internal;
using SignalBox.Core.Workflows;
using SignalBox.Infrastructure;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly ILogger<ProfileController> logger;
        private readonly IAuth0Service auth0Service;

        public ProfileController(ILogger<ProfileController> logger, IAuth0Service auth0Service)
        {
            this.logger = logger;
            this.auth0Service = auth0Service;
        }

        /// <summary>Sets metadata for the current logged in user.</summary>
        [HttpPost("metadata")]
        public async Task<UserMetadata> SetMetadata([FromBody] UserMetadata metadata)
        {
            var user = await auth0Service.SetMetadata(User.Auth0Id(), metadata);
            logger.LogInformation($"Updated metadata for user {user.UserId}");
            return metadata;
        }

        /// <summary>Gets metadata for the current logged in user.</summary>
        [HttpGet("metadata")]
        public async Task<UserMetadata> GetMetadata()
        {
            return await auth0Service.GetMetadata(User.Auth0Id());
        }
    }
}

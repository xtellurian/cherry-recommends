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
    public class ApiKeysController : SignalBoxControllerBase
    {
        private readonly ApiKeyWorkflows workflows;
        private readonly IHashedApiKeyStore store;

        public ApiKeysController(ApiKeyWorkflows workflows, IHashedApiKeyStore store)
        {
            this.workflows = workflows;
            this.store = store;
        }

        /// <summary>Exchange an API key for a token.</summary>
        [HttpPost("exchange")]
        [AllowAnonymous]
        public async Task<ApiKeyExchangeResponseDto> ExchangeApiKeyForToken(ApiKeyExchangeRequestDto dto)
        {
            var token = await workflows.ExchangeApiKeyForToken(dto.ApiKey);
            return new ApiKeyExchangeResponseDto(token.AccessToken);
        }

        /// <summary>Creates an API key.</summary>
        [HttpPost("create")]
        [HttpPost]
        public async Task<CreateApiKeyResponseDto> CreateApiKey(CreateApiKeyDto dto)
        {
            var key = await workflows.GenerateAndStoreApiKey(dto.Name, dto.ApiKeyType, User, dto.Scope);
            return new CreateApiKeyResponseDto(dto.Name, key);
        }

        /// <summary>Creates an API key.</summary>
        [HttpDelete("{id}")]
        public async Task<DeleteResponse> DeleteApiKey(long id)
        {
            var result = await workflows.DeleteApiKey(id);
            return new DeleteResponse(id, Request.Path.Value, result);
        }

        /// <summary>Lists the names of all API Keys.</summary>
        [HttpGet]
        public async Task<Paginated<ApiKeyDto>> ListApiKeyNames([FromQuery] PaginateRequest p)
        {
            var result = await store.Query(p);
            var x = new Paginated<ApiKeyDto>(
                result.Items.Select(_ => new ApiKeyDto(_)),
                result.Pagination.PageCount,
                result.Pagination.TotalItemCount,
                result.Pagination.PageNumber);
            return x;
        }
    }
}
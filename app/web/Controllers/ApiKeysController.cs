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
    public class ApiKeysController : ControllerBase
    {
        private readonly ApiKeyWorkflows workflows;
        private readonly IHashedApiKeyStore store;

        public ApiKeysController(ApiKeyWorkflows workflows, IHashedApiKeyStore store)
        {
            this.workflows = workflows;
            this.store = store;
        }

        [HttpPost("exchange")]
        [AllowAnonymous]
        public async Task<ApiKeyExchangeResponseDto> ExchangeApiKeyForToken(ApiKeyExchangeRequestDto dto)
        {
            var token = await workflows.ExchangeApiKeyForToken(dto.ApiKey);
            return new ApiKeyExchangeResponseDto(token);
        }

        [HttpPost("create")]
        public async Task<CreateApiKeyResponseDto> CreateApiKey(CreateApiKeyDto dto)
        {
            var key = await workflows.GenerateAndStoreApiKey(dto.Name);
            return new CreateApiKeyResponseDto(dto.Name, key);
        }

        [HttpGet]
        public async Task<IEnumerable<string>> ListApiKeyNames()
        {
            var allKeys = await store.List();
            return allKeys.Select(_ => _.Name);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.OAuth;

namespace SignalBox.Core.Workflows
{
    public class ApiKeyWorkflows : IWorkflow
    {
        private readonly IStorageContext storageContext;
        private readonly ITenantProvider tenantProvider;
        private readonly IApiTokenFactory tokenFactory;
        private readonly IHashedApiKeyStore keyStore;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ILogger<ApiKeyWorkflows> logger;
        private readonly IM2MTokenCache tokenCache;
        private readonly IHasher hasher;

        public ApiKeyWorkflows(IStorageContext storageContext,
                               ITenantProvider tenantProvider,
                               IApiTokenFactory tokenFactory,
                               IHashedApiKeyStore keyStore,
                               IDateTimeProvider dateTimeProvider,
                               ILogger<ApiKeyWorkflows> logger,
                               IM2MTokenCache tokenCache,
                               IHasher hasher)
        {
            this.storageContext = storageContext;
            this.tenantProvider = tenantProvider;
            this.tokenFactory = tokenFactory;
            this.keyStore = keyStore;
            this.dateTimeProvider = dateTimeProvider;
            this.logger = logger;
            this.tokenCache = tokenCache;
            this.hasher = hasher;
        }

        public async Task<bool> IsValidApiKey(string apiKey, ApiKeyTypes? apiKeyType = null)
        {
            // check the key
            var hashedKey = hasher.Hash(apiKey);
            if (await keyStore.HashExists(hashedKey))
            {
                if (apiKeyType != null && apiKeyType.HasValue)
                {
                    // compare the key types
                    var storedKey = await keyStore.ReadFromHash(hashedKey);
                    return apiKeyType.Value.HasFlag(storedKey.ApiKeyType);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<HashedApiKey> LoadRecord(string apiKey)
        {
            var hashedKey = hasher.Hash(apiKey);
            if (await keyStore.HashExists(hashedKey))
            {
                return await keyStore.ReadFromHash(hashedKey);
            }
            else
            {
                throw new BadRequestException("Api Key not found");
            }
        }

        public async Task<TokenResponse> ExchangeApiKeyForToken(string apiKey)
        {
            // check the key
            if (await IsValidApiKey(apiKey))
            {
                var hashedKey = hasher.Hash(apiKey);
                // update the last exchanged time for the token.
                var key = await keyStore.ReadFromHash(hashedKey);
                key.LastExchanged = dateTimeProvider.Now;
                key.TotalExchanges++;
                await storageContext.SaveChanges();
                var token = await tokenCache.Get(apiKey, () => tokenFactory.GetM2MToken(key.Scope));
                // var token = await tokenFactory.GetM2MToken(key.Scope);
                return token;

            }
            else
            {
                throw new SecurityException("Api Key was invalid");
            }

        }

        public async Task<bool> DeleteApiKey(long id)
        {
            var result = await keyStore.Remove(id);
            await storageContext.SaveChanges();
            return result;
        }

        public async Task<string> GenerateAndStoreApiKey(string name, string type, ClaimsPrincipal principal, string scope = null)
        {
            // scope: "openid profile email webAPI write:metrics"
            if (System.Enum.TryParse<ApiKeyTypes>(type, out var t))
            {
                var scopeClaim = principal.Claims.FirstOrDefault(_ => _.Type == "scope");
                if (string.IsNullOrEmpty(scope))
                {
                    scope = scopeClaim.Value;
                }
                else
                {
                    foreach (var s in scope.Split(' '))
                    {
                        var userScope = scopeClaim.Value;
                        if (!userScope.Contains(s))
                        {
                            throw new SecurityException($"Cannot assign scope {s} as user does not have scope.");
                        }
                    }
                }

                // remove the OIdC scopes
                var oidcScopes = new List<string> { "openid", "profile", "email" };
                foreach (var o in oidcScopes)
                {
                    if (scope.Contains(o))
                    {
                        scope = scope.Replace(o, "");
                    }
                }

                scope = scope.Trim();

                var tenant = tenantProvider.Current();
                if (tenant != null)
                {
                    logger.LogInformation("Adding tenant scope to API Key");
                    // remove any other tenant that might be sneakily in the user's token
                    var scopeList = scope.Split(' ').Where(s => !s.Contains("tenant:")).Append(tenant.AccessScope());
                    scope = string.Join(' ', scopeList);
                }

                // generate a new key
                var apiKey = System.Guid.NewGuid().ToBase64Encoded();
                var hashedKey = hasher.Hash(apiKey);
                var storedKey = await keyStore.Create(new HashedApiKey(name, t, hasher.DefaultAlgorithm, hashedKey, scope));
                await storageContext.SaveChanges();
                return apiKey;
            }
            else
            {
                throw new BadRequestException($"Unable to parse key type {type}");
            }
        }
    }
}
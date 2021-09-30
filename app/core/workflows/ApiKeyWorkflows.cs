using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SignalBox.Core.Workflows
{
    public class ApiKeyWorkflows : IWorkflow
    {
        private readonly IStorageContext storageContext;
        private readonly IApiTokenFactory tokenFactory;
        private readonly IHashedApiKeyStore keyStore;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ILogger<ApiKeyWorkflows> logger;
        private readonly IHasher hasher;

        public ApiKeyWorkflows(IStorageContext storageContext,
                               IApiTokenFactory tokenFactory,
                               IHashedApiKeyStore keyStore,
                               IDateTimeProvider dateTimeProvider,
                               ILogger<ApiKeyWorkflows> logger,
                               IHasher hasher)
        {
            this.storageContext = storageContext;
            this.tokenFactory = tokenFactory;
            this.keyStore = keyStore;
            this.dateTimeProvider = dateTimeProvider;
            this.logger = logger;
            this.hasher = hasher;
        }

        public async Task<string> ExchangeApiKeyForToken(string apiKey)
        {
            // check the key
            var hashedKey = hasher.Hash(apiKey);
            if (await keyStore.HashExists(hashedKey))
            {
                // update the last exchanged time for the token.
                var key = await keyStore.ReadFromHash(hashedKey);
                key.LastExchanged = dateTimeProvider.Now;
                key.TotalExchanges++;
                await storageContext.SaveChanges();
                return await tokenFactory.GetM2MToken(key.Scope);
            }
            else
            {
                throw new SecurityException("Api Key was invalid");
            }

        }

        public async Task<string> GenerateAndStoreApiKey(string name, ClaimsPrincipal principal, string scope = null)
        {
            // scope: "openid profile email webAPI write:features"
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

            // generate a new key
            var apiKey = System.Guid.NewGuid().ToBase64Encoded();
            var hashedKey = hasher.Hash(apiKey);
            var storedKey = await keyStore.Create(new HashedApiKey(name, hasher.DefaultAlgorithm, hashedKey, scope));
            await storageContext.SaveChanges();
            return apiKey;
        }


    }
}
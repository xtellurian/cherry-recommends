using System.Threading.Tasks;

namespace SignalBox.Core.Workflows
{
    public class ApiKeyWorkflows : IWorkflow
    {
        private readonly IStorageContext storageContext;
        private readonly IApiTokenFactory tokenFactory;
        private readonly IHashedApiKeyStore keyStore;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IHasher hasher;

        public ApiKeyWorkflows(IStorageContext storageContext,
                               IApiTokenFactory tokenFactory,
                               IHashedApiKeyStore keyStore,
                               IDateTimeProvider dateTimeProvider,
                               IHasher hasher)
        {
            this.storageContext = storageContext;
            this.tokenFactory = tokenFactory;
            this.keyStore = keyStore;
            this.dateTimeProvider = dateTimeProvider;
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
                return await tokenFactory.GetToken();
            }
            else
            {
                throw new StorageException("Api Key was invalid");
            }

        }

        public async Task<string> GenerateAndStoreApiKey(string name)
        {
            // generate a new key
            var apiKey = System.Guid.NewGuid().ToBase64Encoded();
            var hashedKey = hasher.Hash(apiKey);
            var storedKey = await keyStore.Create(new HashedApiKey(name, hasher.DefaultAlgorithm, hashedKey));
            await storageContext.SaveChanges();
            return apiKey;
        }

        
    }
}
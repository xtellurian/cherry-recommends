using System.Threading.Tasks;

namespace SignalBox.Core.Workflows
{
    public class ApiKeyWorkflows : IWorkflow
    {
        private readonly IStorageContext storageContext;
        private readonly IApiTokenFactory tokenFactory;
        private readonly IHashedApiKeyStore keyStore;
        private readonly IHasher hasher;

        public ApiKeyWorkflows(IStorageContext storageContext, IApiTokenFactory tokenFactory, IHashedApiKeyStore keyStore, IHasher hasher)
        {
            this.storageContext = storageContext;
            this.tokenFactory = tokenFactory;
            this.keyStore = keyStore;
            this.hasher = hasher;
        }

        public async Task<string> ExchangeApiKeyForToken(string apiKey)
        {
            // check the key
            var hashedKey = hasher.Hash(apiKey);
            if (await keyStore.HashExists(hashedKey))
            {
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
            var apiKey = Base64Encode(System.Guid.NewGuid().ToString());
            var hashedKey = hasher.Hash(apiKey);
            var storedKey = await keyStore.Create(new HashedApiKey(name, hasher.AlgorithmName, hashedKey));
            await storageContext.SaveChanges();
            return apiKey;
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
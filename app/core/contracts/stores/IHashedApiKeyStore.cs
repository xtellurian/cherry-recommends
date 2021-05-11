using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IHashedApiKeyStore : IEntityStore<HashedApiKey>
    {
        Task<bool> HashExists(string hashedKey);
    }
}
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFHashedAPIKeyStore : EFEntityStoreBase<HashedApiKey>, IHashedApiKeyStore
    {
        public EFHashedAPIKeyStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, (c) => c.ApiKeys)
        { }

        public async Task<bool> HashExists(string hashedKey)
        {
            return await Set.AnyAsync(_ => _.HashedKey == hashedKey);
        }

        public async Task<HashedApiKey> ReadFromHash(string hashedKey)
        {
            return await Set.FirstAsync(_ => _.HashedKey == hashedKey);
        }
    }
}
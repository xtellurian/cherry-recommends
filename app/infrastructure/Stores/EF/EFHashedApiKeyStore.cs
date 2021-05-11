using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure
{
    public class EFHashedAPIKeyStore : EFEntityStoreBase<HashedApiKey>, IHashedApiKeyStore
    {
        public EFHashedAPIKeyStore(SignalBoxDbContext context)
        : base(context, (c) => c.ApiKeys)
        {
        }

        public async Task<bool> HashExists(string hashedKey)
        {
            return await Set.AnyAsync(_ => _.HashedKey == hashedKey);
        }
    }
}
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFIntegratedSystemCredentialStore : EFEntityStoreBase<IntegratedSystemCredential>, IIntegratedSystemCredentialStore
    {
        public EFIntegratedSystemCredentialStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, _ => _.IntegratedSystemCredentials)
        { }

        public async Task<EntityResult<IntegratedSystemCredential>> ReadFromKey(string key)
        {
            var entity = await QuerySet.FirstOrDefaultAsync(_ => _.Key == key);

            return new EntityResult<IntegratedSystemCredential>(entity);
        }
    }
}
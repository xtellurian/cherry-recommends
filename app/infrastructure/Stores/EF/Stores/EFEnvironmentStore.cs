using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFEnvironmentStore : EFEntityStoreBase<Core.Environment>, IEnvironmentStore
    {
        public EFEnvironmentStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, c => c.Environments)
        { }
    }
}
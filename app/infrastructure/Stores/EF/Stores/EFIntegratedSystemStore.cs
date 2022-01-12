using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFIntegratedSystemStore : EFCommonEntityStoreBase<IntegratedSystem>, IIntegratedSystemStore
    {
        protected override bool IsEnvironmentScoped => true;
        public EFIntegratedSystemStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentProvider environmentService)
        : base(contextProvider, environmentService, c => c.IntegratedSystems)
        { }

    }
}
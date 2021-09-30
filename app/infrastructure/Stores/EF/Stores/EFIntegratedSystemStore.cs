using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFIntegratedSystemStore : EFCommonEntityStoreBase<IntegratedSystem>, IIntegratedSystemStore
    {
        public EFIntegratedSystemStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentService environmentService)
        : base(contextProvider, environmentService, c => c.IntegratedSystems)
        { }

    }
}
using SignalBox.Core;
using SignalBox.Core.Integrations.Custom;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFCustomIntegratedSystemStore : EFCommonEntityStoreBase<CustomIntegratedSystem>, ICustomIntegratedSystemStore
    {
        public EFCustomIntegratedSystemStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentService environmentService)
        : base(contextProvider, environmentService, c => c.CustomIntegratedSystems)
        { }

    }
}
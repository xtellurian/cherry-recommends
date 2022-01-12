using SignalBox.Core;
using SignalBox.Core.Integrations.Custom;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFCustomIntegratedSystemStore : EFCommonEntityStoreBase<CustomIntegratedSystem>, ICustomIntegratedSystemStore
    {
        protected override bool IsEnvironmentScoped => true;
        public EFCustomIntegratedSystemStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentProvider environmentService)
        : base(contextProvider, environmentService, c => c.CustomIntegratedSystems)
        { }

    }
}
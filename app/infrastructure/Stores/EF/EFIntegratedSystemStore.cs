using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFIntegratedSystemStore : EFCommonEntityStoreBase<IntegratedSystem>, IIntegratedSystemStore
    {
        public EFIntegratedSystemStore(SignalBoxDbContext context, IEnvironmentService environmentService)
        : base(context, environmentService, c => c.IntegratedSystems)
        {
        }
    }
}
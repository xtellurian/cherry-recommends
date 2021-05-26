using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFIntegratedSystemStore : EFEntityStoreBase<IntegratedSystem>, IIntegratedSystemStore
    {
        public EFIntegratedSystemStore(SignalBoxDbContext context) : base(context, c => c.IntegratedSystems)
        {
        }
    }
}
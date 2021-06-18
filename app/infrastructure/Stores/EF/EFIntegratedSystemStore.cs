using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFIntegratedSystemStore : EFCommonEntityStoreBase<IntegratedSystem>, IIntegratedSystemStore
    {
        public EFIntegratedSystemStore(SignalBoxDbContext context) : base(context, c => c.IntegratedSystems)
        {
        }
    }
}
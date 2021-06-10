using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFTouchpointStore : EFCommonEntityStoreBase<Touchpoint>, ITouchpointStore
    {
        public EFTouchpointStore(SignalBoxDbContext context) : base(context, c => c.Touchpoints)
        {
        }
    }
}
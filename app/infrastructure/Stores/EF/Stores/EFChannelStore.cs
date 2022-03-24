using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFChannelStore : EFEntityStoreBase<ChannelBase>, IChannelStore
    {
        public EFChannelStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, (c) => c.Channels)
        { }
    }
}
using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFChannelStore : EFEnvironmentScopedEntityStoreBase<ChannelBase>, IChannelStore
    {
        protected override bool IsEnvironmentScoped => true;
        public EFChannelStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentProvider environmentProvider)
        : base(contextProvider, environmentProvider, (c) => c.Channels)
        { }
    }
}
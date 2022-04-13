using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFWebChannelStore : EFEnvironmentScopedEntityStoreBase<WebChannel>, IWebChannelStore
    {
        protected override bool IsEnvironmentScoped => true;
        public EFWebChannelStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentProvider environmentProvider)
        : base(contextProvider, environmentProvider, (c) => c.WebChannels)
        { }
    }
}
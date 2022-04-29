using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFEmailChannelStore : EFEnvironmentScopedEntityStoreBase<EmailChannel>, IEmailChannelStore
    {
        protected override bool IsEnvironmentScoped => true;
        public EFEmailChannelStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentProvider environmentProvider)
        : base(contextProvider, environmentProvider, (c) => c.EmailChannels)
        { }
    }
}
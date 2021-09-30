using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFModelRegistrationStore : EFEntityStoreBase<ModelRegistration>, IModelRegistrationStore
    {
        public EFModelRegistrationStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, (c) => c.ModelRegistrations)
        { }
    }
}
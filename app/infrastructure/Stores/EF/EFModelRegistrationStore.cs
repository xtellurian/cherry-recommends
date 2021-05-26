using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFModelRegistrationStore : EFEntityStoreBase<ModelRegistration>, IModelRegistrationStore
    {
        public EFModelRegistrationStore(SignalBoxDbContext context)
        : base(context, (c) => c.ModelRegistrations)
        {
        }
    }
}
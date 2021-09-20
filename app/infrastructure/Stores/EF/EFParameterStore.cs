using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFParameterStore : EFCommonEntityStoreBase<Parameter>, IParameterStore
    {
        protected override bool IsEnvironmentScoped => true;
        public EFParameterStore(SignalBoxDbContext context, IEnvironmentService environmentService)
        : base(context, environmentService, (c) => c.Parameters)
        {
        }
    }
}
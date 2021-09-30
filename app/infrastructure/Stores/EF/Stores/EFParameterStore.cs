using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFParameterStore : EFCommonEntityStoreBase<Parameter>, IParameterStore
    {
        protected override bool IsEnvironmentScoped => true;
        public EFParameterStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentService environmentService)
        : base(contextProvider, environmentService, (c) => c.Parameters)
        { }
    }
}
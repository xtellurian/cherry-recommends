using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class BusinessTypeConfiguration : CommonEntityTypeConfigurationBase<Business>, IEntityTypeConfiguration<Business>
    {
        protected override DeleteBehavior OnEnvironmentDelete => DeleteBehavior.NoAction;
        public override void Configure(EntityTypeBuilder<Business> builder)
        {
            base.Configure(builder);
        }
    }
}
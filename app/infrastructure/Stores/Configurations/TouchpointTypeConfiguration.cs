using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class TouchpointTypeConfiguration : CommonEntityTypeConfigurationBase<Touchpoint>, IEntityTypeConfiguration<Touchpoint>
    {
        public override void Configure(EntityTypeBuilder<Touchpoint> builder)
        {
            base.Configure(builder);
        }
    }
}
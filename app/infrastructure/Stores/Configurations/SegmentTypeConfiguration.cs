using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class SegmentTypeConfiguration : EnvironmentScopedEntityTypeConfigurationBase<Segment>, IEntityTypeConfiguration<Segment>
    {
        public override void Configure(EntityTypeBuilder<Segment> builder)
        {
            base.Configure(builder);

            builder.HasData(Segment.MoreThan10Events);
        }
    }
}
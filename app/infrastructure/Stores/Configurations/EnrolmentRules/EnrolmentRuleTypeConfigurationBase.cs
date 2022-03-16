
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Segments;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal abstract class EnrolmentRuleTypeConfigurationBase<T> : EntityTypeConfigurationBase<T>, IEntityTypeConfiguration<T> where T : EnrolmentRule
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            builder
                .HasOne(_ => _.Segment)
                .WithMany()
                .HasForeignKey(_ => _.SegmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
    internal class EnrolmentRuleTypeConfiguration : EnrolmentRuleTypeConfigurationBase<EnrolmentRule>, IEntityTypeConfiguration<EnrolmentRule>
    {
        public override void Configure(EntityTypeBuilder<EnrolmentRule> builder)
        {
            base.Configure(builder);
        }
    }
}

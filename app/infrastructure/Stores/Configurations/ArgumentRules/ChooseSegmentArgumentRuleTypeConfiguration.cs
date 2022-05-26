using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class ChooseSegmentArgumentRuleTypeConfiguration : ArgumentRuleTypeConfigurationBase<ChooseSegmentArgumentRule>
    {
        public override void Configure(EntityTypeBuilder<ChooseSegmentArgumentRule> builder)
        {
            base.Configure(builder);

            builder.Property(_ => _.ArgumentValue)
                .HasColumnName("ArgumentValue")
                .HasMaxLength(127);

            builder.HasIndex(_ => new { _.ArgumentId, _.SegmentId, _.CampaignId, _.ArgumentValue })
                .IsUnique();

            builder.Navigation(_ => _.Segment).AutoInclude();
        }
    }
}

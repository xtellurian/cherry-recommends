using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Campaigns;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class ChoosePromotionArgumentRuleTypeConfiguration : ArgumentRuleTypeConfigurationBase<ChoosePromotionArgumentRule>
    {
        public override void Configure(EntityTypeBuilder<ChoosePromotionArgumentRule> builder)
        {
            base.Configure(builder);

            builder.Property(_ => _.ArgumentValue)
                .HasColumnName("ArgumentValue")
                .HasMaxLength(127);

            builder.HasIndex(_ => new { _.ArgumentId, _.PromotionId, _.CampaignId, _.ArgumentValue })
                .IsUnique();
        }
    }
}

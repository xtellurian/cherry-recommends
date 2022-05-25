using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class ChoosePromotionArgumentRuleTypeConfiguration : ArgumentRuleTypeConfigurationBase<ChoosePromotionArgumentRule>
    {
        public override void Configure(EntityTypeBuilder<ChoosePromotionArgumentRule> builder)
        {
            base.Configure(builder);

            builder.HasIndex(_ => new { _.ArgumentId, _.PromotionId, _.CampaignId, _.ArgumentValue })
                .IsUnique();
        }
    }
}

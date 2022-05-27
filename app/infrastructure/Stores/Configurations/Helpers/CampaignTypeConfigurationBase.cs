using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Campaigns;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal abstract class CampaignEntityBaseTypeConfigurationBase<T>
       : CommonEntityTypeConfigurationBase<T>, IEntityTypeConfiguration<T> where T : CampaignEntityBase
    {
        protected override DeleteBehavior OnEnvironmentDelete => DeleteBehavior.SetNull;
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder
                .HasMany(_ => _.RecommenderInvokationLogs)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasMany(_ => _.RecommendationCorrelators)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(_ => _.OldArguments).IsRequired(false).HasJsonConversion();
            builder.Property(_ => _.ErrorHandling).HasJsonConversion();
            builder.Property(_ => _.TriggerCollection).HasJsonConversion();
            builder.Property(_ => _.Settings).HasJsonConversion();

            builder.ToTable("Recommenders"); // db backwards compatibility
        }
    }

    internal class RecommenderEntityBaseTypeConfiguration
       : CampaignEntityBaseTypeConfigurationBase<CampaignEntityBase>, IEntityTypeConfiguration<CampaignEntityBase>
    {
        public override void Configure(EntityTypeBuilder<CampaignEntityBase> builder)
        {
            base.Configure(builder);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Campaigns;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class ParameterSetCampaignTypeConfiguration
        : CampaignEntityBaseTypeConfigurationBase<ParameterSetCampaign>, IEntityTypeConfiguration<ParameterSetCampaign>
    {
        public override void Configure(EntityTypeBuilder<ParameterSetCampaign> builder)
        {
            base.Configure(builder);
            builder.Property(_ => _.ParameterBounds).HasJsonConversion();
            builder.Property(_ => _.OldArguments).HasJsonConversion();
            builder.Property(_ => _.TriggerCollection).HasJsonConversion();

            builder
                .HasMany(_ => _.Recommendations)
                .WithOne(_ => _.Recommender)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasMany(_ => _.Parameters)
                .WithMany(_ => _.ParameterSetRecommenders)
                .UsingEntity(join => join.ToTable("ParameterParameterSetRecommender")); // backwards compat old join entity table

            builder.HasDiscriminator()
                .HasValue<ParameterSetCampaign>("ParameterSetRecommender"); // backwards compat old discriminator
        }
    }
}
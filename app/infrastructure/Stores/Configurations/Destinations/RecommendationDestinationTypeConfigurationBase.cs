using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;
using SignalBox.Core.Recommendations.Destinations;
using SignalBox.Core.Campaigns;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal abstract class RecommendationDestinationTypeConfigurationBase<TDestination> : EntityTypeConfigurationBase<TDestination>, IEntityTypeConfiguration<TDestination>
    where TDestination : RecommendationDestinationBase
    {
        // I don't inherit from CommonEntityTypeConfiguration because sometimes the CommonID of an integrated system is null
        // If you change this inheritence, be sure to go through the onboarding process for a new integrated system.
        public override void Configure(EntityTypeBuilder<TDestination> builder)
        {
            base.Configure(builder);
            builder.Ignore(_ => _.DestinationType);
            builder.Property(_ => _.Trigger).HasJsonConversion();

            builder.HasDiscriminator(_ => _.Discriminator);

            builder
                .HasOne(_ => _.ConnectedSystem)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(_ => _.Recommender)
                .WithMany(nameof(CampaignEntityBase.RecommendationDestinations))
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);
        }
    }

    internal class RecommendationDestinationTypeConfiguration : RecommendationDestinationTypeConfigurationBase<RecommendationDestinationBase>
    { }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Recommendations;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class RecommendationCorrelatorTypeConfiguration : EntityTypeConfigurationBase<RecommendationCorrelator>, IEntityTypeConfiguration<RecommendationCorrelator>
    {
        public override void Configure(EntityTypeBuilder<RecommendationCorrelator> builder)
        {
            base.Configure(builder);
            builder
                .HasOne(_ => _.ModelRegistration)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

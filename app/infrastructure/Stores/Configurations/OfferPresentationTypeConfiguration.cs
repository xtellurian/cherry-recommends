using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;
using SignalBox.Core.Recommendations;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class OfferPresentationTypeConfiguration : RecommendationEntityTypeConfigurationBase<OfferRecommendation>, IEntityTypeConfiguration<OfferRecommendation>
    {
        public override void Configure(EntityTypeBuilder<OfferRecommendation> builder)
        {
            base.Configure(builder);

            builder
                .Property(_ => _.Features)
                .HasJsonConversion();
        }
    }
}
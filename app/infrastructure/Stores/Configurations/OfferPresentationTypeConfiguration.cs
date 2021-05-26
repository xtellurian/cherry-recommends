using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class OfferPresentationTypeConfiguration : EntityTypeConfigurationBase<OfferRecommendation>, IEntityTypeConfiguration<OfferRecommendation>
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
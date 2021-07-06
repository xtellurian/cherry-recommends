using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Recommendations;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class ProductRecommendationTypeConfiguration : RecommendationEntityTypeConfigurationBase<ProductRecommendation>, IEntityTypeConfiguration<ProductRecommendation>
    {
        public override void Configure(EntityTypeBuilder<ProductRecommendation> builder)
        {
            base.Configure(builder);
        }
    }
}
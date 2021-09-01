using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Recommendations;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class ItemsRecommendationTypeConfiguration : RecommendationEntityTypeConfigurationBase<ItemsRecommendation>, IEntityTypeConfiguration<ItemsRecommendation>
    {
        public override void Configure(EntityTypeBuilder<ItemsRecommendation> builder)
        {
            base.Configure(builder);

            builder
                .HasMany(_ => _.Items)
                .WithMany(_ => _.Recommendations);

            builder
                .Property(_ => _.Scores)
                .HasJsonConversion();

            builder.Ignore(_ => _.ScoredItems);
        }
    }
}
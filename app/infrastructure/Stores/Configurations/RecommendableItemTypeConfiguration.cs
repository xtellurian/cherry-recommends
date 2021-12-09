using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class RecommendableItemTypeConfiguration : CommonEntityTypeConfigurationBase<RecommendableItem>, IEntityTypeConfiguration<RecommendableItem>
    {
        public override void Configure(EntityTypeBuilder<RecommendableItem> builder)
        {
            base.Configure(builder);

            builder
                .HasMany(_ => _.Recommenders)
                .WithMany(_ => _.Items);

            builder.Property("Discriminator").HasDefaultValue("Product");

            builder.HasData(RecommendableItem.DefaultRecommendableItem);
        }
    }
}
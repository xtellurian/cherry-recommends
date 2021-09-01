using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class ProductTypeConfiguration : CommonEntityTypeConfigurationBase<Product>, IEntityTypeConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);

            builder
                .HasMany(_ => _.ProductRecommenders)
                .WithMany(_ => _.Products);
        }
    }
}
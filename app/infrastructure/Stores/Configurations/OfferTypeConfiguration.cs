using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class OrderTypeConfiguration : EntityTypeConfigurationBase<Offer>, IEntityTypeConfiguration<Offer>
    {
        public override void Configure(EntityTypeBuilder<Offer> builder)
        {
            base.Configure(builder);
            builder.Property(t => t.Price)
                .IsRequired();
        }
    }
}
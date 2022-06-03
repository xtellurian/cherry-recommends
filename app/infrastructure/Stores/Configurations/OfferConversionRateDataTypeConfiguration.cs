using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class OfferConversionRateDataTypeConfiguration : IEntityTypeConfiguration<OfferConversionRateData>
    {
        public void Configure(EntityTypeBuilder<OfferConversionRateData> builder)
        {
            builder.HasNoKey();
            builder.Metadata.SetIsTableExcludedFromMigrations(true);
        }
    }
}
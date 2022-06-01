using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class OfferMeanGrossRevenueTypeConfiguration : IEntityTypeConfiguration<OfferMeanGrossRevenue>
    {
        public void Configure(EntityTypeBuilder<OfferMeanGrossRevenue> builder)
        {
            builder.HasNoKey();
            builder.Metadata.SetIsTableExcludedFromMigrations(true);
        }
    }
}
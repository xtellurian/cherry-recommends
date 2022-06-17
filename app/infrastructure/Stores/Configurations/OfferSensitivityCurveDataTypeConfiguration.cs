using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class OfferSensitivityCurveDataTypeConfiguration : IEntityTypeConfiguration<OfferSensitivityCurveData>
    {
        public void Configure(EntityTypeBuilder<OfferSensitivityCurveData> builder)
        {
            builder.HasNoKey();
            builder.Metadata.SetIsTableExcludedFromMigrations(true);
        }
    }
}
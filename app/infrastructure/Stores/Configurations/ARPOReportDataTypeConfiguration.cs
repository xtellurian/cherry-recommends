using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class ARPOReportDataTypeConfiguration : IEntityTypeConfiguration<ARPOReportData>
    {
        public void Configure(EntityTypeBuilder<ARPOReportData> builder)
        {
            builder.HasNoKey();
            builder.Metadata.SetIsTableExcludedFromMigrations(true);
        }
    }
}
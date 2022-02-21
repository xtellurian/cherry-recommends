using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class MetricDailyBinValueNumericTypeConfiguration : IEntityTypeConfiguration<MetricDailyBinValueNumeric>
    {
        public void Configure(EntityTypeBuilder<MetricDailyBinValueNumeric> builder)
        {
            builder.HasNoKey();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class MetricDailyBinValueStringTypeConfiguration : IEntityTypeConfiguration<MetricDailyBinValueString>
    {
        public void Configure(EntityTypeBuilder<MetricDailyBinValueString> builder)
        {
            builder.HasNoKey();
        }
    }
}
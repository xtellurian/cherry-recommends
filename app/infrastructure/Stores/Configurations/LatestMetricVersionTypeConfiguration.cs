using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class LatestMetricVersionTypeConfiguration : IEntityTypeConfiguration<LatestMetric>
    {
        public void Configure(EntityTypeBuilder<LatestMetric> builder)
        {
            builder.HasNoKey();
            // builder.ToView("View_MaxHistoricTrackedUserFeatureVersion");
            // builder.ToView("View_MaxHistoricCustomerMetricVersion");
            builder.ToView("View_LatestMetrics");
        }
    }
}
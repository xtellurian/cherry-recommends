using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class LatestMetricVersionTypeConfiguration : IEntityTypeConfiguration<LatestMetricVersion>
    {
        public void Configure(EntityTypeBuilder<LatestMetricVersion> builder)
        {
            builder.HasNoKey();
            // builder.ToView("View_MaxHistoricTrackedUserFeatureVersion");
            builder.ToView("View_MaxHistoricCustomerMetricVersion");
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class LatestMetricVersionTypeConfiguration : IEntityTypeConfiguration<LatestMetric>
    {
        //    migrationBuilder.Sql(
        //             @"CREATE VIEW View_MaxHistoricCustomerMetricVersion AS
        //                 (
        //                     SELECT Id HistoricCustomerMetricId, CustomerId, Version MaxVersion, MetricId, NumericValue, StringValue
        //                     FROM HistoricTrackedUserFeatures allValues
        //                     JOIN(
        //                         SELECT max(Id) HistoricCustomerMetricId, CustomerId as cIs, MetricId as mId, max(Version) MaxVersion
        //                         FROM HistoricTrackedUserFeatures
        //                         GROUP BY CustomerId, MetricId
        //                     ) n
        //                     ON allValues.Id = n.HistoricCustomerMetricId
        //                 )"
        //             );
        public void Configure(EntityTypeBuilder<LatestMetric> builder)
        {
            builder.HasNoKey();
            // builder.ToView("View_MaxHistoricTrackedUserFeatureVersion");
            builder.ToView("View_MaxHistoricCustomerMetricVersion");
        }
    }
}
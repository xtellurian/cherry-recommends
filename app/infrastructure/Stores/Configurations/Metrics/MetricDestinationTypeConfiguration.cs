using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;
using SignalBox.Core.Metrics.Destinations;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class MetricDestinationTypeConfiguration : EntityTypeConfigurationBase<MetricDestinationBase>, IEntityTypeConfiguration<MetricDestinationBase>
    {
        public override void Configure(EntityTypeBuilder<MetricDestinationBase> builder)
        {
            base.Configure(builder);

            builder.HasDiscriminator()
                .HasValue<WebhookMetricDestination>("WebhookFeatureDestination") // backwards compat old discrimintors
                .HasValue<WebhookMetricDestination>("WebhookMetricDestination");
        }
    }
}
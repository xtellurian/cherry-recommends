using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;
using SignalBox.Core.Recommendations.Destinations;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class WebhookDestinationTypeConfiguration : RecommendationDestinationTypeConfigurationBase<WebhookDestination>, IEntityTypeConfiguration<WebhookDestination>
    {
        public override void Configure(EntityTypeBuilder<WebhookDestination> builder)
        {
            base.Configure(builder);
        }
    }
}
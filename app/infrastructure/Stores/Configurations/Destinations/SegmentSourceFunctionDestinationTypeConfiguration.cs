using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;
using SignalBox.Core.Recommendations.Destinations;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class SegmentSourceFunctionDestinationTypeConfiguration
    : RecommendationDestinationTypeConfigurationBase<SegmentSourceFunctionDestination>, IEntityTypeConfiguration<SegmentSourceFunctionDestination>
    {
        public override void Configure(EntityTypeBuilder<SegmentSourceFunctionDestination> builder)
        {
            base.Configure(builder);
        }
    }
}
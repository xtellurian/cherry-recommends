using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class LatestFeatureVersionTypeConfiguration : IEntityTypeConfiguration<LatestFeatureVersion>
    {
        public void Configure(EntityTypeBuilder<LatestFeatureVersion> builder)
        {
            builder.HasNoKey();
            builder.ToView("View_MaxHistoricTrackedUserFeatureVersion");
        }
    }
}
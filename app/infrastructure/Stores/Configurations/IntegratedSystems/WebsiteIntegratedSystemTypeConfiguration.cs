using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Integrations.Website;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class WebsiteIntegratedSystemTypeConfiguration : IntegratedSystemTypeConfigurationBase<WebsiteIntegratedSystem>, IEntityTypeConfiguration<WebsiteIntegratedSystem>
    {
        // I don't inherit from CommonEntityTypeConfiguration because sometimes the CommonID of an integrated system is null
        // If you change this inheritence, be sure to go through the onboarding process for a new integrated system.
        public override void Configure(EntityTypeBuilder<WebsiteIntegratedSystem> builder)
        {
            base.Configure(builder);

            builder.Property(_ => _.ApplicationId) // shared column as CustomIntegratedSystem
                .HasColumnName("ApplicationId");

            builder.Property(_ => _.ApplicationSecret) // shared column as CustomIntegratedSystem
                .HasColumnName("ApplicationSecret");
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;
using SignalBox.Core.Integrations.Custom;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class CustomIntegratedSystemTypeConfiguration : IntegratedSystemTypeConfigurationBase<CustomIntegratedSystem>, IEntityTypeConfiguration<CustomIntegratedSystem>
    {
        // I don't inherit from CommonEntityTypeConfiguration because sometimes the CommonID of an integrated system is null
        // If you change this inheritence, be sure to go through the onboarding process for a new integrated system.
        public override void Configure(EntityTypeBuilder<CustomIntegratedSystem> builder)
        {
            base.Configure(builder);
        }
    }
}
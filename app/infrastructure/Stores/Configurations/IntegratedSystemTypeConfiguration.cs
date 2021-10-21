using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class IntegratedSystemTypeConfiguration : EntityTypeConfigurationBase<IntegratedSystem>, IEntityTypeConfiguration<IntegratedSystem>
    {
        // I don't inherit from CommonEntityTypeConfiguration because sometimes the CommonID of an integrated system is null
        // If you change this inheritence, be sure to go through the onboarding process for a new integrated system.
        public override void Configure(EntityTypeBuilder<IntegratedSystem> builder)
        {
            base.Configure(builder);

            builder
                .Property(t => t.Name)
                .IsRequired();

            builder
                .Property(t => t.SystemType)
                .IsRequired()
                .HasConversion<string>();

            builder
                .Property(_ => _.TokenResponse)
                .HasJsonConversion();

            builder
                .Property(_ => _.IntegrationStatus)
                .HasDefaultValueSql("('NotConfigured')")
                .HasConversion<string>();

            builder
                .Property(_ => _.Properties)
                .HasJsonConversion();

            builder
                .HasMany(_ => _.WebhookReceivers)
                .WithOne(_ => _.IntegratedSystem)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
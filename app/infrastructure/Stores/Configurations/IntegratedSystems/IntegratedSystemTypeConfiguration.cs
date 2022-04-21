using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal abstract class IntegratedSystemTypeConfigurationBase<TSystem> : EntityTypeConfigurationBase<TSystem>, IEntityTypeConfiguration<TSystem>
    where TSystem : IntegratedSystem
    {
        // I don't inherit from CommonEntityTypeConfiguration because sometimes the CommonID of an integrated system is null
        // If you change this inheritence, be sure to go through the onboarding process for a new integrated system.
        public override void Configure(EntityTypeBuilder<TSystem> builder)
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

            builder.Property(_ => _.Discriminator).HasDefaultValue("IntegratedSystem");
            builder.HasDiscriminator(_ => _.Discriminator);

            builder
                .Property(_ => _.Properties)
                .HasJsonConversion();

            builder.Property(_ => _.IsDiscountCodeGenerator).HasDefaultValue(false);
        }
    }

    // keep this one for the base class, for backwards compat.
    internal class IntegratedSystemTypeConfiguration : IntegratedSystemTypeConfigurationBase<IntegratedSystem>, IEntityTypeConfiguration<IntegratedSystem>
    {
        public override void Configure(EntityTypeBuilder<IntegratedSystem> builder)
        {
            base.Configure(builder);

            builder
               .HasMany(_ => _.WebhookReceivers)
               .WithOne(_ => _.IntegratedSystem)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
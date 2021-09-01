using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class ModelRegistrationTypeConfiguration : EntityTypeConfigurationBase<ModelRegistration>, IEntityTypeConfiguration<ModelRegistration>
    {
        public override void Configure(EntityTypeBuilder<ModelRegistration> builder)
        {
            base.Configure(builder);
            builder
                .HasMany(_ => _.Recommenders)
                .WithOne(_ => _.ModelRegistration)
                .HasForeignKey(_ => _.ModelRegistrationId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasMany(_ => _.Correlators)
                .WithOne(_ => _.ModelRegistration)
                .HasForeignKey(_ => _.ModelRegistrationId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(t => t.ModelType)
                .HasConversion<string>()
                .IsRequired();
            builder.Property(t => t.HostingType)
                .HasConversion<string>()
                .IsRequired();
            builder.Property(t => t.Swagger).HasJsonConversion();
        }
    }
}
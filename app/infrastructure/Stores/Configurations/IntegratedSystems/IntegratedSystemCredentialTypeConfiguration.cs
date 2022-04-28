using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class IntegratedSystemCredentialTypeConfiguration : EntityTypeConfigurationBase<IntegratedSystemCredential>, IEntityTypeConfiguration<IntegratedSystemCredential>
    {
        public override void Configure(EntityTypeBuilder<IntegratedSystemCredential> builder)
        {
            base.Configure(builder);

            builder
                .Property(_ => _.Key)
                .HasMaxLength(30)
                .IsRequired();

            builder
                .Property(_ => _.SystemType)
                .HasMaxLength(30)
                .IsRequired()
                .HasConversion<string>();

            builder
                .Property(_ => _.Credentials)
                .HasMaxLength(300);

            builder
                .Property(_ => _.Config)
                .HasMaxLength(300);

            builder
                .HasIndex(_ => _.Key)
                .IsUnique();
        }
    }
}
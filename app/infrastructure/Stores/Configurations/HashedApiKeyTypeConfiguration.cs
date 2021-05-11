using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class HashedApiKeyTypeConfiguration : EntityTypeConfigurationBase<HashedApiKey>, IEntityTypeConfiguration<HashedApiKey>
    {
        public override void Configure(EntityTypeBuilder<HashedApiKey> builder)
        {
            base.Configure(builder);
            builder.Property(t => t.Name)
                .IsRequired();
        }
    }
}
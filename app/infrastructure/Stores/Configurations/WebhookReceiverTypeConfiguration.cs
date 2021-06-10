using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class WebhookReceiverTypeConfiguration : EntityTypeConfigurationBase<WebhookReceiver>, IEntityTypeConfiguration<WebhookReceiver>
    {
        public override void Configure(EntityTypeBuilder<WebhookReceiver> builder)
        {
            base.Configure(builder);
            builder.HasIndex(t => t.EndpointId)
                .IsUnique();
        }
    }
}
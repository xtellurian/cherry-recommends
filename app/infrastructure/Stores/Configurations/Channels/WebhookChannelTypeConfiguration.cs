using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class WebhookChannelTypeConfiguration : ChannelBaseTypeConfiguration<WebhookChannel>, IEntityTypeConfiguration<WebhookChannel>
    {
        public override void Configure(EntityTypeBuilder<WebhookChannel> builder)
        {
            base.Configure(builder);

            builder.Property(_ => _.Endpoint) // shared column as WebChannel
                .HasColumnName("Endpoint");
        }
    }
}
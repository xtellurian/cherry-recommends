using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class WebChannelTypeConfiguration : ChannelBaseTypeConfiguration<WebChannel>, IEntityTypeConfiguration<WebChannel>
    {
        public override void Configure(EntityTypeBuilder<WebChannel> builder)
        {
            base.Configure(builder);

            builder.Property(_ => _.Endpoint) // shared column as WebhookChannel
                .HasColumnName("Endpoint");
        }
    }
}
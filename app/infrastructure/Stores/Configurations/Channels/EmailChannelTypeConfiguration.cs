using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class EmailChannelTypeConfiguration : ChannelBaseTypeConfiguration<EmailChannel>, IEntityTypeConfiguration<EmailChannel>
    {
        public override void Configure(EntityTypeBuilder<EmailChannel> builder)
        {
            base.Configure(builder);
        }
    }
}
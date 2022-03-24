using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal abstract class ChannelBaseTypeConfiguration<T> : EntityTypeConfigurationBase<T>, IEntityTypeConfiguration<T> where T : ChannelBase
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(_ => _.ChannelType).HasConversion<string>();

            builder.ToTable("Channels");
        }
    }

    internal class ChannelTypeConfiguration : ChannelBaseTypeConfiguration<ChannelBase>
    { }
}
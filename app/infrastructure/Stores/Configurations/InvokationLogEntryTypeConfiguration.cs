using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class InvokationLogEntryTypeConfiguration : EntityTypeConfigurationBase<InvokationLogEntry>, IEntityTypeConfiguration<InvokationLogEntry>
    {
        public override void Configure(EntityTypeBuilder<InvokationLogEntry> builder)
        {
            base.Configure(builder);
            builder.HasIndex(_ => _.InvokeStarted);
            builder.Property(_ => _.Messages).HasJsonConversion();
        }
    }
}
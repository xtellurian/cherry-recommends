using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Campaigns;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class InvokationLogEntryTypeConfiguration : EntityTypeConfigurationBase<InvokationLogEntry>, IEntityTypeConfiguration<InvokationLogEntry>
    {
        public override void Configure(EntityTypeBuilder<InvokationLogEntry> builder)
        {
            base.Configure(builder);
            builder.HasIndex(_ => _.InvokeStarted);
            builder.Property(_ => _.Messages).HasJsonConversion();

            builder
                .HasOne(_ => _.Customer)
                .WithMany()
                .HasForeignKey(_ => _.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(_ => _.Business)
                .WithMany()
                .HasForeignKey(_ => _.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property("CampaignEntityBaseId").HasColumnName("RecommenderEntityBaseId"); // backwards compatibility
        }
    }
}
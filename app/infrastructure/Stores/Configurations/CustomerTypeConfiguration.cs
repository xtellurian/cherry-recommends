using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class CustomerTypeConfiguration : CommonEntityTypeConfigurationBase<Customer>, IEntityTypeConfiguration<Customer>
    {
        // TODO: make this cascase (causes error on migrate)
        protected override DeleteBehavior OnEnvironmentDelete => DeleteBehavior.Cascade;
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            base.Configure(builder);

            builder.ToTable("TrackedUsers");

            builder.Property(_ => _.CommonId) // fixes legacy column name
                .HasColumnName("CommonUserId");

            builder.OwnsOne(_ => _.BusinessMembership, (b2) =>
            {
                b2.WithOwner(_ => _.Customer);
                b2.HasOne(_ => _.Business)
                    .WithMany()
                    .HasForeignKey(_ => _.BusinessId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            builder.HasData(Customer.Anonymous);
        }
    }
}
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal abstract class ArgumentRuleTypeConfigurationBase<T> : EntityTypeConfigurationBase<T>, IEntityTypeConfiguration<T> where T : ArgumentRule
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.HasOne(_ => _.Campaign)
                .WithMany(_ => (IEnumerable<T>)_.ArgumentRules)
                .HasForeignKey(_ => _.CampaignId)
                .OnDelete(DeleteBehavior.NoAction); // don't do a double cascade

            builder.HasOne(_ => _.Argument)
                .WithMany()
                .HasForeignKey(_ => _.ArgumentId)
                .OnDelete(DeleteBehavior.Cascade); // delete when argument is deleted
        }
    }
}
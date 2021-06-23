using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class ParameterTypeConfiguration : CommonEntityTypeConfigurationBase<Parameter>, IEntityTypeConfiguration<Parameter>
    {
        public override void Configure(EntityTypeBuilder<Parameter> builder)
        {
            base.Configure(builder);
            builder.Property(_ => _.ParameterType).HasConversion<string>();
            builder.Property(_ => _.DefaultValue).HasJsonConversion();
        }
    }
}
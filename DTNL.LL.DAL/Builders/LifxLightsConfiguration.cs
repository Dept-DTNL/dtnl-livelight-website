using DTNL.LL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DTNL.LL.DAL.Builders
{
    public class LifxLightsConfiguration : IEntityTypeConfiguration<LifxLight>
    {
        public void Configure(EntityTypeBuilder<LifxLight> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .UseIdentityColumn();

            builder.Property(m => m.LifxApiKey)
                .IsRequired(false);

            builder.Property(m => m.LightGroupName)
                .IsRequired();

            builder.Property(m => m.TimeRangeEnabled)
                .IsRequired();

            builder.Property(m => m.TimeRangeStart)
                .IsRequired();

            builder.Property(m => m.TimeRangeEnd)
                .IsRequired();
        }
    }
}
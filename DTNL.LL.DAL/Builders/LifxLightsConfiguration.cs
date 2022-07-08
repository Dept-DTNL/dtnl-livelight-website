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

            builder.Property(m => m.ConversionColor)
                .IsRequired();

            builder.Property(m => m.ConversionPeriod)
                .IsRequired();

            builder.Property(m => m.HighTrafficAmount)
                .IsRequired();

            builder.Property(m => m.HighTrafficBrightness)
                .IsRequired();

            builder.Property(m => m.HighTrafficBrightness)
                .IsRequired();

            builder.Property(m => m.MediumTrafficAmount)
                .IsRequired();

            builder.Property(m => m.MediumTrafficBrightness)
                .IsRequired();

            builder.Property(m => m.MediumTrafficColor)
                .IsRequired();

            builder.Property(m => m.LowTrafficBrightness)
                .IsRequired();

            builder.Property(m => m.LowTrafficColor)
                .IsRequired();

            builder.Property(m => m.Uuid)
                .IsRequired();

            builder.HasOne(m => m.Project)
                .WithMany(m => m.LifxLights);
            
            builder.Property(m => m.TimeRangeEnabled)
                .IsRequired();

            builder.Property(m => m.TimeRangeStart)
                .IsRequired();

            builder.Property(m => m.TimeRangeEnd)
                .IsRequired();

            builder.Property(m => m.VeryHighTrafficAmount)
                .IsRequired().HasDefaultValue(0);

            builder.Property(m => m.VeryHighTrafficCycleTime)
                .IsRequired().HasDefaultValue(1);

            builder.Property(m => m.EffectCooldownInMinutes)
                .IsRequired().HasDefaultValue(5);

            builder.Property(m => m.PulseAmount)
                .IsRequired().HasDefaultValue(1);
        }
    }
}
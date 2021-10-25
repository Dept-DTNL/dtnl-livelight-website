using System;
using System.Collections.Generic;
using System.Linq;
using DTNL.LL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DTNL.LL.DAL.Builders
{
    internal class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(m => m.Id);

            //Auto Increment id
            builder.Property(m => m.Id)
                .UseIdentityColumn();

            builder.Property(m => m.ProjectName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.CustomerName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.AnalyticsVersion)
                .IsRequired();

            builder.Property(m => m.PollingTimeInMinutes)
                .IsRequired();

            builder.Property(m => m.Active)
                .IsRequired();

            // Lamp connection
            builder.Property(m => m.LifxApiKey)
                .IsRequired(false);

            builder.Property(m => m.LightGroupName)
                .IsRequired();

            builder.Property(m => m.Uuid)
                .IsRequired();

            builder.Property(m => m.GuideEnabled)
                .IsRequired();

            // Time Range
            builder.Property(m => m.TimeRangeEnabled)
                .IsRequired();

            builder.Property(m => m.TimeRangeStart)
                .IsRequired();

            builder.Property(m => m.TimeRangeEnd)
                .IsRequired();

            // Light settings
            builder.Property(m => m.LowTrafficColor)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("red");

            builder.Property(m => m.LowTrafficBrightness)
                .IsRequired()
                .HasDefaultValue(0.5);

            builder.Property(m => m.MediumTrafficAmount)
                .IsRequired()
                .HasDefaultValue(5);

            builder.Property(m => m.MediumTrafficColor)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("orange");

            builder.Property(m => m.MediumTrafficBrightness)
                .IsRequired()
                .HasDefaultValue(0.5);

            builder.Property(m => m.HighTrafficColor)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("green");

            builder.Property(m => m.HighTrafficBrightness)
                .IsRequired()
                .HasDefaultValue(0.5);

            builder.Property(m => m.HighTrafficAmount)
                .IsRequired()
                .HasDefaultValue(50);

            builder.Property(m => m.ConversionColor)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("blue");

            builder.Property(m => m.ConversionCycle)
                .IsRequired()
                .HasDefaultValue(0.5);

            builder.Property(m => m.ConversionPeriod)
                .IsRequired()
                .HasDefaultValue(20);

            ValueComparer<List<string>> conversionValueComparer = new ValueComparer<List<string>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList());

            builder.Property(m => m.ConversionTags)
                .HasConversion(v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
                .Metadata.SetValueComparer(conversionValueComparer);

            builder.ToTable("Projects");
        }
    }
}

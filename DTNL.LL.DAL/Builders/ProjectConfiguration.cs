using DTNL.LL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DTNL.LL.DAL.Builders
{
    class ProjectConfiguration : IEntityTypeConfiguration<Project>
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

            builder.Property(m => m.Active)
                .IsRequired();
            
            builder.HasMany(m => m.Lamps)
                .WithOne()
                .IsRequired();

            builder.Property(m => m.TimeRangeEnabled)
                .IsRequired();

            builder.Property(m => m.TimeRangeStart)
                .IsRequired(false);

            builder.Property(m => m.TimeRangeEnd)
                .IsRequired(false);

            builder.ToTable("Projects");
        }
    }
}

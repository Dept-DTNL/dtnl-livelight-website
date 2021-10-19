using DTNL.LL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DTNL.LL.DAL.Builders
{
    internal class LampConfiguration : IEntityTypeConfiguration<Lamp>
    {
        public void Configure(EntityTypeBuilder<Lamp> builder)
        {
            builder.HasKey(m => m.Id);

            //Auto Increment id
            builder.Property(m => m.Id)
                .UseIdentityColumn();

            builder.Property(m => m.AccessToken)
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(m => m.TokenType)
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(m => m.ExpiresAt)
                .IsRequired();

            builder.Property(m => m.RefreshToken)
                .IsRequired()
                .HasMaxLength(64);

            builder.ToTable("Lamps");
        }
    }
}
using Authentication.RefreshToken.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.RefreshToken.Persistence.Configurations
{
    public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.ToTable("RoleClaims");

            builder.HasKey(rc => rc.Id);

            builder.Property(rc => rc.ClaimType)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(rc => rc.ClaimValue)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(rc => rc.IsEnabled)
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.Property(rc => rc.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(r => r.CreatedBy)
                   .IsRequired(false);

            builder.Property(r => r.UpdatedAt)
                   .IsRequired(false);

            builder.Property(r => r.UpdatedBy)
                   .IsRequired(false);

            builder.Property(r => r.DeactivatedAt)
                   .IsRequired(false);

            builder.Property(r => r.DeactivatedBy)
                   .IsRequired(false);

            // Relaciones
            builder.HasOne(rc => rc.Role)
                   .WithMany(r => r.RoleClaims)
                   .HasForeignKey(rc => rc.RoleId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.CreatedByUser)
                .WithMany()
                .HasForeignKey(r => r.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.UpdateByUser)
                .WithMany()
                .HasForeignKey(r => r.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.DeactivatedByUser)
                .WithMany()
                .HasForeignKey(r => r.DeactivatedBy)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

using Authentication.RefreshToken.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.RefreshToken.Persistence.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(p => p.Name)
                   .IsUnique();

            builder.Property(p => p.Description)
                   .HasMaxLength(200);

            builder.Property(p => p.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.Property(p => p.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.CreatedBy)
                   .IsRequired(false);

            builder.Property(p => p.UpdatedAt)
                   .IsRequired(false);

            builder.Property(p => p.UpdatedBy)
                   .IsRequired(false);

            builder.Property(p => p.DeactivatedAt)
                   .IsRequired(false);

            builder.Property(p => p.DeactivatedBy)
                   .IsRequired(false);

            builder.HasMany(p => p.RolePermissions)
                .WithOne(rp => rp.Permission)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            //builder.HasMany(p => p.UserPermissions)
            //    .WithOne(up => up.Permission)
            //    .HasForeignKey(up => up.PermissionId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.HasMany(p => p.Menus)
            //    .WithOne(m => m.Permission)
            //    .HasForeignKey(m => m.PermissionId)
            //    .OnDelete(DeleteBehavior.Restrict);

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

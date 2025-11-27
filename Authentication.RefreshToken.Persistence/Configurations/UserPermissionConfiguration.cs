using Authentication.RefreshToken.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.RefreshToken.Persistence.Configurations
{
    public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
    {
        public void Configure(EntityTypeBuilder<UserPermission> builder)
        {
            builder.ToTable("UserPermissions");

            builder.HasKey(up => new { up.UserId, up.PermissionId });

            builder.Property(r => r.CreatedAt)
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

            builder.Property(up => up.IsEnabled)
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.HasOne(up => up.User)
                   .WithMany(u => u.UserPermissions)
                   .HasForeignKey(up => up.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(up => up.Permission)
                   .WithMany(p => p.UserPermissions)
                   .HasForeignKey(up => up.PermissionId)
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

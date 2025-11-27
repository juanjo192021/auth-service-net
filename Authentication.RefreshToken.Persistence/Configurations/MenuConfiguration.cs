using Authentication.RefreshToken.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.RefreshToken.Persistence.Configurations
{
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.ToTable("Menus");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Title)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(m => m.Route)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasIndex(m => m.Route)
                   .IsUnique();

            builder.Property(m => m.Order)
                   .IsRequired()
                   .HasDefaultValue(0);

            builder.Property(m => m.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);

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

            // Relaciones
            builder.HasOne(m => m.Permission)
                   .WithMany()
                   .HasForeignKey(m => m.PermissionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.Children)
                   .WithOne()
                   .HasForeignKey(m => m.ParentId)
                   .OnDelete(DeleteBehavior.Restrict);

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

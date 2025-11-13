using Authentication.RefreshToken.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.RefreshToken.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(u => u.Email)
                   .IsUnique();

            builder.Property(u => u.PasswordHash)
                   .IsRequired();

            builder.Property(u => u.FirstName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.LastName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.ImageUrl)
                   .HasMaxLength(int.MaxValue);

            builder.Property(u => u.DocumentType)
                   .HasMaxLength(20);

            builder.Property(u => u.DocumentNumber)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.HasIndex(u => u.DocumentNumber)
                   .IsUnique();

            builder.Property(u => u.BirthDate)
                   .IsRequired();

            builder.Property(u => u.Phone)
                   .HasMaxLength(20);

            builder.Property(u => u.Mobile)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(u => u.Gender)
                   .HasMaxLength(20);

            builder.Property(u => u.Address)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(u => u.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.Property(u => u.IsBlocked)
                   .IsRequired()
                   .HasDefaultValue(false);

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
            // UserRoles (uno a muchos)
            builder.HasMany(u => u.UserRoles)
                   .WithOne(ur => ur.User)
                   .HasForeignKey(ur => ur.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // UserClaims (uno a muchos)
            builder.HasMany(u => u.UserClaims)
                   .WithOne(uc => uc.User)
                   .HasForeignKey(uc => uc.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // UserRefreshTokens (uno a muchos)
            builder.HasMany(u => u.UserRefreshTokens)
                   .WithOne(rt => rt.User)
                   .HasForeignKey(rt => rt.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
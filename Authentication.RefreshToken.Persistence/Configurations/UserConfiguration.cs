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

            builder.Property(u => u.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.Property(u => u.IsBlocked)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(r => r.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(r => r.CreatedBy).IsRequired(false);
            builder.Property(r => r.UpdatedAt).IsRequired(false);
            builder.Property(r => r.UpdatedBy).IsRequired(false);
            builder.Property(r => r.DeactivatedAt).IsRequired(false);
            builder.Property(r => r.DeactivatedBy).IsRequired(false);

            builder.HasMany(u => u.UserRoles)
                   .WithOne(ur => ur.User)
                   .HasForeignKey(ur => ur.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.UserClaims)
                   .WithOne(uc => uc.User)
                   .HasForeignKey(uc => uc.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.UserRefreshTokens)
                   .WithOne(rt => rt.User)
                   .HasForeignKey(rt => rt.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.Employee)
                   .WithOne(e => e.User)
                   .HasForeignKey<Employee>(e => e.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.Customer)
                   .WithOne(c => c.User)
                   .HasForeignKey<Customer>(c => c.UserId)
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
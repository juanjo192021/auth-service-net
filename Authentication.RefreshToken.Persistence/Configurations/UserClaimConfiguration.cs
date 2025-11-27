using Authentication.RefreshToken.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.RefreshToken.Persistence.Configurations
{
    public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.ToTable("UserClaims");

            builder.HasKey(uc => uc.Id);

            builder.Property(uc => uc.ClaimType)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(uc => uc.ClaimValue)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(uc => uc.IsEnabled)
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.Property(ur => ur.CreatedAt)
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
            builder.HasOne(uc => uc.User)
                   .WithMany(u => u.UserClaims)
                   .HasForeignKey(uc => uc.UserId)
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

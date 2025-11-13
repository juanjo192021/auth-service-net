using Authentication.RefreshToken.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.RefreshToken.Persistence.Configurations
{
    public class UserRefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken>
    {
        public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
        {
            builder.ToTable("UserRefreshTokens");

            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.JwtId)
                   .IsRequired()
                   .HasMaxLength(int.MaxValue);

            builder.Property(rt => rt.RefreshTokenHash)
                   .IsRequired()
                   .HasMaxLength(int.MaxValue);

            builder.Property(rt => rt.IsRevoked)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(rt => rt.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(rt => rt.ExpirationDate)
                   .IsRequired();

            // Relación con User
            builder.HasOne(rt => rt.User)
                   .WithMany(u => u.UserRefreshTokens)
                   .HasForeignKey(rt => rt.UserId)
                   .OnDelete(DeleteBehavior.Cascade); // si borras usuario, borra tokens
        }
    }
}

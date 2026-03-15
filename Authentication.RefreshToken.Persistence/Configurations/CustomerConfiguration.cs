using Authentication.RefreshToken.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.RefreshToken.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.CustomerType).HasMaxLength(50);
            builder.Property(c => c.CompanyName).HasMaxLength(150);
            builder.Property(c => c.ContactName).HasMaxLength(100);

            builder.Property(c => c.DocumentType).HasMaxLength(20);
            builder.Property(c => c.DocumentNumber).HasMaxLength(20);

            builder.Property(c => c.Phone).HasMaxLength(20);
            builder.Property(c => c.Mobile).HasMaxLength(20);

            builder.Property(c => c.BillingAddress).HasMaxLength(250);
            builder.Property(c => c.ShippingAddress).HasMaxLength(250);

            builder.Property(c => c.BillingPreferences).HasMaxLength(100);
            builder.Property(c => c.CustomerSegment).HasMaxLength(50);

            builder.Property(c => c.CreditLimit)
                   .HasColumnType("decimal(18,2)");

            builder.Property(c => c.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETDATE()");
            builder.Property(c => c.CreatedBy).IsRequired(false);
            builder.Property(c => c.UpdatedAt).IsRequired(false);
            builder.Property(c => c.UpdatedBy).IsRequired(false);
            builder.Property(c => c.DeactivatedAt).IsRequired(false);
            builder.Property(c => c.DeactivatedBy).IsRequired(false);

            // --- Relaciones ---

            builder.HasOne(c => c.User)
                   .WithOne(u => u.Customer)
                   .HasForeignKey<Customer>(c => c.UserId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(c => c.UserId)
                   .IsUnique()
                   .HasFilter("[UserId] IS NOT NULL");

            builder.HasOne(c => c.CreatedByUser)
                   .WithMany()
                   .HasForeignKey(c => c.CreatedBy)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.UpdateByUser)
                   .WithMany()
                   .HasForeignKey(c => c.UpdatedBy)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.DeactivatedByUser)
                   .WithMany()
                   .HasForeignKey(c => c.DeactivatedBy)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

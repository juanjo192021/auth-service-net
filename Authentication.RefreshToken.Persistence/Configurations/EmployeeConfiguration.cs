using Authentication.RefreshToken.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.RefreshToken.Persistence.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.DocumentType).HasMaxLength(20);
            builder.Property(e => e.DocumentNumber).HasMaxLength(20);
            builder.Property(e => e.Mobile).HasMaxLength(20);
            builder.Property(e => e.Gender).HasMaxLength(20);

            builder.Property(e => e.EmergencyContactName).HasMaxLength(100);
            builder.Property(e => e.EmergencyContactPhone).HasMaxLength(20);

            builder.Property(e => e.Department).HasMaxLength(100);
            builder.Property(e => e.JobTitle).HasMaxLength(100);

            builder.Property(e => e.Salary)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.BirthDate);
            builder.Property(e => e.HireDate).IsRequired();
            builder.Property(e => e.TerminationDate);

            builder.Property(e => e.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETDATE()");
            builder.Property(e => e.CreatedBy).IsRequired(false);
            builder.Property(e => e.UpdatedAt).IsRequired(false);
            builder.Property(e => e.UpdatedBy).IsRequired(false);
            builder.Property(e => e.DeactivatedAt).IsRequired(false);
            builder.Property(e => e.DeactivatedBy).IsRequired(false);

            // --- Relaciones ---

            builder.HasOne(e => e.User)
                   .WithOne(u => u.Employee)
                   .HasForeignKey<Employee>(e => e.UserId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(e => e.UserId)
                   .IsUnique()
                   .HasFilter("[UserId] IS NOT NULL");

            builder.HasOne(e => e.Manager)
                   .WithMany()
                   .HasForeignKey(e => e.ManagerId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.CreatedByUser)
                   .WithMany()
                   .HasForeignKey(e => e.CreatedBy)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.UpdateByUser)
                   .WithMany()
                   .HasForeignKey(e => e.UpdatedBy)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.DeactivatedByUser)
                   .WithMany()
                   .HasForeignKey(e => e.DeactivatedBy)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

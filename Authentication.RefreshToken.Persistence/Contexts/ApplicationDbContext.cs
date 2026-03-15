using Authentication.RefreshToken.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Authentication.RefreshToken.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        // Entidades a mapear
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Customer>().ToTable("Customers");
            builder.Entity<Employee>().ToTable("Employees");
            builder.Entity<User>().ToTable("Users");
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<UserRole>().ToTable("UserRoles");
            builder.Entity<UserClaim>().ToTable("UserClaims");
            builder.Entity<RoleClaim>().ToTable("RoleClaims");
            builder.Entity<UserRefreshToken>().ToTable("UserRefreshTokens");
            builder.Entity<Permission>().ToTable("Permissions");
            builder.Entity<RolePermission>().ToTable("RolePermissions");
            
            base.OnModelCreating(builder);

            // Aplica automáticamente todas las configuraciones del assembly
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Ejemplo: lanzar eventos de dominio antes de guardar
            // await _mediator.DispatchDomainEvents(this);

            // Luego, guardar normalmente
            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}

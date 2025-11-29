using Authentication.RefreshToken.Domain.Entities;
using Authentication.RefreshToken.Persistence.Contexts;
using Authentication.RefreshToken.Persistence.Seed.Data;
using Authentication.RefreshToken.Persistence.Seed.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Authentication.RefreshToken.Persistence.Seed
{
    public class DatabaseSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly AdminSettings _defaultAdmin;

        public DatabaseSeeder(
            ApplicationDbContext context,
            IOptions<AdminSettings> adminOptions)
        {
            _context = context;
            _defaultAdmin = adminOptions.Value;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

            //await SeedRolesAsync();
            //await SeedPermissionsAsync();
            //var admin = await SeedAdminAsync();
            //await SeedUserRoleAsync(admin);

            var admin = await SeedAdminAsync();   // 1. Crear admin primero
            await SeedRolesAsync();          // 2. Crear roles con CreatedBy = admin.Id
            await SeedPermissionsAsync();         // 3. Insertar permisos
            await SeedUserRoleAsync(admin);
        }

        private async Task SeedRolesAsync()
        {
            var existingRoleNames = await _context.Roles
                .Select(r => r.Name)
                .ToListAsync();

            var rolesToAdd = RoleSeed.List
                .Where(r => !existingRoleNames.Contains(r.Name))
                .ToList();

            if (rolesToAdd.Any())
            {
                _context.Roles.AddRange(rolesToAdd); // Bulk Insert
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedPermissionsAsync()
        {
            // A. INSERTAR PERMISOS EN LA BD
            var allPermissions = PermissionSeed.GetPermissionsFromConstants();
            var dbPermissionsNames = await _context.Permissions.Select(p => p.Name).ToListAsync();

            var permissionsToAdd = allPermissions
                .Where(p => !dbPermissionsNames.Contains(p.Name))
                .ToList();

            if (permissionsToAdd.Any())
            {
                _context.Permissions.AddRange(permissionsToAdd);
                await _context.SaveChangesAsync();
            }

            // B. ASIGNAR TODOS LOS PERMISOS AL SUPER_ADMIN
            var superAdminRole = await _context.Roles
                .Include(r => r.RolePermissions)
                .FirstOrDefaultAsync(r => r.Name == RoleSeed.SuperAdmin.Name);

            if (superAdminRole is null) return;

            // Traemos todos los permisos de la BD (con sus IDs reales)
            var dbPermissions = await _context.Permissions.AsNoTracking().ToListAsync();
            // Identificar cuáles le faltan al SuperAdmin
            var existingPermissionIds = superAdminRole.RolePermissions?
                .Select(rp => rp.PermissionId)
                .ToList() ?? new List<int>();

            var missingPermissions = dbPermissions
                .Where(p => !existingPermissionIds.Contains(p.Id))
                .Select(p => new RolePermission
                {
                    RoleId = superAdminRole.Id,
                    PermissionId = p.Id,
                    IsEnabled = true
                }).ToList();
            if (missingPermissions.Any())
            {
                
                _context.RolePermissions.AddRange(missingPermissions);

                await _context.SaveChangesAsync();
            }
        }

        private async Task<User> SeedAdminAsync()
        {
            var email = _defaultAdmin.Email;
            var password = _defaultAdmin.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Default admin email or password is not configured.");

            var admin = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (admin is null)
            {
                admin = UserSeed.SystemAdmin(email, password);
                _context.Users.Add(admin);
                await _context.SaveChangesAsync();
            }

            return admin;
        }
        public async Task SeedUserRoleAsync(User admin)
        {
            var superAdminRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == RoleSeed.SuperAdmin.Name);

            if (superAdminRole is null)
                throw new Exception("SuperAdmin role was not created during seeding.");

            bool hasRole = await _context.UserRoles.AnyAsync(ur =>
                ur.UserId == admin.Id &&
                ur.RoleId == superAdminRole.Id);

            if (!hasRole)
            {
                _context.UserRoles.Add(new UserRole
                {
                    UserId = admin.Id,
                    RoleId = superAdminRole.Id,
                    IsAssigned = true
                });

                await _context.SaveChangesAsync();
            }
        }
    }
}

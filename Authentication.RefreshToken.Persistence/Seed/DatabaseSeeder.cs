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
        private readonly DefaultAdminSettings _defaultAdmin;

        public DatabaseSeeder(
            ApplicationDbContext context,
            IOptions<DefaultAdminSettings> adminOptions)
        {
            _context = context;
            _defaultAdmin = adminOptions.Value;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

            await SeedRolesAsync();
            var admin = await SeedAdminAsync();
            await SeedUserRoleAsync(admin);
        }

        private async Task SeedRolesAsync()
        {
            foreach (var role in DefaultRoles.List)
            {
                if (!await _context.Roles.AnyAsync(r => r.Name == role.Name))
                    _context.Roles.Add(role);
            }

            await _context.SaveChangesAsync();
        }

        private async Task<User> SeedAdminAsync()
        {
            var email = _defaultAdmin.Email;
            var password = _defaultAdmin.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Default admin email or password is not configured.");

            var admin = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (admin == null)
            {
                admin = DefaultUsers.SystemAdmin(email, password);
                _context.Users.Add(admin);
                await _context.SaveChangesAsync();
            }

            return admin;
        }
        public async Task SeedUserRoleAsync(User admin)
        {
            var superAdminRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == DefaultRoles.SuperAdmin.Name);

            if (superAdminRole == null)
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

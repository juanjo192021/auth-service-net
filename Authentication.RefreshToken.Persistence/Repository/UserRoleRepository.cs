using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Domain.Entities;
using Authentication.RefreshToken.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Authentication.RefreshToken.Persistence.Repository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        protected readonly ApplicationDbContext _context;
        public UserRoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> CreateAsync(int userId, List<int> roleIds)
        {
            var validRoles = await _context.Roles
                .Where(r => roleIds.Contains(r.Id) && r.IsActive)
                .Select(r => r.Id)
                .ToListAsync();

            if (!validRoles.Any())
                return 0;

            var userRoles = validRoles.Select(roleId => new UserRole
            {
                UserId = userId,
                RoleId = roleId,
                IsAssigned = true
            }).ToList();

            await _context.UserRoles.AddRangeAsync(userRoles);
            var result = await _context.SaveChangesAsync();

            return result;
        }
    }
}

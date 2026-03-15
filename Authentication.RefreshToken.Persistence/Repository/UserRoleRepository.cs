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
        public async Task<List<UserRole>> AssignRolesAsync(int userId, List<int> roleIds)
        {
            var validRoles = await _context.Roles
                .Where(r => roleIds.Contains(r.Id) && r.IsActive)
                .Select(r => r.Id)
                .ToListAsync();

            if (!validRoles.Any())
                return new List<UserRole>();

            var userRoles = validRoles.Select(roleId => new UserRole
            {
                UserId = userId,
                RoleId = roleId,
            }).ToList();

            await _context.UserRoles.AddRangeAsync(userRoles);

            return userRoles;
        }

        public async Task UpdateRolesAsync(int userId, List<int> newRoleIds)
        {
            // A. Traer las relaciones actuales
            var currentUserRoles = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .ToListAsync();

            // B. Detectar qué ELIMINAR (Están en BD, pero NO en la nueva lista)
            var rolesToRemove = currentUserRoles
                .Where(ur => !newRoleIds.Contains(ur.RoleId))
                .ToList();

            // C. Detectar qué AGREGAR (Están en la nueva lista, pero NO en BD)
            var currentRoleIds = currentUserRoles.Select(ur => ur.RoleId).ToList();
            var roleIdsToAdd = newRoleIds
                .Except(currentRoleIds) // 'Except' nos da la diferencia
                .ToList();

            // D. Ejecutar Eliminación
            if (rolesToRemove.Any())
            {
                _context.UserRoles.RemoveRange(rolesToRemove);
            }

            // E. Ejecutar Inserción (Validando que existan y estén activos)
            if (roleIdsToAdd.Any())
            {
                var validRoles = await _context.Roles
                    .Where(r => roleIdsToAdd.Contains(r.Id) && r.IsActive)
                    .Select(r => r.Id)
                    .ToListAsync();

                var newUserRoles = validRoles.Select(roleId => new UserRole
                {
                    UserId = userId,
                    RoleId = roleId,
                    // CreatedOn / CreatedBy se maneja en SaveChanges si tienes auditoría
                });

                await _context.UserRoles.AddRangeAsync(newUserRoles);
            }
        }
    }
}

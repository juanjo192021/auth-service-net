using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Domain.Entities;
using Authentication.RefreshToken.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Authentication.RefreshToken.Persistence.Repository
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        protected readonly ApplicationDbContext _context;

        public RolePermissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RolePermission>> AssignPermissionAsync(int roleId, List<int> permissionIds)
        {
            var validPermission = await _context.Permissions
                .Where(r => permissionIds.Contains(r.Id) && r.IsActive)
                .Select(r => r.Id)
                .ToListAsync();

            if (!validPermission.Any())
                return new List<RolePermission>();

            var rolePermissions = validPermission.Select(permissionId => new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            }).ToList();

            await _context.RolePermissions.AddRangeAsync(rolePermissions);

            return rolePermissions;
        }

        public async Task UpdatePermissionsAsync(int roleId, List<int> newPermissionIds)
        {
            // 1. Traer las relaciones actuales de la BD para este Rol
            var currentRolePermissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .ToListAsync();

            // 2. Identificar qué eliminar (Están en BD pero NO en la nueva lista)
            var permissionsToRemove = currentRolePermissions
                .Where(rp => !newPermissionIds.Contains(rp.PermissionId))
                .ToList();

            // 3. Identificar qué agregar (Están en la nueva lista pero NO en BD)
            // Primero obtenemos los IDs que ya tenemos
            var currentPermissionIds = currentRolePermissions.Select(rp => rp.PermissionId).ToList();

            // Filtramos los nuevos que no estan en los actuales
            var permissionIdsToAdd = newPermissionIds
                .Except(currentPermissionIds) // "Except" saca la diferencia
                .ToList();

            // 4. Ejecutar Eliminación
            if (permissionsToRemove.Any())
            {
                _context.RolePermissions.RemoveRange(permissionsToRemove);
            }

            // 5. Ejecutar Inserción (validando que los permisos existan y estén activos)
            if (permissionIdsToAdd.Any())
            {
                var validPermissions = await _context.Permissions
                    .Where(p => permissionIdsToAdd.Contains(p.Id) && p.IsActive)
                    .Select(p => p.Id) // Solo necesitamos el ID
                    .ToListAsync();

                var newRolePermissions = validPermissions.Select(pId => new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = pId,
                });

                await _context.RolePermissions.AddRangeAsync(newRolePermissions);
            }
        }
    }
}

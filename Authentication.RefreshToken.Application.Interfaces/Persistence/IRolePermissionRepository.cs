using Authentication.RefreshToken.Domain.Entities;

namespace Authentication.RefreshToken.Application.Interfaces.Persistence
{
    public interface IRolePermissionRepository
    {
        Task<List<RolePermission>> AssignPermissionAsync(int roleId, List<int> permissionIds);
        Task UpdatePermissionsAsync(int roleId, List<int> newPermissionIds);
    }
}

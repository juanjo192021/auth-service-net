using Authentication.RefreshToken.Domain.Entities;

namespace Authentication.RefreshToken.Application.Interfaces.Persistence
{
    public interface IUserRoleRepository
    {
        Task<List<UserRole>> AssignRolesAsync(int userId, List<int> roleIds);
        Task UpdateRolesAsync(int userId, List<int> newRoleIds);
    }
}

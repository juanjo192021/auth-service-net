using Authentication.RefreshToken.Domain.Entities;

namespace Authentication.RefreshToken.Application.Interfaces.Persistence
{
    public interface IRoleRepository
    {
        Task<Role?> FindByNameAsync(string roleName);
    }
}

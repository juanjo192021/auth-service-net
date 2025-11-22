using Authentication.RefreshToken.Domain.Entities;

namespace Authentication.RefreshToken.Application.Interfaces.Persistence
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role?> GetByNameAsync(string roleName);
        Task<int> CountAsync();
    }
}

using Authentication.RefreshToken.Domain.Entities;

namespace Authentication.RefreshToken.Application.Interfaces.Persistence
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<bool> IsNameUniqueAsync(string name);
        Task<Role?> GetByNameAsync(string name);
        Task<IEnumerable<Role>> GetAllAsync();
    }
}

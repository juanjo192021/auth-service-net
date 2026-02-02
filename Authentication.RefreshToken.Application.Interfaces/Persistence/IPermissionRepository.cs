using Authentication.RefreshToken.Domain.Entities;

namespace Authentication.RefreshToken.Application.Interfaces.Persistence
{
    public interface IPermissionRepository : IGenericRepository<Permission>
    {
        Task<bool> IsNameUniqueAsync(string name);
        Task<IEnumerable<Permission>> GetAllAsync(int pageNumber, int pageSize, string search);
        Task<IEnumerable<Permission>> GetLookupAsync();
    }
}

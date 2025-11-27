using Authentication.RefreshToken.Domain.Entities;

namespace Authentication.RefreshToken.Application.Interfaces.Persistence
{
    public interface IMenuRepository : IGenericRepository<Menu>
    {
        Task<bool> IsRouteUniqueAsync(string route);
    }
}

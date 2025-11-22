using Authentication.RefreshToken.Domain.Entities;

namespace Authentication.RefreshToken.Application.Interfaces.Persistence
{
    public interface IUserRepository: IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}

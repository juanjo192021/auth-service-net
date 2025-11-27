using Authentication.RefreshToken.Domain.Entities;

namespace Authentication.RefreshToken.Application.Interfaces.Persistence
{
    public interface IUserRepository: IGenericRepository<User>
    {
        Task<bool> IsDocumentUniqueAsync(string documentNumber);
        Task<bool> IsEmailUniqueAsync(string email);
        Task<User?> GetWithRolesByEmailAsync(string email);
        Task<User?> GetWithRolesByIdAsync(int id);
    }
}

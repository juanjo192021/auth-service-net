namespace Authentication.RefreshToken.Application.Interfaces.Persistence
{
    public interface IUserRoleRepository
    {
        Task<int> CreateAsync(int userId, List<int> roleIds);
    }
}

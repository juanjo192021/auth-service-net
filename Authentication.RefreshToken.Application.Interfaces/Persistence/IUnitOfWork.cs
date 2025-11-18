namespace Authentication.RefreshToken.Application.Interfaces.Persistence
{
    public interface IUnitOfWork: IDisposable
    {
        IUserRepository Users { get; }
        IUserRoleRepository UserRoles { get; }
        IUserRefreshTokenRepository UserRefreshTokens { get; }
        IRoleRepository Roles { get; }

        // Este metodo nos va permitir persistir los cambios en la base de datos de manera atomica
        Task<int> Save(CancellationToken cancellationToken);
    }
}

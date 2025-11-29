namespace Authentication.RefreshToken.Application.Interfaces.Persistence
{
    public interface IUnitOfWork: IDisposable
    {
        IPermissionRepository Permissions { get; }
        IUserRepository Users { get; }
        IUserRoleRepository UserRoles { get; }
        IUserRefreshTokenRepository UserRefreshTokens { get; }
        IRoleRepository Roles { get; }
        IRolePermissionRepository RolePermissions { get; }

        // Este metodo nos va permitir persistir los cambios en la base de datos de manera atomica
        Task<int> CommitAsync(CancellationToken cancellationToken);
    }
}

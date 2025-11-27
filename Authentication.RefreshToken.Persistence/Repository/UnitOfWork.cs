using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Persistence.Contexts;

namespace Authentication.RefreshToken.Persistence.Repository
{
    public class UnitOfWork: IUnitOfWork
    {
        public IMenuRepository Menus { get; }
        public IPermissionRepository Permissions { get; }
        public IUserRepository Users { get; }
        public IUserRoleRepository UserRoles { get; }
        public IUserRefreshTokenRepository UserRefreshTokens { get; }
        public IRoleRepository Roles { get; }

        private readonly ApplicationDbContext _applicationDbContext;

        public UnitOfWork(
            IMenuRepository menus,
            IPermissionRepository permissions,
            IUserRepository users, 
            IUserRoleRepository userRoles, 
            IUserRefreshTokenRepository userRefreshTokens, 
            IRoleRepository roles, 
            ApplicationDbContext applicationDbContext)
        {
            Menus = menus;
            Permissions = permissions;
            Users = users;
            UserRoles = userRoles;
            UserRefreshTokens = userRefreshTokens;
            Roles = roles;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken)
        {
            // Vamos a persistir los cambios de manera atomica en la base de datos
            // Va a tomar todos los cambios que se han realizado en los repositorios
            // Este metodo se puede utilizar para hacer operaciones transaccionales como operaciones granulares
            return await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

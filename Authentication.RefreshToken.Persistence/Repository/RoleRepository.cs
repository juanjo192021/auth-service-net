using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Domain.Entities;
using Authentication.RefreshToken.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;


namespace Authentication.RefreshToken.Persistence.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Role?> FindByNameAsync(string roleName)
        {
            return await _context.Set<Role>()
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Name.Equals(roleName));
        }
    }
}

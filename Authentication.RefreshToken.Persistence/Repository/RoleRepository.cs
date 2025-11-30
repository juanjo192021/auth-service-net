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

        public async Task<int> CountAsync()
        {
            return await _context.Roles.CountAsync();
        }

        public async Task<Role?> CreateAsync(Role entity)
        {
            await _context.Roles.AddAsync(entity);
            return entity;
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var entity = await _context.Roles
                 .SingleOrDefaultAsync(x => x.Id.Equals(id));

            if (entity == null) return false;

            entity.IsActive = false;
            _context.Update(entity);
            return true;
        }

        public async Task<IEnumerable<Role>> GetAllAsync(int pageNumber, int pageSize, string search)
        {
            IQueryable<Role> query = _context.Roles.AsNoTracking()
                .Include(r => r.UserRoles!)
                    .ThenInclude(ur => ur.User)
                        .ThenInclude(u => u.CreatedByUser)
                .Include(r => r.UserRoles!)
                    .ThenInclude(ur => ur.User.UpdateByUser)
                .Include(r => r.UserRoles!)
                    .ThenInclude(ur => ur.User.DeactivatedByUser)

                .Include(r => r.RolePermissions!)
                    .ThenInclude(rp => rp.Permission)
                        .ThenInclude(p => p.CreatedByUser)
                .Include(r => r.RolePermissions!)
                    .ThenInclude(rp => rp.Permission.UpdateByUser)
                .Include(r => r.RolePermissions!)
                    .ThenInclude(rp => rp.Permission.DeactivatedByUser)

                .Include(r => r.CreatedByUser)
                .Include(r => r.UpdateByUser)
                .Include(r => r.DeactivatedByUser)
                .AsSplitQuery();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m => EF.Functions.Like(m.Name.ToLower(), $"%{search.ToLower()}%"))
                             .OrderBy(m => m.Id);
            }

            var data = await query.OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            return data;
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            var roles = await _context.Roles.AsNoTracking()
                .Include(r => r.UserRoles!)
                    .ThenInclude(ur => ur.User)
                        .ThenInclude(u => u.CreatedByUser)
                .Include(r => r.UserRoles!)
                    .ThenInclude(ur => ur.User.UpdateByUser)
                .Include(r => r.UserRoles!)
                    .ThenInclude(ur => ur.User.DeactivatedByUser)

                .Include(r => r.RolePermissions!)
                    .ThenInclude(rp => rp.Permission)
                        .ThenInclude(p => p.CreatedByUser)
                .Include(r => r.RolePermissions!)
                    .ThenInclude(rp => rp.Permission.UpdateByUser)
                .Include(r => r.RolePermissions!)
                    .ThenInclude(rp => rp.Permission.DeactivatedByUser)

                .Include(r => r.CreatedByUser)
                .Include(r => r.UpdateByUser)
                .Include(r => r.DeactivatedByUser)
                .AsSplitQuery()
                .ToListAsync();

            return roles;
        }

        public async Task<Role?> GetByIdAsync(int id)
        {
            return await _context.Roles.AsNoTracking()
                .Include(r => r.UserRoles!)
                    .ThenInclude(ur => ur.User)
                        .ThenInclude(u => u.CreatedByUser)
                .Include(r => r.UserRoles!)
                    .ThenInclude(ur => ur.User.UpdateByUser)
                .Include(r => r.UserRoles!)
                    .ThenInclude(ur => ur.User.DeactivatedByUser)

                .Include(r => r.RolePermissions!)
                    .ThenInclude(rp => rp.Permission)
                        .ThenInclude(p => p.CreatedByUser)
                .Include(r => r.RolePermissions!)
                    .ThenInclude(rp => rp.Permission.UpdateByUser)
                .Include(r => r.RolePermissions!)
                    .ThenInclude(rp => rp.Permission.DeactivatedByUser)

                .Include(r => r.CreatedByUser)
                .Include(r => r.UpdateByUser)
                .Include(r => r.DeactivatedByUser)
                .AsSplitQuery()
                .SingleOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<Role?> FindByIdAsync(int id)
        {
            return await _context.Roles
                .SingleOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> IsNameUniqueAsync(string name)
        {
            var existsRole = await _context.Roles
                .SingleOrDefaultAsync(x => x.Name.Equals(name));
            if (existsRole is not null) return false;
            
            return true;
        }

        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _context.Roles
                .SingleOrDefaultAsync(x => x.Name.Equals(name));
        }

        public async Task<Role?> UpdateAsync(Role entity)
        {
            _context.Roles.Update(entity);
            return await Task.FromResult<Role?>(entity);
        }
    }
}

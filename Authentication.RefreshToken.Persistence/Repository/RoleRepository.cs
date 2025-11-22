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
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var entity = await _context
                 .Roles
                 .SingleOrDefaultAsync(x => x.Id.Equals(id));
            if (entity == null) return false;

            entity.IsActive = false;

            _context.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Role>> GetAllAsync(int pageNumber, int pageSize, string search)
        {
            IQueryable<Role> query = _context.Roles
                .Include(r => r.UserRoles!)
                .ThenInclude(ur => ur.User);

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

        public async Task<Role?> GetByIdAsync(int id)
        {
            return await _context.Roles
                .AsNoTracking()
                .Include(r => r.UserRoles!)
                .ThenInclude(ur => ur.User)
                .SingleOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<Role?> GetByNameAsync(string roleName)
        {
            return await _context.Roles
                .AsNoTracking()
                .Include(r => r.UserRoles!)
                .ThenInclude(ur => ur.User)
                .SingleOrDefaultAsync(x => x.Name.Equals(roleName));
        }

        public async Task<Role?> UpdateAsync(Role entity)
        {
            var existing = await _context.Roles.FindAsync(entity.Id);
            if (existing is null) return null;

            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return existing;
        }

        //public async Task<Role?> ByNameAsync(string roleName)
        //{
        //    
        //}
    }
}

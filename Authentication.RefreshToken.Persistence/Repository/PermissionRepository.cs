using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Domain.Entities;
using Authentication.RefreshToken.Persistence.Contexts;
using Authentication.RefreshToken.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Authentication.RefreshToken.Persistence.Repository
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext _context;

        public PermissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountAsync()
        {
            return await _context.Permissions.CountAsync();
        }

        public async Task<Permission?> CreateAsync(Permission entity)
        {
            await _context.Permissions.AddAsync(entity);
            return entity;
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var entity = await _context.Permissions
                 .SingleOrDefaultAsync(x => x.Id.Equals(id));

            if (entity == null) return false;

            entity.IsActive = false;
            _context.Update(entity);
            return true;
        }

        public async Task<IEnumerable<Permission>> GetAllAsync(int pageNumber, int pageSize, string search)
        {
            IQueryable<Permission> query = _context.Permissions
                .ApplyFullIncludes()
                .AsSplitQuery();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m => EF.Functions.Like(m.Name.ToLower(), $"%{search.ToLower()}%"))
                             .OrderBy(m => m.Id);
            }

            return await query
                .OrderBy(x => x.Id)
                .Paginate(pageNumber, pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Permission>> GetLookupAsync()
        {
            return await _context.Permissions
                .AsNoTracking()
                .Select(p => new Permission
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    IsActive = p.IsActive
                })
                .ToListAsync();
        }

        public async Task<Permission?> GetByIdAsync(int id)
        {
            return await _context.Permissions
                //.Include(r => r.UserRoles!)
                //.ThenInclude(ur => ur.User)
                .SingleOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<Permission?> FindByIdAsync(int id)
        {
            return await _context.Permissions
                .SingleOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> IsNameUniqueAsync(string name)
        {
            var entity = await _context.Permissions
                .SingleOrDefaultAsync(x => x.Name.Equals(name));

            if (entity is not null) return false;

            return true;
        }

        public async Task<Permission?> UpdateAsync(Permission entity)
        {
            _context.Permissions.Update(entity);
            return await Task.FromResult<Permission?>(entity);
        }

        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            var permissions = await _context.Permissions
                .Include(r => r.CreatedByUser)
                .Include(r => r.UpdateByUser)
                .Include(r => r.DeactivatedByUser)
                .OrderBy(x => x.Id)
                .ToListAsync();

            return permissions;
        }
    }
}

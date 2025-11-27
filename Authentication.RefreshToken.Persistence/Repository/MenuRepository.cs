using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Domain.Entities;
using Authentication.RefreshToken.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Authentication.RefreshToken.Persistence.Repository
{
    public class MenuRepository : IMenuRepository
    {
        private readonly ApplicationDbContext _context;

        public MenuRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountAsync()
        {
            return await _context.Menus.CountAsync();
        }

        public async Task<Menu?> CreateAsync(Menu entity)
        {
            await _context.Menus.AddAsync(entity);
            return entity;
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var entity = await _context.Menus
                 .SingleOrDefaultAsync(x => x.Id.Equals(id));

            if (entity == null) return false;

            entity.IsActive = false;
            _context.Update(entity);
            return true;
        }

        public async Task<IEnumerable<Menu>> GetAllAsync(int pageNumber, int pageSize, string search)
        {
            IQueryable<Menu> query = _context.Menus;
                //.Include(r => r.UserRoles!)
                //.ThenInclude(ur => ur.User);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m => EF.Functions.Like(m.Title.ToLower(), $"%{search.ToLower()}%"))
                             .OrderBy(m => m.Id);
            }

            var data = await query.OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            return data;
        }

        public async Task<Menu?> GetByIdAsync(int id)
        {
            return await _context.Menus
                //.Include(r => r.UserRoles!)
                //.ThenInclude(ur => ur.User)
                .SingleOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<Menu?> FindByIdAsync(int id)
        {
            return await _context.Menus
                .SingleOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> IsRouteUniqueAsync(string route)
        {
            var entity = await _context.Menus
                .SingleOrDefaultAsync(x => x.Route.Equals(route));

            if (entity is not null) return false;

            return true;
        }

        public async Task<Menu?> UpdateAsync(Menu entity)
        {
            _context.Menus.Update(entity);
            return await Task.FromResult<Menu?>(entity);
        }
    }
}

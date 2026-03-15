using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Domain.Entities;
using Authentication.RefreshToken.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Authentication.RefreshToken.Persistence.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> CountAsync() => await _context.Users.CountAsync();
        
        public async Task<User?> CreateAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
            return entity;
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var entity = await _context.Users
                 .SingleOrDefaultAsync(x => x.Id.Equals(id));
            
            if (entity == null) return false;

            entity.IsActive = false;

            _context.Update(entity);
            return true;
        }

        public async Task<IEnumerable<User>> GetAllAsync(int pageNumber, int pageSize, string search)
        {
            IQueryable<User> query = _context.Users.AsNoTracking()
                .Include(u => u.CreatedByUser)
                .Include(u => u.UpdateByUser)
                .Include(u => u.DeactivatedByUser)

                .Include(u => u.UserRoles!)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.CreatedByUser)
                .Include(u => u.UserRoles!)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.UpdateByUser)
                .Include(u => u.UserRoles!)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.DeactivatedByUser)

                 .Include(r => r.UserRoles!)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .AsSplitQuery();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(
                    m => EF.Functions.Like(m.FirstName.ToLower(), $"%{search.ToLower()}%") || 
                    EF.Functions.Like(m.LastName.ToLower(), $"%{search.ToLower()}%"))
                    .OrderBy(m => m.Id);
            }

            var data = await query.OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            return data;
        }

        public async Task<bool> IsDocumentUniqueAsync(string documentNumber)
        {
            //var existsUser = await _context.Users
            //    .SingleOrDefaultAsync(x => x.DocumentNumber.Equals(documentNumber));

            //if (existsUser is not null) return false;

            return true;
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            var existsUser = await _context.Users
                .SingleOrDefaultAsync(x => x.Email.Equals(email));

            if (existsUser is not null) return false;

            return true;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<User?> FindByIdAsync(int id)
        {
            return await _context.Users
                .SingleOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<User?> GetWithRolesByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetWithRolesByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
            return await Task.FromResult<User?>(entity);
        }
    }
}

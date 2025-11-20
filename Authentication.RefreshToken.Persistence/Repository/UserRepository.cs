using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Domain.Entities;
using Authentication.RefreshToken.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Authentication.RefreshToken.Persistence.Repository
{
    internal class UserRepository : IUserRepository
    {
        protected readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<User?> CreateAsync(User entity)
        {
            await _context.AddAsync(entity);
            int rows = await _context.SaveChangesAsync();

            return (entity.Id > 0 && rows > 0) ? entity : null;
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var entity = await _context
                 .Set<User>() // Crea el objeto DbSet para la entidad Discount
                 .SingleOrDefaultAsync(x => x.Id.Equals(id)); // Busca la entidad por su Id
            if (entity == null) return false;

            entity.IsActive = false;
            entity.DeactivatedBy = entity.Id;

            _context.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<User>> GetAllAsync(int pageNumber, int pageSize, string search)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Set<User>()
                .AsNoTracking()
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(x => x.Email.Equals(email));
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Set<User>()
                .AsNoTracking()
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<User?> UpdateAsync(User user)
        {
            var entity = await _context
                 .Set<User>() // Crea el objeto DbSet para la entidad Discount
                 .AsNoTracking() // Indica que ignore el seguimiento de cambios para esta entidad
                 .SingleOrDefaultAsync(x => x.Id.Equals(user.Id)); // Busca la entidad por su Id
            
            if (entity == null) return null;

            entity.Email ??= user.Email;
            entity.LastName ??= user.LastName;
            entity.FirstName ??= user.FirstName;
            entity.ImageUrl ??= user.ImageUrl;
            entity.DocumentType ??= user.DocumentType;
            entity.DocumentNumber ??= user.DocumentNumber;
            entity.BirthDate ??= user.BirthDate;
            entity.Phone ??= user.Phone;
            entity.Mobile ??= user.Mobile;
            entity.Gender ??= user.Gender;
            entity.Address ??= user.Address;
            entity.IsActive = user.IsActive;
            entity.IsBlocked = user.IsBlocked;
            entity.UpdatedBy = user.UpdatedBy;

            _context.Update(entity);
            int rows = await _context.SaveChangesAsync();

            return (rows > 0) ? user : null;
        }
    }
}

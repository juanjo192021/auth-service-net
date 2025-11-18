namespace Authentication.RefreshToken.Application.Interfaces.Persistence
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> CreateAsync(T entity);
        Task<bool> DeactivateAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize, string search);
        Task<T?> GetByIdAsync(int id);
        Task<T?> UpdateAsync(T entity);
    }
}

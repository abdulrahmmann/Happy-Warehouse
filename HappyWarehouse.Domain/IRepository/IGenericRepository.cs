namespace HappyWarehouse.Domain.IRepository;

public interface IGenericRepository<T> where T: class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);

    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
}
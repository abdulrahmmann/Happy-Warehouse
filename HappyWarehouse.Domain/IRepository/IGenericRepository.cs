namespace HappyWarehouse.Domain.IRepository;

public interface IGenericRepository<T> where T: class
{
    /// <summary> Method to Get All Entity Records. </summary>
    Task<IEnumerable<T>> GetAllAsync();
    
    /// <summary> Method to Get All Entity By Id. </summary>
    Task<T> GetByIdAsync(int id);

    /// <summary> Method to Add Entity Record. </summary>
    Task AddAsync(T entity);
    
    /// <summary> Method to Add Entity Records Range. </summary>
    Task AddRangeAsync(IEnumerable<T> entities);
}
using HappyWarehouse.Domain.IRepository;
using HappyWarehouse.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HappyWarehouse.Infrastructure.Repository;

public class GenericRepository<T>: IGenericRepository<T> where T : class
{
    #region Instance Fields

    private readonly ApplicationDbContext _dbContext;
    private readonly DbSet<T> _dbSet;
    #endregion

    #region Constructor
    public GenericRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }
    #endregion
    
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return (await _dbSet.FindAsync(id))!;
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbContext.AddRangeAsync(entities);
    }
}
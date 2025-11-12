using System.Data;
using HappyWarehouse.Domain.IRepository;
using HappyWarehouse.Infrastructure.Context;
using HappyWarehouse.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore.Storage;

namespace HappyWarehouse.Infrastructure.UOF;

public class UnitOfWork: IUnitOfWork
{
    #region Instance Fields
    private readonly ApplicationDbContext  _dbContext;
    private readonly Dictionary<Type, object> _repositories;
    public ICountryRepository GetCountryRepository { get; }
    public IWarehouseRepository GetWarehouseRepository { get; }
    public IWarehouseItemRepository GetWarehouseItemRepository { get; }
    public ApplicationDbContext GetDbContext { get; }
    #endregion

    #region Constructor
    public UnitOfWork(ApplicationDbContext dbContext, ApplicationDbContext context, ICountryRepository getCountryRepository, 
        IWarehouseRepository getWarehouseRepository, IWarehouseItemRepository getWarehouseItemRepository)
    {
        _dbContext = dbContext;
        GetDbContext = context;
        GetCountryRepository = getCountryRepository;
        GetWarehouseRepository = getWarehouseRepository;
        GetWarehouseItemRepository = getWarehouseItemRepository;
        _repositories = new Dictionary<Type, object>();
    }
    #endregion
    
    public IGenericRepository<T> GetRepository<T>() where T : class
    {
        var type = typeof(T);
        
        if (!_repositories.ContainsKey(type))
        {
            var repoInstance = new GenericRepository<T>(_dbContext);
            _repositories[type] = repoInstance;
        }

        return (IGenericRepository<T>)_repositories[type];
    }

    public IDbTransaction BeginTransaction()
    {
        return _dbContext.Database.BeginTransaction().GetDbTransaction();
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
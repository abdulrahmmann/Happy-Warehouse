using System.Data;
using HappyWarehouse.Domain.IRepository;
using HappyWarehouse.Infrastructure.Context;

namespace HappyWarehouse.Infrastructure.UOF;

public interface IUnitOfWork: IDisposable
{
    IGenericRepository<T> GetRepository<T>() where T : class;
    
    ICountryRepository GetCountryRepository { get; }
    
    IWarehouseRepository GetWarehouseRepository { get; }
    
    IWarehouseItemRepository GetWarehouseItemRepository { get; }
    
    IDashboardRepository GetDashboardRepository { get; }
    
    ApplicationDbContext GetDbContext { get; }
    
    IDbTransaction BeginTransaction();
    
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
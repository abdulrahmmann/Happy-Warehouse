using System.Linq.Expressions;
using HappyWarehouse.Domain.Entities;

namespace HappyWarehouse.Domain.IRepository;

public interface IWarehouseRepository: IGenericRepository<Warehouse>
{
    /// <summary> Method to Update Warehouse. </summary>
    Task UpdateAsync(Warehouse warehouse, string? modifiedBy = null);

    /// <summary> Method to Softly Delete Warehouse. </summary>
    Task SoftlyDeleteAsync(int id, string? deletedBy = null);
    
    /// <summary> Method to Restore Deleted Warehouse. </summary>
    Task RestoreAsync(int id, string? restoredBy = null);
    
    /// <summary> Method to Add Item to Warehouse. </summary>
    Task<WarehouseItem> AddItemAsync(int warehouseId, string itemName, string? skuCode, int qty, decimal costPrice, decimal? msrpPrice, int? createdByUserId, string? createdByUser);
    
    Task<IEnumerable<Warehouse>> GetAllWithItemsAsync(CancellationToken cancellationToken = default);
    
    public Task<Warehouse> FirstOrDefaultAsync(Expression<Func<Warehouse, bool>> predicate, CancellationToken cancellationToken);
    public Task<Warehouse> FirstOrDefaultAsyncWithIgnoreQueryFilter(Expression<Func<Warehouse, bool>> predicate, CancellationToken cancellationToken);
}
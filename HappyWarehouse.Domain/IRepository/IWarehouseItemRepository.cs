using System.Linq.Expressions;
using HappyWarehouse.Domain.Entities;

namespace HappyWarehouse.Domain.IRepository;

public interface IWarehouseItemRepository: IGenericRepository<WarehouseItem>
{
    /// <summary> Method to Add Items to a specific warehouse. </summary>
    Task AddItemAsync(WarehouseItem item, string? createdBy = null);
    
    /// <summary> Method to Update Warehouse Item. </summary>
    Task UpdateAsync(WarehouseItem item, string? modifiedBy = null);
    
    /// <summary> Method to Softly Delete Warehouse Item. </summary>
    Task SoftlyDeleteAsync(int id, string? deletedBy = null);
    
    /// <summary> Method to Add Item to Warehouse Item. </summary>
    Task RestoreAsync(int id, string? restoredBy = null);
    
    public Task<WarehouseItem> FirstOrDefaultAsync(Expression<Func<WarehouseItem, bool>> predicate, CancellationToken cancellationToken);
    public Task<WarehouseItem> FirstOrDefaultAsyncWithIgnoreQueryFilter(Expression<Func<WarehouseItem, bool>> predicate, CancellationToken cancellationToken);
}
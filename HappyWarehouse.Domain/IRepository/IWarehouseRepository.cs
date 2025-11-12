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
    
    public Task<Country> FirstOrDefaultAsync(Expression<Func<Country, bool>> predicate, CancellationToken cancellationToken);
}
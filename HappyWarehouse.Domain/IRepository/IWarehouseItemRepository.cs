using HappyWarehouse.Domain.Entities;

namespace HappyWarehouse.Domain.IRepository;

public interface IWarehouseItemRepository: IGenericRepository<WarehouseItem>
{
    Task AddItemAsync(WarehouseItem item, string? createdBy = null);
    
    Task UpdateAsync(WarehouseItem item, string? modifiedBy = null);
    
    Task SoftlyDelete(int id, string? deletedBy = null);
    
    Task Restore(int id, string? restoredBy = null);
}
using System.Linq.Expressions;
using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Domain.IRepository;
using HappyWarehouse.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HappyWarehouse.Infrastructure.Repository;

public class WarehouseItemRepository: GenericRepository<WarehouseItem>, IWarehouseItemRepository
{
    #region Instance Fields
    private readonly ApplicationDbContext _dbContext;
    #endregion

    #region Constructor
    public WarehouseItemRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    #endregion
    
    public async Task AddItemAsync(WarehouseItem item, string? createdBy = null)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        var warehouseExists = await _dbContext.Warehouses.AnyAsync(w => w.Id == item.WarehouseId);
        if (!warehouseExists)
            throw new KeyNotFoundException($"Warehouse with ID {item.WarehouseId} not found.");

        await _dbContext.WarehouseItems.AddAsync(item);
    }

    public async Task UpdateAsync(WarehouseItem item, string? modifiedBy = null)
    {
        var existingItem = await _dbContext.WarehouseItems.FirstOrDefaultAsync(i => i.Id == item.Id);

        if (existingItem == null)
            throw new KeyNotFoundException($"WarehouseItem with ID {item.Id} not found.");

        existingItem.Update(item.ItemName, item.Qty, item.CostPrice, item.MsrpPrice, item.SkuCode, modifiedBy);
    }

    public async Task SoftlyDeleteAsync(int id, string? deletedBy = null)
    {
        var existingItem = await _dbContext.WarehouseItems.FirstOrDefaultAsync(i => i.Id == id);

        if (existingItem == null)
            throw new KeyNotFoundException($"WarehouseItem with ID {id} not found.");

        existingItem.SoftDelete(deletedBy);
    }

    public async Task RestoreAsync(int id, string? restoredBy = null)
    {
        var existingItem = await _dbContext.WarehouseItems.FirstOrDefaultAsync(i => i.Id == id);

        if (existingItem == null)
            throw new KeyNotFoundException($"WarehouseItem with ID {id} not found.");

        existingItem.Restore(restoredBy);
    }

    public async Task<WarehouseItem> FirstOrDefaultAsync(Expression<Func<WarehouseItem, bool>> predicate, CancellationToken cancellationToken)
    {
        return (await _dbContext.WarehouseItems.FirstOrDefaultAsync(predicate, cancellationToken))!;
    }

    public async Task<WarehouseItem> FirstOrDefaultAsyncWithIgnoreQueryFilter(Expression<Func<WarehouseItem, bool>> predicate, CancellationToken cancellationToken)
    {
        return (await _dbContext.WarehouseItems.IgnoreQueryFilters().FirstOrDefaultAsync(predicate, cancellationToken))!;
    }
}
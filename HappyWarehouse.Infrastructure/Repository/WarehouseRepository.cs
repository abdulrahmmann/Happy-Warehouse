using System.Linq.Expressions;
using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Domain.IRepository;
using HappyWarehouse.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HappyWarehouse.Infrastructure.Repository;

public class WarehouseRepository: GenericRepository<Warehouse>, IWarehouseRepository
{
    #region Instance Fields
    private readonly ApplicationDbContext _dbContext;
    #endregion

    #region Constructor
    public WarehouseRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    #endregion
    
    public async Task UpdateAsync(Warehouse warehouse, string? modifiedBy = null)
    {
        var existingWarehouse = await _dbContext.Warehouses.FirstOrDefaultAsync(c => c.Id == warehouse.Id);

        if (existingWarehouse == null) throw new KeyNotFoundException($"Warehouse with ID {warehouse.Id} not found.");
        
        existingWarehouse.Update(warehouse.Name, warehouse.Address, warehouse.City, warehouse.CountryId, modifiedBy);
    }

    public async Task SoftlyDeleteAsync(int id, string? deletedBy = null)
    {
        var existingWarehouse = await _dbContext.Warehouses.FirstOrDefaultAsync(c => c.Id == id);

        if (existingWarehouse == null) throw new KeyNotFoundException($"Warehouse with ID {id} not found."); 
        
        existingWarehouse.SoftDelete(deletedBy);
    }

    public async Task RestoreAsync(int id, string? restoredBy = null)
    {
        var existingWarehouse = await _dbContext.Warehouses.FirstOrDefaultAsync(c => c.Id == id);

        if (existingWarehouse == null) throw new KeyNotFoundException($"Warehouse with ID {id} not found."); 
        
        existingWarehouse.Restore(restoredBy);
    }

    public async Task<WarehouseItem> AddItemAsync(int warehouseId, string itemName, string? skuCode, int qty, decimal costPrice, decimal? msrpPrice, int? createdByUserId, string? createdByUser)
    {
        var warehouse = await _dbContext.Warehouses
            .Include(w => w.WarehouseItems)
            .FirstOrDefaultAsync(w => w.Id == warehouseId);

        if (warehouse == null)
            throw new KeyNotFoundException($"Warehouse with ID {warehouseId} not found.");

        var newItem = warehouse.AddWarehouseItem(
            itemName,
            skuCode,
            qty,
            costPrice,
            msrpPrice,
            warehouseId,
            createdByUserId,
            createdByUser
        );

        return newItem;
    }

    public async Task<IEnumerable<Warehouse>> GetAllWithItemsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Warehouses
            .Include(c => c.Country)
            .Include(wi => wi.WarehouseItems)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public IQueryable<Warehouse> GetAllQueryable()
    {
        return _dbContext.Warehouses.Include(w => w.Country).Include(w => w.CreatedByUser).Where(x => !x.IsDeleted).AsNoTracking();
    }
    
    public async Task<Warehouse> FirstOrDefaultAsync(Expression<Func<Warehouse, bool>> predicate, CancellationToken cancellationToken)
    {
        return (await _dbContext.Warehouses.FirstOrDefaultAsync(predicate, cancellationToken))!;
    }

    public async Task<Warehouse> FirstOrDefaultAsyncWithIgnoreQueryFilter(Expression<Func<Warehouse, bool>> predicate, CancellationToken cancellationToken)
    {
        return (await _dbContext.Warehouses.IgnoreQueryFilters().FirstOrDefaultAsync(predicate, cancellationToken))!;
    }

    public async Task<Warehouse> GetWarehouseByIdAsync(int id, CancellationToken cancellationToken)
    {
        return (await _dbContext.Warehouses.Include(w => w.Country)
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken))!;
    }
}
using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Domain.IRepository;
using HappyWarehouse.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HappyWarehouse.Infrastructure.Repository;

public class DashboardRepository(ApplicationDbContext dbContext): IDashboardRepository
{
    public async Task<List<Warehouse>> GetWarehouseStatusAsync()
    {
        return await dbContext.Warehouses
            .Include(w => w.WarehouseItems)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<WarehouseItem>> GetTopItemsAsync()
    {
        return await dbContext.WarehouseItems
            .Include(w => w.Warehouse)
            .AsNoTracking()
            .ToListAsync();
    }
    
    public async Task<List<Warehouse>> GetWarehouseWithCountInventoryAsync()
    {
        return await dbContext.Warehouses
            .Include(w => w.Country)
            .Include(w => w.WarehouseItems)
            .AsNoTracking()
            .ToListAsync();
    }
}
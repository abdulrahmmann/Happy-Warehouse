using HappyWarehouse.Domain.Entities;

namespace HappyWarehouse.Domain.IRepository;

public interface IDashboardRepository
{
    Task<List<Warehouse>> GetWarehouseStatusAsync();
    Task<List<WarehouseItem>> GetTopItemsAsync();
}
namespace HappyWarehouse.Application.Features.DashboardFeature.DTOs;

public record WarehouseCountInventoryStatusDto(int WarehouseId, string WarehouseName, string WarehouseCountry, string WarehouseCity, int ItemsCount, int QtyCounts);
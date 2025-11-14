namespace HappyWarehouse.Application.Features.DashboardFeature.DTOs;

public record WarehouseTopItemsDto(string WarehouseName, string ItemName, int Qty, string? SkuCode);
namespace HappyWarehouse.Application.Features.WarehouseItemFeature.DTOs;

public record CreateWarehouseItemDto(string ItemName, string? SkuCode, int Qty, decimal CostPrice, decimal? MsrpPrice, 
    int WarehouseId, int CreatedByUserId, string? CreatedBy);
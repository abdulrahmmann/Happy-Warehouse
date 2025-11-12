namespace HappyWarehouse.Application.Features.WarehouseItemFeature.DTOs;

public record WarehouseItemDto(int Id, string ItemName, string? SkuCode, int Qty, decimal CostPrice, decimal? MsrpPrice,
    int WarehouseId, int? CreatedByUserId, string? CreatedByUserName);
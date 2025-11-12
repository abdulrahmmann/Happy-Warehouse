namespace HappyWarehouse.Application.Features.WarehouseFeature.DTOs;

public record CreateWarehouseDto(string Name, string Address, string City, int CountryId, int CreatedByUserId, string? CreatedBy);
namespace HappyWarehouse.Application.Features.WarehouseFeature.DTOs;

public record UpdateWarehouseDto(string Name, string Address, string City, int CountryId, string? UpdatedBy);
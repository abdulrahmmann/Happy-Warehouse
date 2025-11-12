namespace HappyWarehouse.Application.Features.WarehouseFeature.DTOs;

public record WarehouseDto(int Id, string Name, string Address, string City, int CountryId, int CreatedByUserId, 
    DateTime? CreatedAt, string? CreatedBy, DateTime? ModifiedAt,  string? ModifiedBy);
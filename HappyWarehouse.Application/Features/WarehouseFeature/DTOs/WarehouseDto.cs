namespace HappyWarehouse.Application.Features.WarehouseFeature.DTOs;

public record WarehouseDto(int Id, string Name, string Address, string City, string CountryName, string CreatedByUser);
public record WarehouseDto2(int Id, string Name, string Address, string City, string CountryName, int  CreatedByUserId);

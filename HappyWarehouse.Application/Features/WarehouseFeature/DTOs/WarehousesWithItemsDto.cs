using HappyWarehouse.Application.Features.WarehouseItemFeature.DTOs;

namespace HappyWarehouse.Application.Features.WarehouseFeature.DTOs;

public record WarehousesWithItemsDto(
    int Id,
    string Name,
    string Address,
    string City,
    string CountryName,
    List<WarehouseItemDto> Items);
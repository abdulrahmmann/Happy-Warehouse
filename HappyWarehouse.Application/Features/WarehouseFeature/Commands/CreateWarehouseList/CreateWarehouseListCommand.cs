using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.WarehouseFeature.DTOs;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.WarehouseFeature.Commands.CreateWarehouseList;

public record CreateWarehouseListCommand(IEnumerable<CreateWarehouseDto> WarehousesDto): ICommand<BaseResponse<string>>;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.WarehouseFeature.DTOs;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.WarehouseFeature.Commands.CreateWarehouse;

public record CreateWarehouseCommand(CreateWarehouseDto WarehouseDto): ICommand<BaseResponse<string>>;
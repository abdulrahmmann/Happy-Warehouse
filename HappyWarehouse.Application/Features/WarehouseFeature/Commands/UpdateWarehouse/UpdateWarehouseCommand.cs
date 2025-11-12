using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.WarehouseFeature.DTOs;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.WarehouseFeature.Commands.UpdateWarehouse;

public record UpdateWarehouseCommand(int Id, UpdateWarehouseDto WarehouseDto): ICommand<BaseResponse<string>>;
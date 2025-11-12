using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.WarehouseItemFeature.DTOs;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.WarehouseItemFeature.Commands.CreateWarehouseItem;

public record CreateWarehouseItemCommand(CreateWarehouseItemDto ItemDto): ICommand<BaseResponse<string>>;
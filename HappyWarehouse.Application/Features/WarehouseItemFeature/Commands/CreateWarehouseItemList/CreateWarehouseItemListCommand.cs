using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.WarehouseItemFeature.DTOs;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.WarehouseItemFeature.Commands.CreateWarehouseItemList;

public record CreateWarehouseItemListCommand(IEnumerable<CreateWarehouseItemDto> ItemsDto): ICommand<BaseResponse<string>>;
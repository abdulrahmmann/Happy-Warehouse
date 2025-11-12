using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.WarehouseItemFeature.DTOs;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.WarehouseItemFeature.Queries.GetItemByWarehouseId;

public record GetItemsByWarehouseIdQuery(int WarehouseId): IQuery<BaseResponse<IEnumerable<WarehouseItemDto>>>;
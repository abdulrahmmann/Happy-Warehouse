using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.WarehouseItemFeature.DTOs;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.WarehouseItemFeature.Queries.GetItemByWarehouseId;

public record GetItemsByWarehouseIdQuery(int WarehouseId, int PageNumber = 1, int PageSize = 10)
    : IQuery<BaseResponse<IEnumerable<WarehouseItemDto>>>;
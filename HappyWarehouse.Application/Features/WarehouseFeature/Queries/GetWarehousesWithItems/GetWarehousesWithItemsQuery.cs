using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.WarehouseFeature.DTOs;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.WarehouseFeature.Queries.GetWarehousesWithItems;

public record GetWarehousesWithItemsQuery(): IQuery<BaseResponse<IEnumerable<WarehousesWithItemsDto>>>;
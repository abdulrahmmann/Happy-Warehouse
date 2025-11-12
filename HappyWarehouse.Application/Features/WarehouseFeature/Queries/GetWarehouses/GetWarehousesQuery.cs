using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.WarehouseFeature.DTOs;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.WarehouseFeature.Queries.GetWarehouses;

public record GetWarehousesQuery(): IQuery<BaseResponse<IEnumerable<WarehouseDto>>>;
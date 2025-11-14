using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.WarehouseFeature.DTOs;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.WarehouseFeature.Queries.GetWarehouseById;

public record GetWarehouseByIdQuery(int Id): IQuery<BaseResponse<WarehouseDto2>>;
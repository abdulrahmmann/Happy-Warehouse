using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.DashboardFeature.DTOs;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.DashboardFeature.Queries.GetWarehouseWithCountInventory;

public record GetWarehouseWithCountInventoryQuery(int Page = 1, int PageSize = 10)
    : IQuery<BaseResponse<List<WarehouseCountInventoryStatusDto>>>;
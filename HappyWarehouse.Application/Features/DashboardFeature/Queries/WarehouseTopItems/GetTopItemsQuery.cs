using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.DashboardFeature.DTOs;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.DashboardFeature.Queries.WarehouseTopItems;

public record GetTopItemsQuery(int Top = 10): IQuery<BaseResponse<List<WarehouseTopItemsDto>>>;
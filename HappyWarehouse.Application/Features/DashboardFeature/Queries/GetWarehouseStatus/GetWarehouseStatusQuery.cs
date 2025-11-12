using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.DashboardFeature.DTOs;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.DashboardFeature.Queries.GetWarehouseStatus;

public record GetWarehouseStatusQuery(): IQuery<BaseResponse<List<WarehouseStatusDto>>>;
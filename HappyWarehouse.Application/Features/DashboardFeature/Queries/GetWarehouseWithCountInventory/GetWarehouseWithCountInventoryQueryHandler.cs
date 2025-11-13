using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.DashboardFeature.DTOs;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Infrastructure.UOF;
using Serilog;

namespace HappyWarehouse.Application.Features.DashboardFeature.Queries.GetWarehouseWithCountInventory;

public class GetWarehouseWithCountInventoryQueryHandler(IUnitOfWork unitOfWork, ILogger logger)
    : IQueryHandler<GetWarehouseWithCountInventoryQuery, BaseResponse<List<WarehouseCountInventoryStatusDto>>>
{
    public async Task<BaseResponse<List<WarehouseCountInventoryStatusDto>>> HandleAsync(GetWarehouseWithCountInventoryQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            var warehouses = await unitOfWork.GetDashboardRepository.GetWarehouseWithCountInventoryAsync();

            if (warehouses.Count == 0)
            {
                logger.Information("no warehouses found");
                return BaseResponse<List<WarehouseCountInventoryStatusDto>>.NoContent("no warehouses found");
            }
            
            var warehousesMapped = warehouses
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(w => new WarehouseCountInventoryStatusDto
                (w.Id, w.Name, w.Country.Name, w.City, w.WarehouseItems.Count, w.WarehouseItems.Sum(wi => wi.Qty))).ToList();
            
            return BaseResponse<List<WarehouseCountInventoryStatusDto>>
                .Success(data:warehousesMapped, message:"warehouses with items, quantity counts retrieved successfully");
        }
        catch (Exception e)
        {
            logger.Error("Internal error", e);
            return BaseResponse<List<WarehouseCountInventoryStatusDto>>.InternalError("Internal error");
        }
    }
}
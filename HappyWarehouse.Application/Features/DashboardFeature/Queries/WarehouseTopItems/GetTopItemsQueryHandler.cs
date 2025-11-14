using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.DashboardFeature.DTOs;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Infrastructure.UOF;

namespace HappyWarehouse.Application.Features.DashboardFeature.Queries.WarehouseTopItems;

public class GetTopItemsQueryHandler(IUnitOfWork unitOfWork)
    : IQueryHandler<GetTopItemsQuery, BaseResponse<List<WarehouseTopItemsDto>>>
{
    public async Task<BaseResponse<List<WarehouseTopItemsDto>>> HandleAsync(GetTopItemsQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            var items = await unitOfWork.GetDashboardRepository.GetTopItemsAsync();

            var warehouseTopItems = items.OrderByDescending(i => i.Qty)
                .Take(query.Top)
                .Select(x => new WarehouseTopItemsDto(x.Warehouse.Name, x.ItemName, x.Qty, x.SkuCode))
                .ToList();
            
            return BaseResponse<List<WarehouseTopItemsDto>>
                .Success(totalCount:warehouseTopItems.Count ,data:warehouseTopItems, message:"top items retrieved successfully");

        }
        catch (Exception e)
        {
            return BaseResponse<List<WarehouseTopItemsDto>>.InternalError();
        }
    }
}
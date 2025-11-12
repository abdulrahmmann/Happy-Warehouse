using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.DashboardFeature.DTOs;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Infrastructure.UOF;
using Microsoft.Extensions.Logging;

namespace HappyWarehouse.Application.Features.DashboardFeature.Queries.WarehouseTopItems;

public class GetTopHighItemsQueryHandler(IUnitOfWork unitOfWork)
    : IQueryHandler<GetTopHighItemsQuery, BaseResponse<List<WarehouseTopItemsDto>>>
{
    public async Task<BaseResponse<List<WarehouseTopItemsDto>>> HandleAsync(GetTopHighItemsQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            var items = await unitOfWork.GetDashboardRepository.GetTopItemsAsync();

            var warehouseTopItems =  items.OrderByDescending(i => i.Qty)
                .Take(query.Top)
                .Select(i => new WarehouseTopItemsDto(i.ItemName, i.Warehouse.Name, i.Qty))
                .ToList();
            
            return BaseResponse<List<WarehouseTopItemsDto>>.Success(warehouseTopItems);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.WarehouseFeature.DTOs;
using HappyWarehouse.Application.Features.WarehouseItemFeature.DTOs;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Infrastructure.UOF;
using Serilog;

namespace HappyWarehouse.Application.Features.WarehouseFeature.Queries.GetWarehousesWithItems;

public class GetWarehousesWithItemsQueryHandler(IUnitOfWork unitOfWork, ILogger logger)
    : IQueryHandler<GetWarehousesWithItemsQuery, BaseResponse<IEnumerable<WarehousesWithItemsDto>>>
{
    public async Task<BaseResponse<IEnumerable<WarehousesWithItemsDto>>> HandleAsync(GetWarehousesWithItemsQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            var warehouses = await unitOfWork.GetWarehouseRepository.GetAllWithItemsAsync(cancellationToken);
            
            var result = warehouses.Select(w =>
                new WarehousesWithItemsDto(
                    w.Id,
                    w.Name,
                    w.Address,
                    w.City,
                    w.Country?.Name ?? "Unknown",
                    w.WarehouseItems.Select(item =>
                        new WarehouseItemDto(
                            item.Id,
                            item.ItemName,
                            item.SkuCode,
                            item.Qty,
                            item.CostPrice,
                            item.MsrpPrice,
                            item.WarehouseId,
                            item.CreatedByUserId,
                            item.CreatedBy
                        )
                    ).ToList()
                )
            ).ToList();

            return BaseResponse<IEnumerable<WarehousesWithItemsDto>>.Success(result);
        }
        catch (Exception e)
        {
            logger.Error(e, "Error occurred while fetching warehouses with items");
            return BaseResponse<IEnumerable<WarehousesWithItemsDto>>.InternalError("Error occurred while fetching warehouses with items");
        }
    }
}
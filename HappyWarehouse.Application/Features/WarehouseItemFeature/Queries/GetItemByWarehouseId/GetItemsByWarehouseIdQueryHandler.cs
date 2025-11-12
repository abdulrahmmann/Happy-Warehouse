using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.WarehouseItemFeature.DTOs;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Infrastructure.UOF;
using Serilog;

namespace HappyWarehouse.Application.Features.WarehouseItemFeature.Queries.GetItemByWarehouseId;

public class GetItemsByWarehouseIdQueryHandler(IUnitOfWork unitOfWork, ILogger logger)
    : IQueryHandler<GetItemsByWarehouseIdQuery, BaseResponse<IEnumerable<WarehouseItemDto>>>
{
    public async Task<BaseResponse<IEnumerable<WarehouseItemDto>>> HandleAsync(GetItemsByWarehouseIdQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            if (query.WarehouseId <= 0)
            {
                logger.Warning("Warehouse Id must be greater than zero");
                return BaseResponse<IEnumerable<WarehouseItemDto>>.ValidationError("Warehouse Id must be greater than zero");
            }
            
            var items = await unitOfWork.GetWarehouseItemRepository.GetAllItemsByWarehouseIdAsync(query.WarehouseId, cancellationToken);

            var warehouseItems = items.ToList();

            if (warehouseItems.Count == 0)
            {
                logger.Warning("Warehouse Items is empty");
                return BaseResponse<IEnumerable<WarehouseItemDto>>.NoContent("Warehouse Items are empty");
            }
            
            var mappedItems = warehouseItems.Select(item => new WarehouseItemDto(
                item.Id, 
                item.ItemName,
                item.SkuCode, 
                item.Qty, 
                item.CostPrice, 
                item.MsrpPrice,
                item.WarehouseId, 
                item.CreatedByUserId, 
                item.CreatedBy
            )).ToList();
            
            logger.Information("Warehouse Items with warehouse Id: {WarehouseId} are successfully retrieved", query.WarehouseId);
            
            return BaseResponse<IEnumerable<WarehouseItemDto>>.Success(totalCount: warehouseItems.Count, data:mappedItems,
                message:$"Warehouse Items with warehouse Id: {query.WarehouseId} are successfully retrieved");
        }
        catch (Exception e)
        {
            logger.Information("Unexpected server error. Please try again later");
            return BaseResponse<IEnumerable<WarehouseItemDto>>.InternalError("Unexpected server error. Please try again later");
        }
    }
}
using HappyWarehouse.Application.Common;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Infrastructure.UOF;

namespace HappyWarehouse.Application.Features.WarehouseItemFeature.Commands.CreateWarehouseItemList;

public class CreateWarehouseItemListCommandHandler(IUnitOfWork unitOfWork): ICommandHandler<CreateWarehouseItemListCommand, BaseResponse<string>>
{
    public async Task<BaseResponse<string>> HandleAsync(CreateWarehouseItemListCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var warehouseItems = command.ItemsDto.Select(request => new WarehouseItem(
                request.ItemName,
                request.SkuCode,
                request.Qty,
                request.CostPrice,
                request.MsrpPrice,
                request.WarehouseId,
                request.CreatedByUserId,
                request.CreatedBy
                ));

            var enumerable = warehouseItems.ToList();
            enumerable.Select(e => e.CreatedAt = DateTime.UtcNow);
                
            await unitOfWork.GetWarehouseItemRepository.AddRangeAsync(enumerable);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            
            return BaseResponse<string>.Created($"Items created");
        }
        catch (Exception e)
        {
            return BaseResponse<string>.InternalError("Unexpected server error. Please try again later");
        }
    }
}
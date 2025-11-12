using FluentValidation;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.WarehouseItemFeature.DTOs;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Infrastructure.UOF;
using Serilog;

namespace HappyWarehouse.Application.Features.WarehouseItemFeature.Commands.CreateWarehouseItem;

public class CreateWarehouseItemCommandHandler(IUnitOfWork unitOfWork, IValidator<CreateWarehouseItemDto> validator, ILogger logger)
    : ICommandHandler<CreateWarehouseItemCommand, BaseResponse<string>>
{
    public async Task<BaseResponse<string>> HandleAsync(CreateWarehouseItemCommand command, CancellationToken cancellationToken = default)
    {
        var request = command.ItemDto;
        
        using var transaction = unitOfWork.BeginTransaction();
        
        try
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
                logger.Warning("validation error: {error}", validationErrors);
                return BaseResponse<string>.ValidationError(validationErrors);
            }
            
            var existingWareHouse = await unitOfWork.GetWarehouseItemRepository
                .FirstOrDefaultAsync(w => w.ItemName == request.ItemName, cancellationToken);

            if (existingWareHouse is not null)
            {
                logger.Warning("Warehouse Item with name '{name}' is already exists.", request.ItemName);
                return BaseResponse<string>.Conflict($"Warehouse Item '{request.ItemName}' is already exists.");
            }
            
            var duplicateItem = await unitOfWork.GetWarehouseItemRepository
                .FirstOrDefaultAsync(i => i.ItemName.ToLower() == request.ItemName.ToLower(), cancellationToken);

            if (duplicateItem != null)
            {
                logger.Warning("Item '{itemName}' already exists in Warehouse {warehouseId}.", request.ItemName, request.WarehouseId);
                return BaseResponse<string>.Conflict($"Item '{request.ItemName}' already exists in this warehouse.");
            }
            
            var warehouseItem = new WarehouseItem(
                request.ItemName,
                request.SkuCode,
                request.Qty,
                request.CostPrice,
                request.MsrpPrice,
                request.WarehouseId,
                request.CreatedByUserId,
                request.CreatedBy
            );
            warehouseItem.CreatedAt = DateTime.UtcNow;
            
            await unitOfWork.GetWarehouseItemRepository.AddAsync(warehouseItem);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            transaction.Commit();
            
            logger.Information("Warehouse item '{itemName}' created successfully in Warehouse {warehouseId}.", request.ItemName, request.WarehouseId);
            return BaseResponse<string>.Success($"Item '{request.ItemName}' added successfully to warehouse {request.WarehouseId}.");
        }
        catch (Exception e)
        {
            logger.Information("Unexpected server error. Please try again later");
            transaction.Rollback();
            return BaseResponse<string>.InternalError("Unexpected server error. Please try again later");
        }
    }
}
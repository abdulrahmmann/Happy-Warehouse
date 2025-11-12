using FluentValidation;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.WarehouseFeature.DTOs;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Infrastructure.UOF;
using Serilog;

namespace HappyWarehouse.Application.Features.WarehouseFeature.Commands.UpdateWarehouse;

public class UpdateWarehouseCommandHandler(IUnitOfWork unitOfWork, ILogger logger, IValidator<UpdateWarehouseDto> validator)
    : ICommandHandler<UpdateWarehouseCommand, BaseResponse<string>>
{
    public async Task<BaseResponse<string>> HandleAsync(UpdateWarehouseCommand command, CancellationToken cancellationToken = default)
    {
        var request = command.WarehouseDto;
        
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

            var existingWareHouse = await unitOfWork.GetWarehouseRepository
                .FirstOrDefaultAsync(w => w.Id == command.Id, cancellationToken);

            if (existingWareHouse is null)
            {
                logger.Warning("Warehouse with Id: '{id}' does not exists.", command.Id);
                return BaseResponse<string>.Conflict($"Warehouse with Id: '{command.Id}' does not exists.");
            }
            
            var duplicate = await unitOfWork.GetWarehouseRepository
                .FirstOrDefaultAsync(c => c.Name.Equals(request.Name, StringComparison.CurrentCultureIgnoreCase), cancellationToken);
            
            if (duplicate != null)
            {
                return BaseResponse<string>.Conflict("A Warehouse with the same name already exists.");
            }

            existingWareHouse.Update(request.Name, request.Address, request.City, request.CountryId, request.UpdatedBy);

            await unitOfWork.GetWarehouseRepository.UpdateAsync(existingWareHouse);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            transaction.Commit();
            
            logger.Information($"Warehouse with ID {command.Id} updated successfully.");

            return BaseResponse<string>.Success("Warehouse updated successfully.");
        }
        catch (Exception e)
        {
            logger.Information("Unexpected server error. Please try again later");
            transaction.Rollback();
            return BaseResponse<string>.InternalError("Unexpected server error. Please try again later");
        }
    }
}
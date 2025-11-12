using FluentValidation;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.WarehouseFeature.DTOs;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Infrastructure.UOF;
using Serilog;

namespace HappyWarehouse.Application.Features.WarehouseFeature.Commands.UpdateWarehouse;

public class UpdateWarehouseCommandHandler(
        IUnitOfWork unitOfWork, 
        ILogger logger, 
        IValidator<UpdateWarehouseDto> validator
    ) : ICommandHandler<UpdateWarehouseCommand, BaseResponse<string>>
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
                logger.Warning("Validation error: {error}", validationErrors);
                return BaseResponse<string>.ValidationError(validationErrors);
            }

            var existingWarehouse = await unitOfWork.GetWarehouseRepository
                .FirstOrDefaultAsync(w => w.Id == command.Id, cancellationToken);

            if (existingWarehouse is null)
            {
                logger.Warning("Warehouse with Id: '{id}' does not exist.", command.Id);
                return BaseResponse<string>.NotFound($"Warehouse with Id: '{command.Id}' does not exist.");
            }

            var duplicate = await unitOfWork.GetWarehouseRepository
                .FirstOrDefaultAsync(w => w.Id != existingWarehouse.Id && w.Name.ToLower() == request.Name.ToLower(), cancellationToken);
            
            if (duplicate != null)
            {
                logger.Warning("Warehouse with name '{name}' already exists.", request.Name);
                return BaseResponse<string>.Conflict("A Warehouse with the same name already exists.");
            }

            existingWarehouse.Update(
                request.Name,
                request.Address,
                request.City,
                request.CountryId,
                request.UpdatedBy ?? "System" 
            );

            await unitOfWork.GetWarehouseRepository.UpdateAsync(existingWarehouse);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            transaction.Commit();

            logger.Information("Warehouse with ID {WarehouseId} updated successfully.", existingWarehouse.Id);

            return BaseResponse<string>.Success("Warehouse updated successfully.");
        }
        catch (Exception e)
        {
            logger.Error(e, "Unexpected server error while updating warehouse with ID {WarehouseId}", command.Id);
            transaction.Rollback();
            return BaseResponse<string>.InternalError("Unexpected server error. Please try again later.");
        }
    }
}

using FluentValidation;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.WarehouseFeature.DTOs;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Infrastructure.UOF;
using Serilog;

namespace HappyWarehouse.Application.Features.WarehouseFeature.Commands.CreateWarehouse;

public class CreateWarehouseCommandHandler(IUnitOfWork unitOfWork, ILogger logger, IValidator<CreateWarehouseDto> validator)
    : ICommandHandler<CreateWarehouseCommand, BaseResponse<string>>
{
    public async Task<BaseResponse<string>> HandleAsync(CreateWarehouseCommand command, CancellationToken cancellationToken = default)
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
                .FirstOrDefaultAsync(w => w.Name == request.Name, cancellationToken);

            if (existingWareHouse is not null)
            {
                logger.Warning("Warehouse with name '{name}' is already exists.", request.Name);
                return BaseResponse<string>.Conflict($"Warehouse '{request.Name}' is already exists.");
            }

            var warehouse = Warehouse.Create(request.Name, request.Address, request.City, request.CountryId, request.CreatedByUserId);

            await unitOfWork.GetRepository<Warehouse>().AddAsync(warehouse);
            
            await unitOfWork.SaveChangesAsync(cancellationToken);

            transaction.Commit();

            logger.Information("Warehouse '{WarehouseName}' created successfully with ID: {WarehouseId}", warehouse.Name, warehouse.Id);

            return BaseResponse<string>.Created($"Country with Name:  created successfully");
        }
        catch (Exception e)
        {
            logger.Information("Unexpected server error. Please try again later");
            transaction.Rollback();
            return BaseResponse<string>.InternalError("Unexpected server error. Please try again later");
        }
    }
}
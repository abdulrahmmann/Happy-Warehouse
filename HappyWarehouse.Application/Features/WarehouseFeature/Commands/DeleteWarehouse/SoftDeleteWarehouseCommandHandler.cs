using HappyWarehouse.Application.Common;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Infrastructure.UOF;
using Serilog;

namespace HappyWarehouse.Application.Features.WarehouseFeature.Commands.DeleteWarehouse;

public class SoftDeleteWarehouseCommandHandler(IUnitOfWork unitOfWork, ILogger logger)
    : ICommandHandler<SoftDeleteWarehouseCommand, BaseResponse<string>>
{
    public async Task<BaseResponse<string>> HandleAsync(SoftDeleteWarehouseCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Id <= 0)
        {
            logger.Warning("Id must be greater than zero");
            return BaseResponse<string>.ValidationError("Id must be greater than zero");
        }
        
        using var transaction = unitOfWork.BeginTransaction();

        try
        {
            var warehouse = await unitOfWork.GetWarehouseRepository.FirstOrDefaultAsync(w => w.Id == command.Id, cancellationToken);

            if (warehouse is null)
            {
                logger.Warning("Warehouse with id {Id} doesn't exist", command.Id);
                return BaseResponse<string>.NotFound("Warehouse not found");
            }
            
            warehouse.SoftDelete();

            await unitOfWork.SaveChangesAsync(cancellationToken);
            
            transaction.Commit();
            
            return BaseResponse<string>.Success($"Warehouse with Id: {command.Id} successfully deleted");
        }
        catch (Exception e)
        {
            logger.Error(e, "Unexpected error while soft deleting warehouse with ID {Id}", command.Id);
            transaction.Rollback();
            return BaseResponse<string>.InternalError("Unexpected server error. Please try again later");
        }

    }
}
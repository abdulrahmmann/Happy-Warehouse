using HappyWarehouse.Application.Common;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Infrastructure.UOF;
using Serilog;

namespace HappyWarehouse.Application.Features.WarehouseFeature.Commands.RestoreWarehouse;

public class RestoreWarehouseCommandHandler(IUnitOfWork unitOfWork, ILogger logger)
    : ICommandHandler<RestoreWarehouseCommand, BaseResponse<string>>
{
    public async Task<BaseResponse<string>> HandleAsync(RestoreWarehouseCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Id <= 0)
        {
            logger.Warning("Id must be greater than zero");
            return BaseResponse<string>.ValidationError("Id must be greater than zero");
        }
        
        using var transaction = unitOfWork.BeginTransaction();

        try
        {
            var warehouse = await unitOfWork.GetWarehouseRepository
                .FirstOrDefaultAsyncWithIgnoreQueryFilter(w => w.Id == command.Id, cancellationToken);

            if (warehouse is null)
            {
                logger.Warning("Warehouse with id {Id} doesn't exist", command.Id);
                return BaseResponse<string>.NotFound("Warehouse not found");
            }
            
            warehouse.Restore(command.RestoreBy);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            transaction.Commit();
            
            return BaseResponse<string>.Success($"Warehouse with Id: {command.Id} successfully restored");
        }
        catch (Exception e)
        {
            logger.Error(e, "Unexpected error while restoring warehouse with ID {Id}", command.Id);
            transaction.Rollback();
            return BaseResponse<string>.InternalError("Unexpected server error. Please try again later");
        }
    }
}

using HappyWarehouse.Application.Common;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Infrastructure.UOF;
using Serilog;

namespace HappyWarehouse.Application.Features.WarehouseFeature.Commands.CreateWarehouseList;

public class CreateWarehouseListCommandHandler(IUnitOfWork unitOfWork, ILogger logger)
    : ICommandHandler<CreateWarehouseListCommand, BaseResponse<string>>
{
    public async Task<BaseResponse<string>> HandleAsync(CreateWarehouseListCommand command, CancellationToken cancellationToken = default)
    {
        var request = command.WarehousesDto;
        
        try
        {
            var warehouses = request.Select(w =>
                Warehouse.Create(w.Name, w.Address, w.City, w.CountryId, w.CreatedByUserId, w.CreatedBy));
            
            await unitOfWork.GetRepository<Warehouse>().AddRangeAsync(warehouses);
            
            await unitOfWork.SaveChangesAsync(cancellationToken);
            
            return BaseResponse<string>.Created($"Warehouses created successfully");
        }
        catch (Exception e)
        {
            logger.Error("Unexpected server error. Please try again later");
            return BaseResponse<string>.InternalError("Unexpected server error. Please try again later");
        }
    }
}
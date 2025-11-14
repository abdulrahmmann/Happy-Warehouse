using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.WarehouseFeature.DTOs;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Infrastructure.UOF;

namespace HappyWarehouse.Application.Features.WarehouseFeature.Queries.GetWarehouseById;

public class GetWarehouseByIdQueryHandler(IUnitOfWork unitOfWork)
    : IQueryHandler<GetWarehouseByIdQuery, BaseResponse<WarehouseDto2>>
{
    public async Task<BaseResponse<WarehouseDto2>> HandleAsync(GetWarehouseByIdQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            if (query.Id <= 0) return BaseResponse<WarehouseDto2>.ValidationError("id must be greater than zero");

            var warehouse = await unitOfWork.GetWarehouseRepository.GetWarehouseByIdAsync(query.Id, cancellationToken);
            
            if (warehouse == null) return BaseResponse<WarehouseDto2>.NotFound();

            var warehouseMapped = new WarehouseDto2(warehouse.Id, warehouse.Name, warehouse.Address, warehouse.City, 
                warehouse.Country.Name, warehouse.CreatedByUserId ?? 1);
            
            return BaseResponse<WarehouseDto2>.Success(warehouseMapped);
        }
        catch (Exception e)
        {
            return BaseResponse<WarehouseDto2>.InternalError();
        }
    }
}
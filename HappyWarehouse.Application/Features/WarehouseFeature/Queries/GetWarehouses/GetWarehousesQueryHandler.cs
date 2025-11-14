using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.WarehouseFeature.DTOs;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Infrastructure.UOF;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace HappyWarehouse.Application.Features.WarehouseFeature.Queries.GetWarehouses;

public class GetWarehousesQueryHandler(IUnitOfWork unitOfWork, ILogger logger): IQueryHandler<GetWarehousesQuery, BaseResponse<IEnumerable<WarehouseDto>>>
{
    public async Task<BaseResponse<IEnumerable<WarehouseDto>>> HandleAsync(GetWarehousesQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            var warehouses = unitOfWork.GetWarehouseRepository.GetAllQueryable();

            var pagedWarehouses = await warehouses
                .OrderBy(w => w.Id)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);
            
            var enumerable = warehouses.ToList();
            
            if (enumerable.Count == 0)
            {
                logger.Warning("No Warehouses found");
                return BaseResponse<IEnumerable<WarehouseDto>>.NoContent("No Warehouses found");
            }

            var warehousesMapped = enumerable.Select(w => new WarehouseDto(w.Id, w.Name, w.Address, w.City, 
                w.Country.Name, w.CreatedByUser.FullName));
            
            return BaseResponse<IEnumerable<WarehouseDto>>
                .Success(data: warehousesMapped, totalCount: enumerable.Count(), message: "Warehouses retrieved successfully");
            
        }
        catch (Exception e)
        {
            logger.Information("Unexpected server error. Please try again later");
            return BaseResponse<IEnumerable<WarehouseDto>>.InternalError("Unexpected server error. Please try again later");
        }
    }
}
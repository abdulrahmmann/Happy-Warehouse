using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.DashboardFeature.DTOs;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Infrastructure.UOF;

namespace HappyWarehouse.Application.Features.DashboardFeature.Queries.GetWarehouseStatus;

public class GetWarehouseStatusQueryHandler(IUnitOfWork unitOfWork)
    : IQueryHandler<GetWarehouseStatusQuery, BaseResponse<List<WarehouseStatusDto>>>
{
    public async Task<BaseResponse<List<WarehouseStatusDto>>> HandleAsync(GetWarehouseStatusQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            var warehouses = await unitOfWork.GetDashboardRepository.GetWarehouseStatusAsync();

            var warehouseStatus = warehouses.Select(w => new WarehouseStatusDto(w.Id, w.Name, w.WarehouseItems.Sum(i => i.Qty))).ToList();
            
            return BaseResponse<List<WarehouseStatusDto>>.Success(warehouseStatus);
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
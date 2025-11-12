using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.WarehouseItemFeature.DTOs;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Infrastructure.UOF;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace HappyWarehouse.Application.Features.WarehouseItemFeature.Queries.GetItemByWarehouseId;

public class GetItemsByWarehouseIdQueryHandler : IQueryHandler<GetItemsByWarehouseIdQuery, BaseResponse<IEnumerable<WarehouseItemDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public GetItemsByWarehouseIdQueryHandler(IUnitOfWork unitOfWork, ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<WarehouseItemDto>>> HandleAsync(GetItemsByWarehouseIdQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            if (query.WarehouseId <= 0)
            {
                _logger.Warning("Warehouse Id must be greater than zero");
                return BaseResponse<IEnumerable<WarehouseItemDto>>.ValidationError("Warehouse Id must be greater than zero");
            }

            var itemsQuery = _unitOfWork.GetWarehouseItemRepository.GetAllItemsByWarehouseIdAsync(query.WarehouseId, cancellationToken);

            var pagedItems = await itemsQuery
                .OrderBy(i => i.Id)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            if (!pagedItems.Any())
            {
                _logger.Warning("Warehouse Items is empty");
                return BaseResponse<IEnumerable<WarehouseItemDto>>.NoContent("Warehouse Items are empty");
            }

            var mappedItems = pagedItems.Select(item => new WarehouseItemDto(
                item.Id,
                item.ItemName,
                item.SkuCode,
                item.Qty,
                item.CostPrice,
                item.MsrpPrice,
                item.WarehouseId,
                item.CreatedByUserId,
                item.CreatedBy
            )).ToList();

            _logger.Information("Warehouse Items with warehouse Id: {WarehouseId} retrieved successfully", query.WarehouseId);

            return BaseResponse<IEnumerable<WarehouseItemDto>>.Success(
                data: mappedItems,
                totalCount: mappedItems.Count,
                message: $"Warehouse Items with warehouse Id: {query.WarehouseId} retrieved successfully"
            );
        }
        catch (Exception e)
        {
            _logger.Information("Unexpected server error. Please try again later");
            return BaseResponse<IEnumerable<WarehouseItemDto>>.InternalError("Unexpected server error. Please try again later");
        }
    }
}

using Asp.Versioning;
using HappyWarehouse.Application.Caching;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.DashboardFeature.DTOs;
using HappyWarehouse.Application.Features.DashboardFeature.Queries.GetWarehouseStatus;
using HappyWarehouse.Application.Features.DashboardFeature.Queries.GetWarehouseWithCountInventory;
using HappyWarehouse.Application.Features.DashboardFeature.Queries.WarehouseTopItems;
using HappyWarehouse.Domain.CQRS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HappyWarehouse.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class DashboardController(Dispatcher dispatcher, IRedisCacheService cacheService) : AppControllerBase
    {
        [AllowAnonymous]
        [HttpGet("warehouse-status")]
        public async Task<IActionResult> GetWarehouseStatus()
        {
            var cachedWarehouseStatus = cacheService.GetData<BaseResponse<List<WarehouseStatusDto>>>("warehouse-status");

            if (cachedWarehouseStatus is not null)
            {
                return NewResult(cachedWarehouseStatus);
            }
            
            var command = new GetWarehouseStatusQuery();
            var response = await dispatcher.SendQueryAsync<GetWarehouseStatusQuery, BaseResponse<List<WarehouseStatusDto>>>(command);
            
            cacheService.SetData("warehouse-status", response);
            
            return NewResult(response);
        }
        
        [AllowAnonymous]
        [HttpGet("top-warehouse-items")]
        public async Task<IActionResult> GetTopItems(int top = 10)
        {
            var cachedTopItems = cacheService.GetData<BaseResponse<List<WarehouseTopItemsDto>>>("top-ten-items");

            if (cachedTopItems is not null)
            {
                return NewResult(cachedTopItems);
            }
            
            var query = new GetTopItemsQuery(top);
            var response = await dispatcher.SendQueryAsync<GetTopItemsQuery, BaseResponse<List<WarehouseTopItemsDto>>>(query);
            
            cacheService.SetData("top-ten-items", response);
            
            return NewResult(response);
        }
        
        [AllowAnonymous]
        [HttpGet("warehouse-inventory-details")]
        public async Task<IActionResult> GetWarehouseWithInventoryDetails(int page = 1, int pageSize = 20)
        {
            var warehouseInventoryCounts = cacheService.GetData<BaseResponse<List<WarehouseCountInventoryStatusDto>>>("warehouse-inventory-details");
            
            if (warehouseInventoryCounts is not null)
            {
                return NewResult(warehouseInventoryCounts);
            }
            
            var query = new GetWarehouseWithCountInventoryQuery(page, pageSize);
            var response = await dispatcher
                .SendQueryAsync<GetWarehouseWithCountInventoryQuery, BaseResponse<List<WarehouseCountInventoryStatusDto>>>(query);
            
            cacheService.SetData("warehouse-inventory-details", response);
            
            return NewResult(response);
        }
    }
}

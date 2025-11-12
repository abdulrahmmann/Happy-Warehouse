using Asp.Versioning;
using HappyWarehouse.Application.Caching;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.WarehouseFeature.Commands.CreateWarehouse;
using HappyWarehouse.Application.Features.WarehouseFeature.Commands.CreateWarehouseList;
using HappyWarehouse.Application.Features.WarehouseFeature.Commands.DeleteWarehouse;
using HappyWarehouse.Application.Features.WarehouseFeature.Commands.RestoreWarehouse;
using HappyWarehouse.Application.Features.WarehouseFeature.Commands.UpdateWarehouse;
using HappyWarehouse.Application.Features.WarehouseFeature.DTOs;
using HappyWarehouse.Application.Features.WarehouseFeature.Queries.GetWarehouses;
using HappyWarehouse.Application.Features.WarehouseFeature.Queries.GetWarehousesWithItems;
using HappyWarehouse.Domain.CQRS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HappyWarehouse.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class WarehouseController(Dispatcher dispatcher, IRedisCacheService cacheService) : AppControllerBase
    {
        #region GET Endpoints
        [AllowAnonymous]
        [HttpGet("warehouses")]
        public async Task<IActionResult> GetWarehouses()
        {
            var cachedWarehouses = cacheService.GetData<BaseResponse<IEnumerable<WarehouseDto>>>("all-warehouses");

            if (cachedWarehouses is not null)
            {
                return NewResult(cachedWarehouses);
            }
            
            var command = new GetWarehousesQuery();
            var response = await dispatcher.SendQueryAsync<GetWarehousesQuery, BaseResponse<IEnumerable<WarehouseDto>>>(command);
            
            cacheService.SetData("all-warehouses", response);
            
            return NewResult(response);
        }
        
        [AllowAnonymous]
        [HttpGet("warehouses-with-items")]
        public async Task<IActionResult> GetWarehousesWithItems()
        {
            var cachedWarehouses = cacheService.GetData<BaseResponse<IEnumerable<WarehousesWithItemsDto>>>("all-warehouses-with-items");
            
            if (cachedWarehouses is not null)
            {
                return NewResult(cachedWarehouses);
            }
            
            var query = new GetWarehousesWithItemsQuery();
            var response = await dispatcher.SendQueryAsync<GetWarehousesWithItemsQuery, BaseResponse<IEnumerable<WarehousesWithItemsDto>>>(query);
            
            cacheService.SetData("all-warehouses-with-items", response);
            
            return NewResult(response);
        }
        #endregion

        #region POST Endpoints
        [AllowAnonymous]
        [HttpPost("create-warehouse")]
        public async Task<IActionResult> CreateWarehouse([FromBody] CreateWarehouseDto warehouseDto)
        {
            var command = new CreateWarehouseCommand(warehouseDto);
            var response = await dispatcher.SendCommandAsync<CreateWarehouseCommand, BaseResponse<string>>(command);
            return NewResult(response);
        } 
        
        [AllowAnonymous]
        [HttpPost("create-warehouse-list")]
        public async Task<IActionResult> CreateWarehousesList([FromBody] IEnumerable<CreateWarehouseDto> warehousesDto)
        {
            var command = new CreateWarehouseListCommand(warehousesDto);
            var response = await dispatcher.SendCommandAsync<CreateWarehouseListCommand, BaseResponse<string>>(command);
            return NewResult(response);
        } 
        #endregion

        #region PUT Endpoints
        [AllowAnonymous]
        [HttpPut("update-warehouse/id={id}")]
        public async Task<IActionResult> UpdateWarehouse([FromQuery] int id, [FromBody] UpdateWarehouseDto warehouseDto)
        {
            var command = new UpdateWarehouseCommand(id, warehouseDto);
            var response = await dispatcher.SendCommandAsync<UpdateWarehouseCommand, BaseResponse<string>>(command);
            return NewResult(response);
        } 
        
        [AllowAnonymous]
        [HttpPut("restore-warehouse/id={id}")]
        public async Task<IActionResult> RestoreWarehouse([FromQuery] int id, string? restoreBy)
        {
            var command = new RestoreWarehouseCommand(id, restoreBy);
            var response = await dispatcher.SendCommandAsync<RestoreWarehouseCommand, BaseResponse<string>>(command);
            return NewResult(response);
        }
        #endregion
        
        #region DELETE Endpoints
        [AllowAnonymous]
        [HttpDelete("delete-warehouse/id={id}")]
        public async Task<IActionResult> DeleteWarehouse([FromQuery] int id)
        {
            var command = new SoftDeleteWarehouseCommand(id);
            var response = await dispatcher.SendCommandAsync<SoftDeleteWarehouseCommand, BaseResponse<string>>(command);
            return NewResult(response);
        }
        #endregion
    }
}

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
        
        [AllowAnonymous]
        [HttpPut("update-warehouse")]
        public async Task<IActionResult> UpdateWarehouse(int id, [FromBody] UpdateWarehouseDto warehouseDto)
        {
            var command = new UpdateWarehouseCommand(id, warehouseDto);
            var response = await dispatcher.SendCommandAsync<UpdateWarehouseCommand, BaseResponse<string>>(command);
            return NewResult(response);
        } 
        
        [AllowAnonymous]
        [HttpDelete("delete-warehouse")]
        public async Task<IActionResult> DeleteWarehouse(int id)
        {
            var command = new SoftDeleteWarehouseCommand(id);
            var response = await dispatcher.SendCommandAsync<SoftDeleteWarehouseCommand, BaseResponse<string>>(command);
            return NewResult(response);
        }
        
        [AllowAnonymous]
        [HttpPut("restore-warehouse")]
        public async Task<IActionResult> RestoreWarehouse(int id, string? restoreBy)
        {
            var command = new RestoreWarehouseCommand(id, restoreBy);
            var response = await dispatcher.SendCommandAsync<RestoreWarehouseCommand, BaseResponse<string>>(command);
            return NewResult(response);
        }
        
        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetWarehouses()
        {
            var command = new GetWarehousesQuery();
            var response = await dispatcher.SendQueryAsync<GetWarehousesQuery, BaseResponse<IEnumerable<WarehouseDto>>>(command);
            return NewResult(response);
        }
        
        [AllowAnonymous]
        [HttpGet("warehouses-with-items")]
        public async Task<IActionResult> GetWarehousesWithItems()
        {
            var query = new GetWarehousesWithItemsQuery();
            var response = await dispatcher.SendQueryAsync<GetWarehousesWithItemsQuery, BaseResponse<IEnumerable<WarehousesWithItemsDto>>>(query);
            return NewResult(response);
        }

    }
}

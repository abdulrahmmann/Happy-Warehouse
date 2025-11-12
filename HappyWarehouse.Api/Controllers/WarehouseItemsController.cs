using Asp.Versioning;
using HappyWarehouse.Application.Caching;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.WarehouseItemFeature.Commands.CreateWarehouseItem;
using HappyWarehouse.Application.Features.WarehouseItemFeature.DTOs;
using HappyWarehouse.Application.Features.WarehouseItemFeature.Queries.GetItemByWarehouseId;
using HappyWarehouse.Domain.CQRS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HappyWarehouse.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class WarehouseItemsController(Dispatcher dispatcher, IRedisCacheService cacheService) : AppControllerBase
    {
        [AllowAnonymous]
        [HttpPost("create-item")]
        public async Task<IActionResult> CreateItem([FromBody] CreateWarehouseItemDto itemDto)
        {
            var command = new CreateWarehouseItemCommand(itemDto);
            var response = await dispatcher.SendCommandAsync<CreateWarehouseItemCommand, BaseResponse<string>>(command);
            return NewResult(response);
        }
        
        [AllowAnonymous]
        [HttpPost("get-items/{warehouseId}")]
        public async Task<IActionResult> GetItemsByWarehouseId([FromQuery] int warehouseId)
        {
            var command = new GetItemsByWarehouseIdQuery(warehouseId);
            var response = await dispatcher.SendQueryAsync<GetItemsByWarehouseIdQuery, BaseResponse<IEnumerable<WarehouseItemDto>>>(command);
            return NewResult(response);
        }
    }
}

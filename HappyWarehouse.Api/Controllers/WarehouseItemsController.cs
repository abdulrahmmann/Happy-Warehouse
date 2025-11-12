using Asp.Versioning;
using HappyWarehouse.Application.Caching;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.WarehouseItemFeature.Commands.CreateWarehouseItem;
using HappyWarehouse.Application.Features.WarehouseItemFeature.Commands.CreateWarehouseItemList;
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
        #region GET Endpoints
        [AllowAnonymous]
        [HttpGet("get-items/{warehouseId}")]
        public async Task<IActionResult> GetItemsByWarehouseId([FromQuery] int warehouseId)
        {
            var cachedKey = $"warehouse-items-{warehouseId}";
            var cachedWarehouseItems = cacheService.GetData<BaseResponse<IEnumerable<WarehouseItemDto>>>(cachedKey);
            if (cachedWarehouseItems is not null)
            {
                return NewResult(cachedWarehouseItems);
            }
            
            var command = new GetItemsByWarehouseIdQuery(warehouseId);
            var response = await dispatcher.SendQueryAsync<GetItemsByWarehouseIdQuery, BaseResponse<IEnumerable<WarehouseItemDto>>>(command);
            
            cacheService.SetData(cachedKey, response);
            
            return NewResult(response);
        }
        #endregion
        
        #region POST Endpoints
        [AllowAnonymous]
        [HttpPost("create-item")]
        public async Task<IActionResult> CreateItem([FromBody] CreateWarehouseItemDto itemDto)
        {
            var command = new CreateWarehouseItemCommand(itemDto);
            var response = await dispatcher.SendCommandAsync<CreateWarehouseItemCommand, BaseResponse<string>>(command);
            return NewResult(response);
        }
        
        [AllowAnonymous]
        [HttpPost("create-item-list")]
        public async Task<IActionResult> CreateItemsList([FromBody] IEnumerable<CreateWarehouseItemDto> itemsDto)
        {
            var command = new CreateWarehouseItemListCommand(itemsDto);
            var response = await dispatcher.SendCommandAsync<CreateWarehouseItemListCommand, BaseResponse<string>>(command);
            return NewResult(response);
        }
        #endregion
    }
}

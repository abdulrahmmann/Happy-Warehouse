using Asp.Versioning;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.WarehouseFeature.Commands.CreateWarehouse;
using HappyWarehouse.Application.Features.WarehouseFeature.Commands.UpdateWarehouse;
using HappyWarehouse.Application.Features.WarehouseFeature.DTOs;
using HappyWarehouse.Domain.CQRS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HappyWarehouse.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class WarehouseController(Dispatcher dispatcher) : AppControllerBase
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
        [HttpPut("update-warehouse")]
        public async Task<IActionResult> UpdateWarehouse(int id, [FromBody] UpdateWarehouseDto warehouseDto)
        {
            var command = new UpdateWarehouseCommand(id, warehouseDto);
            var response = await dispatcher.SendCommandAsync<UpdateWarehouseCommand, BaseResponse<string>>(command);
            return NewResult(response);
        } 
    }
}

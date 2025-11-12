using Asp.Versioning;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.DashboardFeature.DTOs;
using HappyWarehouse.Application.Features.DashboardFeature.Queries.GetWarehouseStatus;
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
    public class DashboardController(Dispatcher dispatcher) : AppControllerBase
    {
        [AllowAnonymous]
        [HttpGet("warehouse-status")]
        public async Task<IActionResult> GetWarehouseStatus()
        {
            var command = new GetWarehouseStatusQuery();
            var response = await dispatcher.SendQueryAsync<GetWarehouseStatusQuery, BaseResponse<List<WarehouseStatusDto>>>(command);
            return NewResult(response);
        }
        
        [AllowAnonymous]
        [HttpGet("warehouse-top-items")]
        public async Task<IActionResult> GetWarehouseTopItems(int top = 10)
        {
            var command = new GetTopHighItemsQuery();
            var response = await dispatcher.SendQueryAsync<GetTopHighItemsQuery, BaseResponse<List<WarehouseTopItemsDto>>>(command);
            return NewResult(response);
        }
    }
}

using Asp.Versioning;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.CountryFeature.Commands.CreateCountry;
using HappyWarehouse.Application.Features.CountryFeature.DTOs;
using HappyWarehouse.Domain.CQRS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HappyWarehouse.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CountryController(Dispatcher dispatcher) : AppControllerBase
    {
        [HttpPost("create-country")]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDto countryDto)
        {
            var command = new CreateCountryCommand(countryDto);
            var response = await dispatcher.SendCommandAsync<CreateCountryCommand, BaseResponse<string>>(command);
            return NewResult(response);
        }
    }
}

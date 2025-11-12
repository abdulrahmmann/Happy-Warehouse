using Asp.Versioning;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.CountryFeature.Commands.CreateCountry;
using HappyWarehouse.Application.Features.CountryFeature.Commands.CreateList;
using HappyWarehouse.Application.Features.CountryFeature.Commands.UpdateCountry;
using HappyWarehouse.Application.Features.CountryFeature.DTOs;
using HappyWarehouse.Application.Features.CountryFeature.Queries.GetAll;
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
        [Authorize(Roles = "Admin")]
        [HttpPost("create-country")]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDto countryDto)
        {
            var command = new CreateCountryCommand(countryDto);
            var response = await dispatcher.SendCommandAsync<CreateCountryCommand, BaseResponse<string>>(command);
            return NewResult(response);
        }
        
        [AllowAnonymous]
        [HttpPost("create-country-list")]
        public async Task<IActionResult> CreateCountryList([FromBody] IEnumerable<CreateCountryDto> countriesDto)
        {
            var command = new CreateCountryListCommand(countriesDto);
            var response = await dispatcher.SendCommandAsync<CreateCountryListCommand, BaseResponse<string>>(command);
            return NewResult(response);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost("update-country")]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDto countryDto)
        {
            var command = new UpdateCountryCommand(id, countryDto);
            var response = await dispatcher.SendCommandAsync<UpdateCountryCommand, BaseResponse<string>>(command);
            return NewResult(response);
        }
        
        [AllowAnonymous]
        [HttpGet("list")]
        public async Task<IActionResult> GetAllCountry()
        {
            var command = new GetAllCountriesQuery();
            var response = await dispatcher.SendQueryAsync<GetAllCountriesQuery, BaseResponse<IEnumerable<CountryDto>>>(command);
            return NewResult(response);
        }
        
    }
}

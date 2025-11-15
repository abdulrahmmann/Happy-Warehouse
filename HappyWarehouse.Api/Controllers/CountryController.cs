using Asp.Versioning;
using HappyWarehouse.Application.Caching;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.CountryFeature.Commands.CreateCountry;
using HappyWarehouse.Application.Features.CountryFeature.Commands.CreateList;
using HappyWarehouse.Application.Features.CountryFeature.Commands.UpdateCountry;
using HappyWarehouse.Application.Features.CountryFeature.DTOs;
using HappyWarehouse.Application.Features.CountryFeature.Queries.GetAll;
using HappyWarehouse.Domain.CQRS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HappyWarehouse.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [EnableCors]
    public class CountryController(Dispatcher dispatcher, IRedisCacheService cacheService) : AppControllerBase
    {
        #region GET Endpoints
        [AllowAnonymous]
        [HttpGet("list")]
        public async Task<IActionResult> GetAllCountry()
        {
            var cachedCountry = cacheService.GetData<BaseResponse<IEnumerable<CountryDto>>>("all-countries");

            if (cachedCountry is not null)
            {
                return NewResult(cachedCountry);
            }
            
            var query = new GetAllCountriesQuery();
            var response = await dispatcher.SendQueryAsync<GetAllCountriesQuery, BaseResponse<IEnumerable<CountryDto>>>(query);
            
            cacheService.SetData("all-countries", response);
            
            return NewResult(response);
        }
        #endregion

        #region POST Endpoints
        [Authorize(Roles = "Admin")]
        [HttpPost("create-country")]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDto countryDto)
        {
            var command = new CreateCountryCommand(countryDto);
            var response = await dispatcher.SendCommandAsync<CreateCountryCommand, BaseResponse<string>>(command);
            return NewResult(response);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost("create-country-list")]
        public async Task<IActionResult> CreateCountryList([FromBody] IEnumerable<CreateCountryDto> countriesDto)
        {
            var command = new CreateCountryListCommand(countriesDto);
            var response = await dispatcher.SendCommandAsync<CreateCountryListCommand, BaseResponse<string>>(command);
            return NewResult(response);
        }
        #endregion
        
        #region PUT Endpoints
        [Authorize(Roles = "Admin")]
        [HttpPut("update-country/id={id}")]
        public async Task<IActionResult> UpdateCountry([FromQuery] int id, [FromBody] UpdateCountryDto countryDto)
        {
            var command = new UpdateCountryCommand(id, countryDto);
            var response = await dispatcher.SendCommandAsync<UpdateCountryCommand, BaseResponse<string>>(command);
            return NewResult(response);
        }
        #endregion
    }
}

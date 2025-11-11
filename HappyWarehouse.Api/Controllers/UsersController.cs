using Asp.Versioning;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.UsersFeature.Commands.LoginUser;
using HappyWarehouse.Application.Features.UsersFeature.Commands.UpdateUser;
using HappyWarehouse.Application.Features.UsersFeature.DTOs;
using HappyWarehouse.Domain.CQRS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HappyWarehouse.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [AllowAnonymous]
    public class UsersController : AppControllerBase
    {
        private readonly Dispatcher _dispatcher;

        public UsersController(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto userDto)
        {
            var command = new LoginUserCommand(userDto);
            var response = await _dispatcher.SendCommandAsync<LoginUserCommand, AuthenticationResponse>(command);
            return NewResult(response);
        }
        
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromQuery] string email, [FromBody] UpdateUserDto userDto)
        {
            var command = new UpdateUserCommand(email, userDto);
            var response = await _dispatcher.SendCommandAsync<UpdateUserCommand, AuthenticationResponse>(command);
            return NewResult(response);
        }
    }
}

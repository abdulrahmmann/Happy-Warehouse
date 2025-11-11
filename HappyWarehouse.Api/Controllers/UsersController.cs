using Asp.Versioning;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.UsersFeature.Commands.ChangePassword;
using HappyWarehouse.Application.Features.UsersFeature.Commands.DeleteUser;
using HappyWarehouse.Application.Features.UsersFeature.Commands.LoginUser;
using HappyWarehouse.Application.Features.UsersFeature.Commands.RestoreUser;
using HappyWarehouse.Application.Features.UsersFeature.Commands.UpdateUser;
using HappyWarehouse.Application.Features.UsersFeature.DTOs;
using HappyWarehouse.Application.Features.UsersFeature.Queries.GetAllUsers;
using HappyWarehouse.Domain.CQRS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HappyWarehouse.Controllers
{
    [Authorize(Roles = "Admin,Management,Auditor")]
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UsersController : AppControllerBase
    {
        private readonly Dispatcher _dispatcher;

        public UsersController(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [AllowAnonymous]
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
        
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromQuery] string email, [FromBody] ChangePasswordDto passwordDto)
        {
            var command = new ChangePasswordCommand(email, passwordDto);
            var response = await _dispatcher.SendCommandAsync<ChangePasswordCommand, AuthenticationResponse>(command);
            return NewResult(response);
        }

        
        [HttpDelete("soft-delete")]
        public async Task<IActionResult> SoftDeleteUser([FromQuery] string email)
        {
            var command = new SoftDeleteUserCommand(email);
            var response = await _dispatcher.SendCommandAsync<SoftDeleteUserCommand, AuthenticationResponse>(command);
            return NewResult(response);
        }
        
        [HttpPut("restore-user")]
        public async Task<IActionResult> ChangePassword([FromQuery] string email)
        {
            var command = new RestoreUserCommand(email);
            var response = await _dispatcher.SendCommandAsync<RestoreUserCommand, AuthenticationResponse>(command);
            return NewResult(response);
        }
        
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var query = new GetAllUsersQuery();
            var users = await _dispatcher.SendQueryAsync<GetAllUsersQuery, UserResponse<IEnumerable<UserDetailsDto>>>(query);
            return NewResult(users);
        }


    }
}

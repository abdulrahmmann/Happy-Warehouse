using Asp.Versioning;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.UsersFeature.Commands.ChangePassword;
using HappyWarehouse.Application.Features.UsersFeature.Commands.DeleteUser;
using HappyWarehouse.Application.Features.UsersFeature.Commands.GenerateNewAccessToken;
using HappyWarehouse.Application.Features.UsersFeature.Commands.LoginUser;
using HappyWarehouse.Application.Features.UsersFeature.Commands.RestoreUser;
using HappyWarehouse.Application.Features.UsersFeature.Commands.UpdateUser;
using HappyWarehouse.Application.Features.UsersFeature.DTOs;
using HappyWarehouse.Application.Features.UsersFeature.Models;
using HappyWarehouse.Application.Features.UsersFeature.Queries.GetAllUsers;
using HappyWarehouse.Application.Features.UsersFeature.Queries.GetUserRole;
using HappyWarehouse.Domain.CQRS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HappyWarehouse.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UsersController(Dispatcher dispatcher) 
        : AppControllerBase
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto userDto)
        {
            var command = new LoginUserCommand(userDto);
            var response = await dispatcher.SendCommandAsync<LoginUserCommand, AuthenticationResponse>(command);
            return NewResult(response);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromQuery] string email, [FromBody] UpdateUserDto userDto)
        {
            var command = new UpdateUserCommand(email, userDto);
            var response = await dispatcher.SendCommandAsync<UpdateUserCommand, AuthenticationResponse>(command);
            return NewResult(response);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromQuery] string email, [FromBody] ChangePasswordDto passwordDto)
        {
            var command = new ChangePasswordCommand(email, passwordDto);
            var response = await dispatcher.SendCommandAsync<ChangePasswordCommand, AuthenticationResponse>(command);
            return NewResult(response);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpDelete("soft-delete")]
        public async Task<IActionResult> SoftDeleteUser([FromQuery] string email)
        {
            var command = new SoftDeleteUserCommand(email);
            var response = await dispatcher.SendCommandAsync<SoftDeleteUserCommand, AuthenticationResponse>(command);
            return NewResult(response);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPut("restore-user")]
        public async Task<IActionResult> RestoreUser([FromQuery] string email)
        {
            var command = new RestoreUserCommand(email);
            var response = await dispatcher.SendCommandAsync<RestoreUserCommand, AuthenticationResponse>(command);
            return NewResult(response);
        }
        
        [Authorize(Roles = "Admin,Auditor")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var query = new GetAllUsersQuery();
            var users = await dispatcher.SendQueryAsync<GetAllUsersQuery, UserResponse<IEnumerable<UserDetailsDto>>>(query);
            return NewResult(users);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet("user-role/{email}")]
        public async Task<IActionResult> GetUserRoleByEmail([FromRoute] string email)
        {
            var query = new GetUserRolesByEmailQuery(email);
            var user = await dispatcher.SendQueryAsync<GetUserRolesByEmailQuery, string>(query);
            return Ok(user);
        }
        
        [HttpPost("generate-new-access-token")]
        public async Task<IActionResult> GenerateNewAccessToken(TokenModel tokenModel)
        {
            var command = new GenerateNewAccessTokenCommand(tokenModel);
            var user = await dispatcher
                .SendCommandAsync<GenerateNewAccessTokenCommand, AuthenticationResponse>(command);
            return NewResult(user);
        }
    }
}

using HappyWarehouse.Application.Common;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace HappyWarehouse.Application.Features.UsersFeature.Commands.ChangePassword;

public class ChangePasswordCommandHandler: ICommandHandler<ChangePasswordCommand, AuthenticationResponse>
{
    #region Instance Fields
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger _logger;
    #endregion

    #region Inject Instances Into Constructor
    public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager, ILogger logger)
    {
        _userManager = userManager;
        _logger = logger;
    }
    #endregion

    #region Change Password Command Handler
    public async Task<AuthenticationResponse> HandleAsync(ChangePasswordCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(command.Email))
                return AuthenticationResponse.Failure("Email is required.");

            var user = await _userManager.FindByEmailAsync(command.Email);
            
            if (user == null)
                return AuthenticationResponse.Failure("User not found.");
            
            // Change password
            var result = await _userManager.ChangePasswordAsync(
                user, 
                command.PasswordDto.CurrentPassword, 
                command.PasswordDto.NewPassword
            );

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.Error("Failed to change password: {errors}", errors);
                return AuthenticationResponse.Failure($"Failed to change password: {errors}");
            }

            return AuthenticationResponse.Success("Password changed successfully.");
        }
        catch (Exception e)
        {
            _logger.Error(e, "Unexpected error while updating user: {Email}", command.Email);
            return AuthenticationResponse.Failure("Unexpected server error. Please try again later.");
        }
    }
    #endregion
}
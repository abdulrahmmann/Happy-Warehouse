using HappyWarehouse.Application.Common;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace HappyWarehouse.Application.Features.UsersFeature.Commands.DeleteUser;

public class SoftDeleteUserCommandHandler: ICommandHandler<SoftDeleteUserCommand, AuthenticationResponse>
{
    #region Instance Fields
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger _logger;
    #endregion

    #region Inject Instances Into Constructor
    public SoftDeleteUserCommandHandler(UserManager<ApplicationUser> userManager, ILogger logger)
    {
        _userManager = userManager;
        _logger = logger;
    }
    #endregion

    #region Softly Delete User Command Handler
    public async Task<AuthenticationResponse> HandleAsync(SoftDeleteUserCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(command.Email))
            {
                _logger.Warning("Email parameter is required for soft deleting user.");
                return AuthenticationResponse.Failure("Email is required for soft deleting user.");
            }

            var user = await _userManager.FindByEmailAsync(command.Email);
            
            if (user == null)
                return AuthenticationResponse.Failure("User not found.");
            
            var role = await _userManager.GetRolesAsync(user);
            if (role.Contains("Admin"))
            {
                _logger.Warning("Admin cannot be deleted.");
                return AuthenticationResponse.Failure("Admin cannot be deleted.");
            }
            
            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;
            user.IsActive = false; 

            var result = await _userManager.UpdateAsync(user);
            
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.Error("Failed to soft delete user: {errors}", errors);
                return AuthenticationResponse.Failure($"Failed to soft delete user: {errors}");
            }

            return AuthenticationResponse.Success("User soft deleted successfully.");
        }
        catch (Exception e)
        {
            _logger.Error(e, "Unexpected error while updating user: {Email}", command.Email);
            return AuthenticationResponse.Failure("Unexpected server error. Please try again later.");
        }
    }
    #endregion
}
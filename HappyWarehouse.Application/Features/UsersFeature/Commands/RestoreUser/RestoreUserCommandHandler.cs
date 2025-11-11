using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.UsersFeature.TokenServices.GenerateToken;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace HappyWarehouse.Application.Features.UsersFeature.Commands.RestoreUser;

public class RestoreUserCommandHandler: ICommandHandler<RestoreUserCommand, AuthenticationResponse>
{
    #region Instance Fields
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IGenerateTokenService _tokenService;
    private readonly ILogger _logger;
    #endregion

    #region Inject Instances Into Constructor 
    public RestoreUserCommandHandler(UserManager<ApplicationUser> userManager, IGenerateTokenService tokenService, ILogger logger)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _logger = logger;
    }
    #endregion
    
    public async Task<AuthenticationResponse> HandleAsync(RestoreUserCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(command.Email))
            {
                _logger.Warning("Email is required");
                return AuthenticationResponse.Failure("Email is required");
            }

            var user = await _userManager.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Email == command.Email, cancellationToken);
            
            if (user == null)
                return AuthenticationResponse.Failure("User not found");

            if (!user.IsDeleted)
                return AuthenticationResponse.Failure("User is not deleted");

            user.IsDeleted = false;
            user.DeletedAt = null;
            user.RestoredAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(e => e.Description));
                _logger.Error("Failed to restore user: {errors}", errors);
                return AuthenticationResponse.Failure($"Failed to restore user, error: {errors}");
            }

            var tokenResponse = _tokenService.GenerateToken(user);
            user.RefreshToken = tokenResponse.RefreshToken;
            user.RefreshTokenExpiration = tokenResponse.RefreshTokenExpiration;

            await _userManager.UpdateAsync(user);

            return tokenResponse;
        }
        catch (Exception e)
        {
            _logger.Error(e, "Unexpected error while restoring user: {Email}", command.Email);
            return AuthenticationResponse.Failure("Unexpected server error. Please try again later.");
        }
    }
}
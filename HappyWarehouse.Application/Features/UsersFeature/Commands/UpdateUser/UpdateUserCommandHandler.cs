using FluentValidation;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.UsersFeature.DTOs;
using HappyWarehouse.Application.Features.UsersFeature.TokenServices.GenerateToken;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using ILogger = Serilog.ILogger;

namespace HappyWarehouse.Application.Features.UsersFeature.Commands.UpdateUser;

public class UpdateUserCommandHandler: ICommandHandler<UpdateUserCommand, AuthenticationResponse>
{
    #region Instance Fields
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IGenerateTokenService _tokenService;
    private readonly IValidator<UpdateUserDto>  _validator;
    private readonly ILogger _logger;
    #endregion

    #region Inject Instances Into Constructor
    public UpdateUserCommandHandler(UserManager<ApplicationUser> userManager, IGenerateTokenService tokenService, 
        ILogger logger, IValidator<UpdateUserDto> validator)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _logger = logger;
        _validator = validator;
    }
    #endregion


    #region Update User Information Command Handler
    public async Task<AuthenticationResponse> HandleAsync(UpdateUserCommand command, CancellationToken cancellationToken = default) 
    {
        var userRequest = command.UserDto;

        try
        {
            if (string.IsNullOrWhiteSpace(command.Email))
            {
                _logger.Warning("Email parameter is required for updating user.");
                return AuthenticationResponse.Failure("Email is required for updating user.");
            }

            var validationResult = await _validator.ValidateAsync(userRequest, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.Error("Validation errors: {errors}", errors);
                return AuthenticationResponse.Failure($"Invalid request: {errors}");
            }

            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user == null)
                return AuthenticationResponse.Failure("User not found.");

            user.UserName = userRequest.UserName ?? user.UserName;
            user.FullName = userRequest.FullName ?? user.FullName;
            user.PhoneNumber = userRequest.PhoneNumber ?? user.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(userRequest.Email))
                user.Email = userRequest.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.Error("Failed to update user: {errors}", errors);
                return AuthenticationResponse.Failure($"Failed to update user: {errors}");
            }

            var tokenResponse = _tokenService.GenerateToken(user);
            user.RefreshToken = tokenResponse.RefreshToken;
            user.RefreshTokenExpiration = tokenResponse.RefreshTokenExpiration;

            await _userManager.UpdateAsync(user);

            return tokenResponse;
        }
        catch (Exception e)
        {
            _logger.Error(e, "Unexpected error while updating user: {Email}", command.Email);
            return AuthenticationResponse.Failure("Unexpected server error. Please try again later.");
        }
    }
    #endregion
}
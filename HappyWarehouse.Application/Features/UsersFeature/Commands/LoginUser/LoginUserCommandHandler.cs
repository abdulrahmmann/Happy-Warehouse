using System.Net;
using FluentValidation;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.UsersFeature.DTOs;
using HappyWarehouse.Application.Features.UsersFeature.TokenServices.GenerateToken;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace HappyWarehouse.Application.Features.UsersFeature.Commands.LoginUser;

public class LoginUserCommandHandler: ICommandHandler<LoginUserCommand, AuthenticationResponse>
{
    #region Instance Fields
    private readonly UserManager<ApplicationUser>  _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IValidator<LoginUserDto> _validator;
    private readonly IGenerateTokenService  _generateTokenService;
    private readonly ILogger _logger;
    #endregion

    #region Inject Instances Into Constructor
    public LoginUserCommandHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, 
        IValidator<LoginUserDto> validator, IGenerateTokenService generateTokenService, ILogger logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _validator = validator;
        _generateTokenService = generateTokenService;
        _logger = logger;
    }
    #endregion

    #region User Login Command Handler
    public async Task<AuthenticationResponse> HandleAsync(LoginUserCommand command, CancellationToken cancellationToken = default) 
    {
        var userRequest = command.UserDto;
        
        try
        {
            // Validate user request.
            var validationResult = await _validator.ValidateAsync(userRequest, cancellationToken);

            if (!validationResult.IsValid)
            {
                var validationErrors = validationResult.Errors.Select(err => string.Join(" ; ", err.ErrorMessage)).ToList();
                _logger.Warning("Error validating user request: {x}", validationErrors);
                return AuthenticationResponse.Failure("Validation failed.", validationErrors, HttpStatusCode.UnprocessableEntity);
            }
            
            // Check if email exists
            var user = await _userManager.FindByEmailAsync(userRequest.Email!);
            
            if (user is null)
            {
                _logger.Warning("Email Does not exists.");
                return AuthenticationResponse.Failure("Email Does not exists.", statusCode: HttpStatusCode.NotFound);
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                _logger.Warning("Account is locked. Try again later.");
                return AuthenticationResponse.Failure("Account is locked. Try again later.");
            }
            
            if (!user.IsActive)
            {
                _logger.Warning("Your account is disabled, please contact support.");
                return AuthenticationResponse.Failure("Your account is disabled, please contact support.");
            }
            
            // check password
            var result = await _signInManager.CheckPasswordSignInAsync(user, userRequest.Password!, lockoutOnFailure:true);
            
            if (!result.Succeeded)
            {
                _logger.Warning("Invalid login attempt.");
                return AuthenticationResponse.Failure("Invalid login attempt.", statusCode: HttpStatusCode.Unauthorized);
            }
            
            if (result.IsLockedOut)
            {
                _logger.Warning("Account is locked due to multiple failed login attempts.");
                return AuthenticationResponse.Failure("Account is locked due to multiple failed login attempts.");
            }

            // Generate Token.
            var tokenResponse = _generateTokenService.GenerateToken(user);

            return tokenResponse;
        }
        catch (Exception e)
        {
            _logger.Error(e, "Unexpected error while registering user: {Email}", userRequest.Email); 
            return AuthenticationResponse.Failure("Unexpected server error. Please try again later."); 
        }
    }
    #endregion
}
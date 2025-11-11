using System.Net;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.UsersFeature.DTOs;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace HappyWarehouse.Application.Features.UsersFeature.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, UserResponse<IEnumerable<UserDetailsDto>>>
{
    #region Instance Fields

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger _logger;

    #endregion

    #region Inject Instances Into Constructor

    public GetAllUsersQueryHandler(UserManager<ApplicationUser> userManager, ILogger logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    #endregion


    public async Task<UserResponse<IEnumerable<UserDetailsDto>>> HandleAsync(GetAllUsersQuery query,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var users = _userManager.Users.ToList(); 

            if (users.Count == 0)
            {
                return UserResponse<IEnumerable<UserDetailsDto>>
                    .Failure("Users are empty", HttpStatusCode.NoContent);
            }

            var userList = new List<UserDetailsDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var roleName = roles.FirstOrDefault() ?? string.Empty;

                userList.Add(new UserDetailsDto(
                    user.Id.ToString(),
                    user.UserName,
                    user.FullName,
                    user.Email,
                    user.PhoneNumber,
                    roleName,
                    user.IsDeleted,
                    user.IsActive,
                    user.CreatedAt,
                    user.UpdatedAt,
                    user.DeletedAt
                ));
            }

            return UserResponse<IEnumerable<UserDetailsDto>>
                .Success(
                    totalCount: userList.Count,
                    httpStatusCode: HttpStatusCode.OK,
                    message: "Users retrieved successfully",
                    data: userList
                );
        }
        catch (Exception e)
        {
            _logger.Error(e, "Unexpected error while retrieving users");
            return UserResponse<IEnumerable<UserDetailsDto>>
                .Failure("Unexpected server error. Please try again later.", HttpStatusCode.InternalServerError);
        }
    }
}
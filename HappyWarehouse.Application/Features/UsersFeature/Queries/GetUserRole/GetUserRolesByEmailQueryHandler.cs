using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace HappyWarehouse.Application.Features.UsersFeature.Queries.GetUserRole;

public class GetUserRolesByEmailQueryHandler(UserManager<ApplicationUser> userManager, ILogger logger)
    : IQueryHandler<GetUserRolesByEmailQuery, string>
{
    public async Task<string> HandleAsync(GetUserRolesByEmailQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(query.Email)) throw new ArgumentNullException(nameof(query.Email));

            var user = await userManager.FindByEmailAsync(query.Email);

            if (user == null)
            {
                logger.Warning("No user found with email: {Email}", query.Email);
                return $"No user found with email: {query.Email}";
            }

            var role = await userManager.GetRolesAsync(user);

            return role[0];
        }
        catch (Exception e)
        {
            logger.Error(e, "Unexpected error while updating user: {Email}", query.Email);
            return $"Unexpected error while updating user: {query.Email}";
        }
    }
}
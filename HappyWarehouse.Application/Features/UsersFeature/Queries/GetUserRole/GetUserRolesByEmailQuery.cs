using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.UsersFeature.Queries.GetUserRole;

public record GetUserRolesByEmailQuery(string Email): IQuery<string>;
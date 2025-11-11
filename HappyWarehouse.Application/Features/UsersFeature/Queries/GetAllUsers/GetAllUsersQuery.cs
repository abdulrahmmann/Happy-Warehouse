using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.UsersFeature.DTOs;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.UsersFeature.Queries.GetAllUsers;

public record GetAllUsersQuery(): IQuery<UserResponse<IEnumerable<UserDetailsDto>>>;
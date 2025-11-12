using HappyWarehouse.Application.Common;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.WarehouseFeature.Commands.RestoreWarehouse;

public record RestoreWarehouseCommand(int Id, string? RestoreBy): ICommand<BaseResponse<string>>;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.WarehouseFeature.Commands.DeleteWarehouse;

public record SoftDeleteWarehouseCommand(int Id): ICommand<BaseResponse<string>>;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.CountryFeature.DTOs;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.CountryFeature.Commands.UpdateCountry;

public record UpdateCountryCommand(int Id, UpdateCountryDto CountryDto): ICommand<BaseResponse<string>>;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.CountryFeature.DTOs;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.CountryFeature.Commands.CreateCountry;

public record CreateCountryCommand(CreateCountryDto CountryDto): ICommand<BaseResponse<string>>;
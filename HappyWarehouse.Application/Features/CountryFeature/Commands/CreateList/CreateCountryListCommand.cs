using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.CountryFeature.DTOs;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.CountryFeature.Commands.CreateList;

public record CreateCountryListCommand(IEnumerable<CreateCountryDto> CountriesDto): ICommand<BaseResponse<string>>;
using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.CountryFeature.DTOs;
using HappyWarehouse.Domain.CQRS;

namespace HappyWarehouse.Application.Features.CountryFeature.Queries.GetAll;

public record GetAllCountriesQuery(): IQuery<BaseResponse<IEnumerable<CountryDto>>>;
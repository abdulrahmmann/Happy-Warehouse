using HappyWarehouse.Application.Common;
using HappyWarehouse.Application.Features.CountryFeature.DTOs;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Infrastructure.UOF;

namespace HappyWarehouse.Application.Features.CountryFeature.Queries.GetAll;

public class GetAllCountriesQueryHandler(IUnitOfWork unitOfWork)
    : IQueryHandler<GetAllCountriesQuery, BaseResponse<IEnumerable<CountryDto>>>
{
    public async Task<BaseResponse<IEnumerable<CountryDto>>> HandleAsync(GetAllCountriesQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            var countries = await unitOfWork.GetRepository<Country>().GetAllAsync();

            var list = countries.ToList();
            
            if (list.Count == 0)
            {
                return BaseResponse<IEnumerable<CountryDto>>.NoContent("No countries found");
            }

            var countriesMapped = list.Select(country => new CountryDto(country.Id, country.Name));
            
            return BaseResponse<IEnumerable<CountryDto>>
                .Success(totalCount:list.Count, data:countriesMapped, message:"countries retrieved successfully");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BaseResponse<IEnumerable<CountryDto>>.InternalError();
        }
    }
}
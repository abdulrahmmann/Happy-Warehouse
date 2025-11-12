using HappyWarehouse.Application.Common;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Infrastructure.UOF;

namespace HappyWarehouse.Application.Features.CountryFeature.Commands.CreateList;

public class CreateCountryListCommandHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<CreateCountryListCommand, BaseResponse<string>>
{
    public async Task<BaseResponse<string>> HandleAsync(CreateCountryListCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var countries = command.CountriesDto.Select(country => new Country(country.Name, country.CreatedBy ?? "Seeder"));

            var enumerable = countries.ToList();
            
            if (enumerable.Count == 0) return BaseResponse<string>.NoContent();

            await unitOfWork.GetRepository<Country>().AddRangeAsync(enumerable);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return BaseResponse<string>.Created();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BaseResponse<string>.InternalError();
        }
    }
}
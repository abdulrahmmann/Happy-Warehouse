using HappyWarehouse.Application.Common;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Infrastructure.UOF;
using Serilog;

namespace HappyWarehouse.Application.Features.CountryFeature.Commands.CreateCountry;

public class CreateCountryCommandHandler(IUnitOfWork unitOfWork, ILogger logger): ICommandHandler<CreateCountryCommand, BaseResponse<string>>
{
    public async Task<BaseResponse<string>> HandleAsync(CreateCountryCommand command, CancellationToken cancellationToken = default)
    {
        var request = command.CountryDto;
        
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentNullException(nameof(request.Name), "Country name is required.");
        
        using var transaction = unitOfWork.BeginTransaction();
        
        try
        {
            var existingCountry = await unitOfWork.GetCountryRepository.FindAsync(c => c.Name == request.Name);

            if (existingCountry is not null)
            {
                return BaseResponse<string>.Conflict($"Country '{request.Name}' is already exists.");
            }
            
            var country = Country.Create(request.Name, request.CreatedBy);

            await unitOfWork.GetRepository<Country>().AddAsync(country);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            transaction.Commit();

            logger.Information("Country '{CountryName}' created successfully with ID: {CountryId}", country.Name, country.Id);

            return BaseResponse<string>.Created($"Country with Name: {country.Name} created successfully");
        }
        catch (Exception e)
        {
            logger.Information("Unexpected server error. Please try again later");
            transaction.Rollback();
            return BaseResponse<string>.InternalError("Unexpected server error. Please try again later");
        }
    }
}
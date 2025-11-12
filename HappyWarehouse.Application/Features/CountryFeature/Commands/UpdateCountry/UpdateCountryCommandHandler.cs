using HappyWarehouse.Application.Common;
using HappyWarehouse.Domain.CQRS;
using HappyWarehouse.Infrastructure.UOF;
using Serilog;

namespace HappyWarehouse.Application.Features.CountryFeature.Commands.UpdateCountry;

public class UpdateCountryCommandHandler(IUnitOfWork unitOfWork, ILogger logger): ICommandHandler<UpdateCountryCommand, BaseResponse<string>>
{
    public async Task<BaseResponse<string>> HandleAsync(UpdateCountryCommand command, CancellationToken cancellationToken = default)
    {
        var request = command.CountryDto;

        if (string.IsNullOrWhiteSpace(request.Name)) return BaseResponse<string>.ValidationError("Name is required");
        
        using var transaction = unitOfWork.BeginTransaction();
        
        try
        {
            var existingCountry = await unitOfWork.GetCountryRepository.FirstOrDefaultAsync(c => c.Id == command.Id, cancellationToken);

            if (existingCountry == null)
            {
                logger.Warning($"Country with ID {command.Id} is not found.");
                return BaseResponse<string>.NotFound("Country does not exist.");
            }

            var duplicate = await unitOfWork.GetCountryRepository
                .FirstOrDefaultAsync(c => c.Name.Equals(request.Name, StringComparison.CurrentCultureIgnoreCase), cancellationToken);
            
            if (duplicate != null)
            {
                return BaseResponse<string>.Conflict("A country with the same name already exists.");
            }
            
            existingCountry.Update(request.Name, request.UpdatedBy);

            await unitOfWork.GetCountryRepository.UpdateAsync(existingCountry);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            transaction.Commit();
            
            logger.Information($"Country with ID {command.Id} updated successfully.");

            return BaseResponse<string>.Success("Country updated successfully.");
        }
        catch (Exception e)
        {
            logger.Information("Unexpected server error. Please try again later");
            transaction.Rollback();
            return BaseResponse<string>.InternalError("Unexpected server error. Please try again later");
        }
    }
}
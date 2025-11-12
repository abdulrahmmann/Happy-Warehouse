using HappyWarehouse.Domain.Entities;

namespace HappyWarehouse.Domain.IRepository;

public interface ICountryRepository: IGenericRepository<Country>
{
    Task UpdateAsync(Country country, string? modifiedBy = null);

    Task SoftlyDelete(int id, string? deletedBy = null);
    
    Task Restore(int id, string? restoredBy = null);
}
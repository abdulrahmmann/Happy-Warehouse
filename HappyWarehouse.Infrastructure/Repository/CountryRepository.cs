using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Domain.IRepository;
using HappyWarehouse.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HappyWarehouse.Infrastructure.Repository;

public class CountryRepository: GenericRepository<Country>, ICountryRepository
{
    #region Instance Fields
    private readonly ApplicationDbContext _dbContext;
    #endregion

    #region Constructor
    public CountryRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    #endregion
    
    public async Task UpdateAsync(Country country, string? modifiedBy = null)
    {
        var existingCountry = await _dbContext.Countries.FirstOrDefaultAsync(c => c.Id == country.Id);

        if (existingCountry == null) throw new KeyNotFoundException($"Country with ID {country.Id} not found.");
        
        existingCountry.Update(country.Name, modifiedBy);
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task SoftlyDelete(int id, string? deletedBy = null)
    {
        var existingCountry = await _dbContext.Countries.FirstOrDefaultAsync(c => c.Id == id);

        if (existingCountry == null) throw new KeyNotFoundException($"Country with ID {id} not found."); 
        
        existingCountry.SoftDelete(deletedBy);
    }

    public async Task Restore(int id, string? restoredBy = null)
    {
        var existingCountry = await _dbContext.Countries.FirstOrDefaultAsync(c => c.Id == id);

        if (existingCountry == null) throw new KeyNotFoundException($"Country with ID {id} not found."); 
        
        existingCountry.Restore(restoredBy);
    }
}
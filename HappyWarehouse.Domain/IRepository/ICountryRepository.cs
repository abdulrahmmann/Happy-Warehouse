using System.Linq.Expressions;
using HappyWarehouse.Domain.Entities;

namespace HappyWarehouse.Domain.IRepository;

public interface ICountryRepository: IGenericRepository<Country>
{
    /// <summary> Method to Update Country. </summary>
    Task UpdateAsync(Country country, string? modifiedBy = null);

    /// <summary> Method to Softly Delete Country. </summary>
    Task SoftlyDeleteAsync(int id, string? deletedBy = null);
    
    /// <summary> Method to Restore Country. </summary>
    Task RestoreAsync(int id, string? restoredBy = null);

    public Task<Country> FindAsync(Expression<Func<Country, bool>> predicate);
}
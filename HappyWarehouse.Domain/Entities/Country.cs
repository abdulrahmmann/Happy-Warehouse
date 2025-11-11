using HappyWarehouse.Domain.Bases;

namespace HappyWarehouse.Domain.Entities;

/// <summary>
/// acts as a Lookup Countries to Represent the Warehouse country.
/// </summary>
public class Country: Entity<int>
{
    /// <summary>
    /// Represent the country name.
    /// </summary>
    public string Name { get; private set; } = null!;

    /// <summary>
    /// Navigation Property, One country can have many warehouses. 
    /// </summary>
    public ICollection<Warehouse> Warehouses = [];

    private Country() {}

    public Country(string name)
    {
        Name = name;
    }
    
    #region Create Country.
    /// <summary> Method to Create Country. </summary>
    public Country Create(string countryName, string? createdBy = null)
    {
        if (string.IsNullOrWhiteSpace(countryName))
        {
            throw new ArgumentNullException(nameof(countryName));
        }
        
        var country = new Country(countryName);

        MarkCreated(createdBy);
        
        return country;
    }
    #endregion

    #region Update Country.
    /// <summary> Method to Update Country. </summary>
    public void Update(string countryName, string? modifiedBy = null)
    {
        if (string.IsNullOrWhiteSpace(countryName))
        {
            throw new ArgumentNullException(nameof(countryName));
        }
        
        Name = countryName;
        
        MarkModified(modifiedBy);
    }
    #endregion

    #region Softly Delete Country.
    /// <summary> Method to Softly Delete Country. </summary>
    public void SoftDelete(string? deletedBy = null)
    {
        MarkDeleted(deletedBy);
    }
    #endregion

    #region Restore Deleted Country.
    /// <summary> Method to Restore Deleted Country. </summary>
    public void Restore(string? restoredBy = null)
    {
        MarkRestored(restoredBy);
    }
    #endregion
}
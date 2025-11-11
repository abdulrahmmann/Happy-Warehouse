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
}
using HappyWarehouse.Domain.Bases;
using HappyWarehouse.Domain.IdentityEntities;

namespace HappyWarehouse.Domain.Entities;

/// <summary>
/// Represents a warehouse entity containing items.
/// </summary>
public class Warehouse: Entity<int>
{
    /// <summary> Represent the Warehouse name or title. </summary>
    public string Name { get; private set; } = null!; 
    
    /// <summary> Represent the Address for the Warehouse. </summary>
    public string Address { get; private set; } = null!; 
    
    /// <summary> Represent the City for the Warehouse. </summary>
    public string City { get; private set; } = null!; 
    
    /// <summary> Foreign Key: Represent the CountryId for the Warehouse. </summary>
    public int CountryId { get; private set; }
    
    /// <summary> Navigation Property: the warehouse is located in one country </summary>
    public Country Country { get; private set; } = null!;
    
    /// <summary> Navigation Property: Represent one warehouse can contain many items. </summary>
    public ICollection<WarehouseItem> WarehouseItems { get; set; } = [];
    
    /// <summary> Foreign Key: Represent the user who create the warehouse. </summary>
    public int? CreatedByUserId { get; private set; }

    /// <summary> Navigation Property: Represent the user who create the warehouse. </summary>
    public ApplicationUser? CreatedByUser { get; private set; } = null!;
}
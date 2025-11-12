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
    
    private Warehouse() { }
    public Warehouse(string name, string address, string city, int countryId, int? createdByUserId)
    {
        Name = name;
        Address = address;
        City = city;
        CountryId = countryId;
        CreatedByUserId = createdByUserId;
    }
    
    #region Create Warehouse.
    /// <summary> Method to Create Warehouse. </summary>
    public static Warehouse Create(string name, string address, string city, int countryId, int? createdByUserId = null, string? createdByUser = null)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrWhiteSpace(address)) throw new ArgumentNullException(nameof(address));
        if (string.IsNullOrWhiteSpace(city)) throw new ArgumentNullException(nameof(city));

        var warehouse = new Warehouse
        {
            Name = name,
            Address = address,
            City = city,
            CountryId = countryId,
            CreatedByUserId = createdByUserId
        };
        
        warehouse.MarkCreated(createdByUser);
        
        return warehouse;
    }
    #endregion

    #region Update Warehouse.
    /// <summary> Method to Update Warehouse. </summary>
    public void Update(string name, string address, string city, int countryId, string? modifiedBy = null)
    {
        if (!string.IsNullOrWhiteSpace(name)) Name = name;
        if (!string.IsNullOrWhiteSpace(address)) Address = address;
        if (!string.IsNullOrWhiteSpace(city)) City = city;
        CountryId = countryId;

        MarkModified(modifiedBy);
    }
    #endregion

    #region Softly Delete Warehouse.
    /// <summary> Method to Softly Delete Warehouse. </summary>
    public void SoftDelete(string? deletedBy = null)
    {
        MarkDeleted(deletedBy);
    }
    #endregion

    #region Restore Deleted Warehouse.
    /// <summary> Method to Restore Deleted Warehouse. </summary>
    public void Restore(string? restoredBy = null)
    {
        MarkRestored(restoredBy);
    }
    #endregion
    
    public WarehouseItem AddWarehouseItem(string itemName, string? skuCode, int qty, decimal costPrice, decimal? msrpPrice, int warehouseId, int? createdByUserId, string? createdByUser)
    {
        if (string.IsNullOrWhiteSpace(itemName)) throw new ArgumentNullException(nameof(itemName));
        if (qty < 1) throw new ArgumentOutOfRangeException(nameof(qty));

        var item = new WarehouseItem(itemName, skuCode, qty, costPrice, msrpPrice, this.Id, createdByUserId);

        WarehouseItems.Add(item);
        MarkCreated(createdByUser);
        return item;
    }

}
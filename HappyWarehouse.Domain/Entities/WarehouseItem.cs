using HappyWarehouse.Domain.Bases;
using HappyWarehouse.Domain.IdentityEntities;

namespace HappyWarehouse.Domain.Entities;

/// <summary>
/// Represents an item stored in a specific warehouse within the system.
/// Each item belongs to exactly one warehouse.
/// </summary>
public class WarehouseItem: Entity<int>
{
    /// <summary>
    /// Unique name of the item.
    /// <para>Required and must be unique within the system.</para>
    /// </summary>
    public string ItemName { get; private set; } = null!; 
    
    /// <summary>
    /// SKU (Stock Keeping Unit) code used to identify the item.
    /// <para>This field is optional.</para>
    /// </summary>
    public string? SkuCode { get; private set; }

    /// <summary>
    /// Represent the available quantity of the item in stock.
    /// <para>Must be an integer greater than or equal to 1.</para>
    /// </summary>
    public int Qty { get; private set; } = 1;
    
    /// <summary>
    /// Represent the purchase price of the item.
    /// <para>Required field.</para>
    /// </summary>
    public decimal CostPrice { get; private set; }

    /// <summary>
    /// Gets or sets the Manufacturer's Suggested Retail Price (MSRP) of the item.
    /// <para>This field is optional.</para>
    /// </summary>
    public decimal? MsrpPrice { get; private set; }

    /// <summary>  Represent the Foreign Key referencing the related warehouse. </summary>
    public int WarehouseId { get; private set; }

    /// <summary> Navigation Property that represents the warehouse this item belongs to. </summary>
    public Warehouse Warehouse { get; private set; } = null!;
    
    /// <summary> Foreign Key: Represent the user who create the warehouse. </summary>
    public int? CreatedByUserId { get; private set; }

    /// <summary> Navigation Property: Represent the user who create the warehouse. </summary>
    public ApplicationUser? CreatedByUser { get; private set; } = null!;
}
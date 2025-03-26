using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventiCloud.Entities;

public class Inventory
{
    [Key]
    public int InventoryId { get; set; }

    [ForeignKey("Product")]
    public int ProductId { get; set; }

    [ForeignKey("Branch")]
    public int BranchId { get; set; }
    public int OnHandquantity { get; set; } = 0;
    public int IncomingQuantity { get; set; } = 0;
    public int AvailableQuantity { get; set; } = 0;
    public int OutgoingQuantity { get; set; } = 0;

 // navigation properties
    public virtual Product Product { get; set; }
    public virtual Branch Branch { get; set; }

}

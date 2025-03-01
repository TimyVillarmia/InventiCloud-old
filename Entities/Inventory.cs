using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventiCloud.Entities;

public class Inventory
{
    [Key]
    public int InventroyId { get; set; }

    [ForeignKey("Product")]
    public int ProductID { get; set; }

    [ForeignKey("Branch")]
    public int BranchID { get; set; }
    public int OnHandquantity { get; set; } = 0;
    public int IncomingQuantity { get; set; } = 0;
    public int AvailableQuantity { get; set; } = 0;
    public int OutgoingQuantity { get; set; } = 0;

 // navigation properties
    public virtual Product Product { get; set; }
    public virtual Branch Branch { get; set; }

}

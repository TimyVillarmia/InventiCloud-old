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

    [Display(Name = "Onhand Quantity")]
    public int OnHandquantity { get; set; } = 0;

    [Display(Name = "Incoming Quantity")]
    public int IncomingQuantity { get; set; } = 0;

    [Display(Name = "Available Quantity")]
    public int AvailableQuantity { get; set; } = 0;

    [Display(Name = "Allocated")]
    public int Allocated { get; set; } = 0;

 // navigation properties
    public virtual Product Product { get; set; }
    public virtual Branch Branch { get; set; }

}

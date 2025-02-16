using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventiCloud.Models;

public class Inventory
{
    [Key]
    public int InventroyId { get; set; }

    public int ProductID { get; set; }

    public int BranchID { get; set; }
    public int OnHand { get; set; } = 0;
    public int Incoming { get; set; } = 0;
    public int Unavailable { get; set; } = 0;

 // navigation properties
    public virtual Product Product { get; set; }
    public virtual Branch Branch { get; set; }

}

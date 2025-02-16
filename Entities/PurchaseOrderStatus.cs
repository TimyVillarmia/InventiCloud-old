using System;
using System.ComponentModel.DataAnnotations;

namespace InventiCloud.Models;

public class PurchaseOrderStatus
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }


    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }


}

using System;
using System.ComponentModel.DataAnnotations;

namespace InventiCloud.Entities;

public class PurchaseOrderStatus
{
    [Key]
    public int PurchaseOrderStatusId { get; set; }
    public string StatusName { get; set; }


    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }


}

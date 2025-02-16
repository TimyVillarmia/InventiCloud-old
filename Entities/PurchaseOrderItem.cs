using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InventiCloud.Models;

public class PurchaseOrderItem
{
    [Key]
    public int PurchaseOrderItemId { get; set; }

    public int PurchaseOrderID { get; set; }

    public int ProductID { get; set; }
    public int Quantity { get; set; }

    [Precision(19, 4)]
    public decimal Price { get; set; }
    [Precision(19, 4)]
    public decimal SubTotal { get; set; }

    public virtual PurchaseOrder PurchaseOrder { get; set; }
    public virtual Product Product { get; set; }


}

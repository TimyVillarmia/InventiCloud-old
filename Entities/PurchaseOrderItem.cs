using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InventiCloud.Entities;

public class PurchaseOrderItem
{
    [Key]
    public int PurchaseOrderItemId { get; set; }

    [Required,
     ForeignKey("PurchaseOrder")]
    public int PurchaseOrderID { get; set; }


    [Required,
     ForeignKey("Product")]
    public int ProductID { get; set; }
    public int Quantity { get; set; }

    [Precision(19, 2)]
    public decimal UnitPrice { get; set; }
    [Precision(19, 2)]
    public decimal SubTotal { get; set; }

    public virtual PurchaseOrder PurchaseOrder { get; set; }
    public virtual Product Product { get; set; }


}

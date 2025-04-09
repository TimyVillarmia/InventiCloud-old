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

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
    [Required(ErrorMessage = "Quantity is required.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Precision(19, 2)]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    [Display(Name = "Unit Price")]
    public decimal UnitPrice { get; set; }

    [Required, Precision(19, 2)]
    public decimal SubTotal { get; set; }

    public virtual PurchaseOrder PurchaseOrder { get; set; }
    public virtual Product Product { get; set; }


}

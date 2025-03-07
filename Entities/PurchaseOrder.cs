using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InventiCloud.Data;
using Microsoft.EntityFrameworkCore;

namespace InventiCloud.Entities;

public class PurchaseOrder
{
    [Key]
    public int PurchaseOrderId { get; set; }

    [Required,
     ForeignKey("Supplier")]
    public int SupplierID { get; set; }

    [Required,
     ForeignKey("DestinationBranch")]
    public int DestinationBranchId { get; set; }

    [Required,
     ForeignKey("PurchaseOrderStatus")]
    public int StatusId { get; set; } = 1;

    [Required,
     ForeignKey("CreatedBy")]
    public string CreatedById { get; set; }

    [Required,
    Precision(19, 2)]
    public decimal TotalAmount { get; set; }


    public string? ReferenceNumber { get; set; }
    public DateTime EstimatedArrival { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime PurchasedDate { get; set; } 
    public DateTime? ReceivedDate { get; set; }


    public virtual ApplicationUser CreatedBy { get; set; }

    public virtual Supplier Supplier { get; set; }
    public virtual Branch DestinationBranch { get; set; }
    public virtual PurchaseOrderStatus PurchaseOrderStatus { get; set; }
    public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }

}

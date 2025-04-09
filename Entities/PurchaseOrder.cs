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
    [Display(Name = "Supplier")]
    public string SupplierCode { get; set; }

    [Required,
     ForeignKey("DestinationBranch")]
    [Display(Name = "Destination Branch")]
    public int DestinationBranchId { get; set; }

    [Required,
     ForeignKey("PurchaseOrderStatus")]
    public int StatusId { get; set; } = 1;

    [Required,
     ForeignKey("CreatedBy")]
    public string CreatedById { get; set; }

    [Required,
    Precision(19, 2)]
    [Display(Name = "Total amount")]
    public decimal TotalAmount { get; set; }

    [Display(Name = "Reference Number")]
    public string? ReferenceNumber { get; set; }

    [Display(Name = "Estimated Arrival")]
    public DateTime? EstimatedArrival { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime? PurchasedDate { get; set; } 
    public DateTime? ReceivedDate { get; set; }


    public virtual ApplicationUser CreatedBy { get; set; }

    public virtual Supplier Supplier { get; set; }
    public virtual Branch DestinationBranch { get; set; }
    public virtual PurchaseOrderStatus PurchaseOrderStatus { get; set; }
    public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }

}

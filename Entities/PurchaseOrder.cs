using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InventiCloud.Data;

namespace InventiCloud.Entities;

public class PurchaseOrder
{
    [Key]
    public int PurchaseOrderId { get; set; }

    [Required,
     ForeignKey("Supplier")]
    public int SupplierID { get; set; }

    [Required,
     ForeignKey("Branch")]
    public int DestinationBranch { get; set; }

    [Required,
     ForeignKey("PurchaseOrderStatus")]
    public int StatusId { get; set; }

    [Required,
     ForeignKey("ApplicationUser")]
    public string CreatedBy { get; set; }


    [Required]
    public string ReferenceNumber { get; set; }
    public DateTime EstimatedArrival { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime PurchasedDate { get; set; } 
    public DateTime? ReceivedDate { get; set; }


    public virtual ApplicationUser ApplicationUser { get; set; }

    public virtual Supplier Supplier { get; set; }
    public virtual Branch Branch { get; set; }
    public virtual PurchaseOrderStatus PurchaseOrderStatus { get; set; }
    public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }

}

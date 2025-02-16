using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InventiCloud.Data;

namespace InventiCloud.Models;

public class PurchaseOrder
{
    [Key]
    public int PurchaseOrderId { get; set; }

    [Required]
    public int SupplierID { get; set; }

    [Required]
    public int DestinationBranch { get; set; }

    public int StatusId { get; set; }


    [Required]
    public string ReferenceNumber { get; set; }
    public DateTime PurchasedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ReceivedDate { get; set; }

    public string CreatedBy { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }

    public virtual Supplier Supplier { get; set; }
    public virtual Branch Branch { get; set; }
    public virtual PurchaseOrderStatus Status { get; set; }
    public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }

}

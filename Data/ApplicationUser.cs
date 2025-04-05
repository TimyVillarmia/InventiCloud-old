using InventiCloud.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventiCloud.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [ForeignKey("Branch")]
    public int? BranchId { get; set; }
    public virtual Branch? Branch { get; set; }

    // navigation properties
    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
    //public virtual ICollection<StockTransfer> StockTransfers { get; set; }
    //public virtual ICollection<StockAdjustment> StockAdjustments { get; set; }
    //public virtual ICollection<SalesOrder> SalesOrders { get; set; }


}


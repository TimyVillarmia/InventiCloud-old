using InventiCloud.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace InventiCloud.Entities;

public class Branch
{
    [Key]
    public int BranchId { get; set; }
    public string BranchName { get; set; } 
    public string Country { get; set; }
    public string Address { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string Region { get; set; }
    public string PhoneNumber { get; set; }

    // navigation properties
    public virtual ICollection<Inventory> Inventories { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }


}

using InventiCloud.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace InventiCloud.Entities;

[Index(nameof(BranchName), IsUnique = true)]
[Index(nameof(PhoneNumber), IsUnique = true)]
public class Branch
{
    [Key]
    public int BranchId { get; set; }

    [Required]
    [Display(Name = "Branch Name")]
    public string BranchName { get; set; } 
    public string Address { get; set; }

    [Required]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }

    // navigation properties
    public virtual ICollection<Inventory> Inventories { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }


}

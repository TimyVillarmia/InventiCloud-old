using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace InventiCloud.Entities;

[Index(nameof(SupplierCode), IsUnique = true)]
public class Supplier
{

    [Key]
    [Display(Name = "Supplier Code")]
    public string SupplierCode { get; set; }

    [Required]
    [Display(Name = "Supplier Name")]
    public string SupplierName { get; set; }

    [Required]
    public string Company { get; set; }

    [Required]
    [Display(Name = "Contact Person")]
    public string ContactPerson { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }
    public string Address { get; set; }

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }


}

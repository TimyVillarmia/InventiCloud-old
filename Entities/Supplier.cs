using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace InventiCloud.Entities;

[Index(nameof(SupplierCode), IsUnique = true)]
public class Supplier
{

    [Key]
    public string SupplierCode { get; set; }

    [Required]
    public string SupplierName { get; set; }

    [Required]
    public string Company { get; set; }

    [Required]
    public string ContactPerson { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string PhoneNumber { get; set; }
    public string Country { get; set; }
    public string Address { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string Region { get; set; }

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }


}

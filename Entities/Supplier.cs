using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace InventiCloud.Models;

[PrimaryKey(nameof(SupplierId))]
public class Supplier
{
    [Key]
    public int SupplierId { get; set; }

    [Required]
    public string Company { get; set; }

    [Required]
    public string ContactPerson { get; set; }

    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Country { get; set; }
    public string Address { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string Region { get; set; }

}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InventiCloud.Entities;


[Index(nameof(PhoneNumber), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class Customer
{
    [Key]
    public int CustomerId { get; set; }

    [Required]
    [Display(Name = "Customer Name")]
    public string CustomerName { get; set; }
    public string Address { get; set; }

    [MaxLength(20)]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [MaxLength(255)]
    [EmailAddress]
    public string? Email { get; set; }

    public DateTime? BirthDate { get; set; }

    public string? Occupation { get; set; }

    public virtual ICollection<SalesOrder> SalesOrders { get; set; }

}
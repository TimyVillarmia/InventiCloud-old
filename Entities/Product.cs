using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InventiCloud.Entities;
using InventiCloud.Utils;
using Microsoft.EntityFrameworkCore;

namespace InventiCloud.Entities;

[Index(nameof(SKU), IsUnique = true)]
public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required,
    Range(1, int.MaxValue , ErrorMessage = "You must select a category for this product."),
    ForeignKey("Category")]
    public int CategoryId { get; set; }

    [ForeignKey("AttributeSet")]
    public int? AttributeSetId { get; set; }

    [Required]
    public string ProductName { get; set; }
    public string? ImageURL { get; set; }

    public string? Brand { get; set; }
    public string? Description { get; set; }

    [Required,
    Precision(19, 2)]
    public decimal UnitCost { get; set; }

    [Required,
    SellingPriceCostValidation(nameof(UnitCost), nameof(UnitPrice)),
    Precision(19, 2)]
    public decimal UnitPrice { get; set; }

    [Required]
    public string SKU { get; set; }

    [Column(TypeName = "bit")]
    public bool isActive { get; set; } = true;


    // navigation properties
    public virtual Category Category { get; set; }
    public virtual AttributeSet? AttributeSet { get; set; }
    public virtual ICollection<Inventory> Inventories { get; set; }
    public virtual ICollection<ProductAttributeValue> ProductAttributeValues { get; set; } 


}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InventiCloud.Utils;
using Microsoft.EntityFrameworkCore;

namespace InventiCloud.Models;

[Index(nameof(SKU), IsUnique = true)]
public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required,
    Range(1, int.MaxValue , ErrorMessage = "You must select a category for this product.")]
    public int CategoryId { get; set; }

    [Required]
    public string Name { get; set; }
    public string? ImageURL { get; set; }

    public string? Brand { get; set; }
    public string? Description { get; set; }

    [Required,
    Precision(19, 2)]
    public decimal Cost { get; set; }

    [Required,
    SellingPriceCostValidation(nameof(Cost), nameof(Price)),
    Precision(19, 2)]
    public decimal Price { get; set; }

    [Required]
    public string SKU { get; set; }

    public double? WeightValue { get; set; }
    public string? WeightUnit { get; set; }

    [Column(TypeName = "bit")]
    public bool isActive { get; set; } = true;


    // navigation properties
    public virtual Category Category { get; set; }
    public virtual ICollection<Inventory> Inventories { get; set; }


}

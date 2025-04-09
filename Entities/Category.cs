using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace InventiCloud.Entities;

[Index(nameof(CategoryName), IsUnique = true)]
public class Category
{
    [Key,
    Required(ErrorMessage = "Please Select a category")]
    public int CategoryId { get; set; }

    [Required]
    [Display(Name = "Category Name")]
    public string CategoryName { get; set; }

     // navigation properties
    public virtual ICollection<Product> Products {get;set;}

}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventiCloud.Models;

public class ProductAttribute
{
    [Key]
    public int ProductAttributeId {get;set;}

    public int ProductId { get; set; }

    public int? AttributeSetId { get; set; }

    public virtual Product Product { get; set; }
    public virtual AttributeSet? AttributeSet { get; set; }

}

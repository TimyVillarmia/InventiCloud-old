using System;
using System.ComponentModel.DataAnnotations;

namespace InventiCloud.Entities;

public class AttributeSet
{
    [Key]
    public int AttributeSetId { get; set; }

    [Required,
    Display(Name = "Attribute Set Name")]
    public string? AttributeSetName { get; set; }

    public virtual ICollection<Attribute> Attributes { get; set; }

}

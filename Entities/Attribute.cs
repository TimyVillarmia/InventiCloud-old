using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventiCloud.Entities;

public class Attribute
{
    [Key]
    public int AttributeId { get; set; }

    [Required,
    Display(Name = "Attribute name")]
    public string AttributeName { get; set; } 

    [ForeignKey("AttributeSet")]
    public int AttributeSetId { get; set; }

    [Column(TypeName = "bit")]
    public bool isRequired { get; set; } = false;


    public virtual AttributeSet AttributeSet { get; set; } 

}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventiCloud.Models;

public class Attribute
{
    [Key]
    public int AttributeId { get; set; }

    [ForeignKey("AttributeSet")]
    public int AttributeSetId { get; set; }


    [Required,
    Display(Name = "Attribute name")]
    public string AttributeName { get; set; }


    public virtual AttributeSet AttributeSet { get; set; }

}

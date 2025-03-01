using InventiCloud.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventiCloud.Entities;

public class ProductAttributeValue
    {
        [Key]
        public int ProductAttributeValueId { get; set; }

        [ForeignKey("ProductId")]
        public int ProductId { get; set; }

        [ForeignKey("Attribute")]
        public int AttributeId { get; set; }

        [ForeignKey("AttributeValue")]
        public int AttributeValueId { get; set; }


        // navigation properties
        public virtual Product Product { get; set; }
        public virtual Attribute Attribute { get; set; }
        public virtual AttributeValue AttributeValue { get; set; }

    }
using System.ComponentModel.DataAnnotations;

namespace InventiCloud.Entities;

public class AttributeValue
    {
        [Key]
        public int AttributeValueId { get; set; }
        public int Value { get; set; }
    }
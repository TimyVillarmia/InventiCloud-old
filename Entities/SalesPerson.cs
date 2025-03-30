using System.ComponentModel.DataAnnotations;

namespace InventiCloud.Entities
{
    public class SalesPerson
    {
        [Key]
        public int SalesPersonId { get; set; }

        [Required]
        public string FullName { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventiCloud.Entities
{
    public class SalesOrderItem
    {
        [Key]
        public int SalesOrderItemId { get; set; }

        [ForeignKey("SalesOrder")]
        public int SalesOrderId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        [Range(0, 1, ErrorMessage = "Discount must be between 0 and 1.")]
        [Precision(19, 4)]
        public decimal? Discount { get; set; }

        [Required(ErrorMessage = "Unit Price is required.")]
        [Precision(19, 2)]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit Price must be greater than 0.")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Subtotal is required.")]
        [Precision(19, 2)]
        public decimal SubTotal { get; set; }

        // Navigation properties
        public virtual SalesOrder SalesOrder { get; set; }
        public virtual Product Product { get; set; }
    }

}


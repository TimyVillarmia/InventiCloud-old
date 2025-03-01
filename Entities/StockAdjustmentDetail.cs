using InventiCloud.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventiCloud.Entities
{
    public class StockAdjustmentDetail
    {
        [Key]
        public int StockAdjustmentDetailId { get; set; }

        [ForeignKey("StockAdjustment")]
        public int StockAdjustmentId { get; set; }

        [ForeignKey("Inventory")]
        public int InventoryId { get; set; }

        [Required(ErrorMessage = "Previous Quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Previous Quantity must be a non-negative number.")]
        public int PreviousQuantity { get; set; }

        [Required(ErrorMessage = "New Quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "New Quantity must be a non-negative number.")]
        public int NewQuantity { get; set; }

        public int AdjustedQuantity { get; set; }


        // navigation properties
        public virtual StockAdjustment StockAdjustment { get; set; }
        public virtual Inventory Inventory { get; set; }

    }
}

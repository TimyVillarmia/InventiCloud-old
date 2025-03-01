using InventiCloud.Entities;
using System.ComponentModel.DataAnnotations;

namespace InventiCloud.Entities
{
    public class StockAdjustmentReason
    {
        [Key]
        public int StockAdjustmentReasonId { get; set; }
        public string Reason { get; set; }

    }
}

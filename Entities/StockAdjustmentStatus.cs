using System.ComponentModel.DataAnnotations;

namespace InventiCloud.Entities
{
    public class StockAdjustmentStatus
    {
        [Key]
        public int StockAdjustmentStatusId { get; set; }
        public string StatusName { get; set; }
    }
}

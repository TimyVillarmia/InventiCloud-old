using InventiCloud.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventiCloud.Entities
{
    public class StockAdjustment
    {
        [Key]
        public int StockAdjustmentId { get; set; }

        public string? ReferenceNumber { get; set; }

        [ForeignKey("SourceBranch")]
        public int SourceBranchId { get; set; }

        [ForeignKey("StockAdjustmentReason")]
        public int ReasonId { get; set; }

        [ForeignKey("StockAdjustmentStatus")]
        public int StatusId { get; set; }

        [ForeignKey("ApplicationUser")]
        public string AdjustedBy { get; set; }
        public DateTime AdjustedDate { get; set; }

        // navigation properties
        public virtual StockAdjustmentStatus StockAdjustmentStatus { get; set; }
        public virtual Branch SourceBranch { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual StockAdjustmentReason StockAdjustmentReason { get; set; }
        public ICollection<StockAdjustmentItem> StockAdjustmentItems { get; set; } 



    }
}

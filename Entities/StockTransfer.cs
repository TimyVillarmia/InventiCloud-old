using InventiCloud.Data;
using InventiCloud.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventiCloud.Entities
{
    public class StockTransfer
    {
        [Key]
        public int StockTransferId { get; set; }

        public string? ReferenceNumber { get; set; }

        [ForeignKey("SourceBranch")]
        public int SourceBranchId { get; set; }

        [ForeignKey("DestinationBranch")]
        public int DestinationBranchId { get; set; }

        [ForeignKey("Status")]
        public int StatusId { get; set; }
        public DateTime? RequestedDate { get; set; } = DateTime.Now;
        public DateTime? DateCompleted { get; set; }
        public DateTime? DateApproved { get; set; }
        public DateTime? RejectedDate { get; set; } // Added RejectedDate
        [Required,
         ForeignKey("RequestedBy")]
        public string RequestedById { get; set; } 
        
        [ForeignKey("ApprovedBy")]
        public string ApprovedById { get; set; }

        public virtual ApplicationUser RequestedBy { get; set; }
        public virtual ApplicationUser ApprovedBy { get; set; }

        [Required(ErrorMessage = "Source Branch is required.")]
        public virtual Branch SourceBranch { get; set; }

        [Required(ErrorMessage = "Destination Branch is required.")]
        public virtual Branch DestinationBranch { get; set; }
        public virtual StockTransferStatus Status { get; set; }
        public virtual ICollection<StockTransferItem> StockTransferItems { get; set; } 
    }
}

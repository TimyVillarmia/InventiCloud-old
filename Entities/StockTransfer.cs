using InventiCloud.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventiCloud.Entities
{
    public class StockTransfer
    {
        [Key]
        public int StockTransferId { get; set; }

        [ForeignKey("FromBranch")]
        public int FromBranchId { get; set; }

        [ForeignKey("ToBranch")]
        public int ToBranchId { get; set; }

        [ForeignKey("Status")]
        public int StatusId { get; set; }
        public DateTime TransferDate { get; set; }
        public DateTime ReceivedDate { get; set; }


        public virtual Branch FromBranch { get; set; } // Assuming you have a Branch entity
        public virtual Branch ToBranch { get; set; }
        public virtual StockTransferStatus Status { get; set; }
        public virtual ICollection<StockTransferDetail> Details { get; set; } 
    }
}

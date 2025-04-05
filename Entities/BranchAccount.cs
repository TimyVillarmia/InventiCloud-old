using InventiCloud.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventiCloud.Entities
{
    public class BranchAccount
    {
        [Key]
        public int BranchAccountId { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }

        [ForeignKey("Branch")]
        public int BranchId { get; set; }

        // navigation properties
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Branch Branch { get; set; }
        public virtual ICollection<StockTransfer> StockTransfers { get; set; }
    }
}

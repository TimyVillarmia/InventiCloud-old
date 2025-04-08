using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventiCloud.Entities
{
    public class StockTransferItem
    {
        [Key]
        public int StockTransferItemlId { get; set; }

        [ForeignKey("StockTransfer")]
        public int StockTransferId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public int TransferQuantity { get; set; }
        public int? PreviousQuantity { get; set; }


        // navigation properties
        public virtual StockTransfer StockTransfer { get; set; }
        public virtual Product Product { get; set; }
    }
}

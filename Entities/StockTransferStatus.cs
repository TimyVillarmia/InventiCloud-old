using System.ComponentModel.DataAnnotations;

namespace InventiCloud.Entities
{
    public class StockTransferStatus
    {
        [Key]
        public int StockTransferStatusId { get; set; }
        public string StatusName { get; set; }
    }
}

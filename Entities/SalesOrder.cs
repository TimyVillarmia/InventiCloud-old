using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventiCloud.Entities
{
    public class SalesOrder
    {
        [Key]
        public int SalesOrderId { get; set; }

        public string? ReferenceNumber { get; set; }

        [ForeignKey("OrderBranch")]
        public int OrderBranchId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [ForeignKey("SalesPerson")]
        public int SalesPersonId { get; set; }

        public DateTime OrderedDate { get; set; } = DateTime.Now;

        // navigation properties
        public virtual Branch OrderBranch { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual SalesPerson SalesPerson { get; set; }
        public virtual ICollection<SalesOrderItem> SalesOrderItems { get; set; }

    }
}

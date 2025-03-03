namespace InventiCloud.Utils
{
    public class PurchaseOrderGenerateReferenceNumber
    {
        public int PurchaseOrderId { get; set; }
        public string ReferenceNumber { get; set; }

        public static string GenerateReferenceNumber(int purchaseOrderId)
        {
            return $"PO-{purchaseOrderId:D8}"; // D8 formats as 8 digits with leading zeros
        }
    }
}

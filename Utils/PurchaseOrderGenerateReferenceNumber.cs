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

        public static int? ReferenceNumberToId(string referenceNumber)
        {
            if (string.IsNullOrEmpty(referenceNumber))
            {
                return null;
            }

            if (referenceNumber.StartsWith("PO-", StringComparison.OrdinalIgnoreCase) && referenceNumber.Length == 11) // PO- + 8 digits = 11
            {
                string idPart = referenceNumber.Substring(3); // Extract the 8 digits

                if (int.TryParse(idPart, out int purchaseOrderId))
                {
                    return purchaseOrderId;
                }
            }

            return null; // Or throw an exception if you prefer
        }
    }
}

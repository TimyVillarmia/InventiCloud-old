namespace InventiCloud.Utils
{
    public class ReferenceNumberGenerator
    {

        private const string PurchaseOrderPrefix = "PO-";
        private const string StockTransferPrefix = "ST-";
        private const string SalesOrderPrefix = "SO-"; 
        private const int IdLength = 8;

        public static string GeneratePurchaseOrderReference(int purchaseOrderId)
        {
            return $"{PurchaseOrderPrefix}{purchaseOrderId:D8}";
        }

        public static string GenerateStockTransferReference(int stockTransferId)
        {
            return $"{StockTransferPrefix}{stockTransferId:D8}";
        }

        public static string GenerateSalesOrderReference(int salesOrderId) // Add Sales Order Generator
        {
            return $"{SalesOrderPrefix}{salesOrderId:D8}";
        }

        public static int? ParsePurchaseOrderId(string referenceNumber)
        {
            return ParseId(referenceNumber, PurchaseOrderPrefix);
        }

        public static int? ParseStockTransferId(string referenceNumber)
        {
            return ParseId(referenceNumber, StockTransferPrefix);
        }

        public static int? ParseSalesOrderId(string referenceNumber) // Add Sales Order Parser
        {
            return ParseId(referenceNumber, SalesOrderPrefix);
        }

        private static int? ParseId(string referenceNumber, string prefix)
        {
            if (string.IsNullOrEmpty(referenceNumber))
            {
                return null;
            }

            if (referenceNumber.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) && referenceNumber.Length == prefix.Length + IdLength)
            {
                string idPart = referenceNumber.Substring(prefix.Length);

                if (int.TryParse(idPart, out int id))
                {
                    return id;
                }
            }

            return null;
        }
    }
}


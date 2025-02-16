using InventiCloud.Models;

namespace InventiCloud.Utils
{
    public class GetFullAddressUtil
    {
        public static string GetFullAddress(Branch branch)
        {
            string location = string.Empty;

            if (!string.IsNullOrEmpty(branch.Address))
            {
                location += branch.Address + ", ";
            }

            if (!string.IsNullOrEmpty(branch.City))
            {
                location += branch.City + ", ";
            }

            if (!string.IsNullOrEmpty(branch.Region))
            {
                location += branch.Region + ", ";
            }

            if (!string.IsNullOrEmpty(branch.PostalCode))
            {
                location += branch.PostalCode + " ";
            }

            if (!string.IsNullOrEmpty(branch.Country))
            {
                location += branch.Country;
            }

            // Remove trailing comma and space if any
            if (location.EndsWith(", "))
            {
                location = location.Substring(0, location.Length - 2);
            }

            return location;
        }
    }
}

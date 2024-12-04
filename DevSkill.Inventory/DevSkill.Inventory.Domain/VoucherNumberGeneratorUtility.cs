using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain
{
    public static class VoucherNumberGeneratorUtility
    {
        private const string VoucherPrefix = "STKISUE/";

        public static string GenerateNextVoucherNumber(string? lastVoucher)
        {
            int nextNumber = 1; // Default to 1 if no voucher exists

            if (!string.IsNullOrEmpty(lastVoucher) && lastVoucher.StartsWith(VoucherPrefix))
            {
                string[] parts = lastVoucher.Split('/');
                if (parts.Length == 2 && int.TryParse(parts[1], out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            // Format the new voucher number as "STKISUE/XX"
            return $"{VoucherPrefix}{nextNumber:D2}"; // E.g., "STKISUE/01"
        }
    }
}

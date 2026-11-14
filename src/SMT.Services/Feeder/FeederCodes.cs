using System.Linq;

namespace SMT.Services.Feeder
{
    // Normalization shared by import, slot generation and scanning, so "m 2" / "M2" and "07" / "7"
    // from an operator's Excel end up on the same slot. Mirrors model-changer, which stripped spaces
    // and upper-cased module, feeder and code values on import.
    public static class FeederCodes
    {
        public const string SlotCodePrefix = "FS-";

        public static string NormalizeModule(string module)
        {
            return Clean(module);
        }

        // Numeric feeder numbers lose leading zeros ("07" -> "7"); anything else (e.g. "12L" for a
        // twin-feeder lane) is kept as-is after cleaning.
        public static string NormalizeSlotNumber(string number)
        {
            var cleaned = Clean(number);

            if (cleaned.Length > 0 && cleaned.All(char.IsDigit))
            {
                var trimmed = cleaned.TrimStart('0');
                return trimmed.Length == 0 ? "0" : trimmed;
            }

            return cleaned;
        }

        public static string NormalizeCode(string code)
        {
            return Clean(code);
        }

        // FS-{lineId}-{module}-{number}, numeric numbers padded to two digits so stickers read evenly.
        public static string BuildSlotCode(int lineId, string module, string number)
        {
            var normalizedNumber = NormalizeSlotNumber(number);
            if (normalizedNumber.All(char.IsDigit) && normalizedNumber.Length == 1)
                normalizedNumber = "0" + normalizedNumber;

            return $"{SlotCodePrefix}{lineId}-{NormalizeModule(module)}-{normalizedNumber}";
        }

        public static bool IsSlotCode(string scanned)
        {
            return !string.IsNullOrWhiteSpace(scanned)
                && scanned.Trim().ToUpperInvariant().StartsWith(SlotCodePrefix);
        }

        private static string Clean(string value)
        {
            return (value ?? string.Empty).Replace(" ", string.Empty).Trim().ToUpperInvariant();
        }
    }
}

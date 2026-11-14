using System.Collections.Generic;
using System.Linq;

namespace SMT.Services.Feeder
{
    public class ParsedReelCode
    {
        public string Raw { get; set; }

        // Null when the code isn't in our label format.
        public string PartNumber { get; set; }

        public string RCode { get; set; }

        // Codes to try against the component catalog, most specific first.
        public List<string> Candidates { get; set; } = new List<string>();
    }

    // Reel labels printed by PrinterApp are "{PartNumber}@{RCode}@{PlaceCode}@{DateTime}". PartNumber
    // can be empty when PrinterApp found the component by RCode, so RCode is always tried first.
    // Anything without '@' (a bare part number / RCode, typed or from an Excel cell) is a single candidate.
    public static class ReelBarcodeParser
    {
        public static ParsedReelCode Parse(string raw)
        {
            var result = new ParsedReelCode { Raw = raw?.Trim() ?? string.Empty };

            if (result.Raw.Length == 0)
                return result;

            if (result.Raw.Contains('@'))
            {
                var parts = result.Raw.Split('@');

                result.PartNumber = NullIfEmpty(FeederCodes.NormalizeCode(parts[0]));
                result.RCode = parts.Length > 1 ? NullIfEmpty(FeederCodes.NormalizeCode(parts[1])) : null;

                result.Candidates = new[] { result.RCode, result.PartNumber }
                    .Where(c => c != null)
                    .Distinct()
                    .ToList();
            }
            else
            {
                result.Candidates.Add(FeederCodes.NormalizeCode(result.Raw));
            }

            return result;
        }

        private static string NullIfEmpty(string value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }
}

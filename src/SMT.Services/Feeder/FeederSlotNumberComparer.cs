using System;
using System.Collections.Generic;

namespace SMT.Services.Feeder
{
    // Orders slot numbers the way they sit on the machine: 1, 2, 10 (not 1, 10, 2); non-numeric
    // numbers such as "12L" sort after the numeric ones.
    public class FeederSlotNumberComparer : IComparer<string>
    {
        public static readonly FeederSlotNumberComparer Instance = new FeederSlotNumberComparer();

        public int Compare(string x, string y)
        {
            var xIsNumber = int.TryParse(x, out var xNumber);
            var yIsNumber = int.TryParse(y, out var yNumber);

            if (xIsNumber && yIsNumber)
                return xNumber.CompareTo(yNumber);

            if (xIsNumber != yIsNumber)
                return xIsNumber ? -1 : 1;

            return string.Compare(x, y, StringComparison.OrdinalIgnoreCase);
        }
    }
}

using System;
using System.Globalization;

namespace Core.UI
{
    public static class NumberFormatter
    {
        public static string GetFormattedFloat(float amount, int precision)
        {
            if (float.IsNaN(amount) || amount < 0) amount = 0;

            if (precision < 0)
                return amount.ToString(CultureInfo.InvariantCulture);

            string[] suffixes = { "", "K", "M", "B", "T", "Q" };
            var suffixIndex = 0;
            double shortAmount = amount;

            while (shortAmount >= 1000f && suffixIndex < suffixes.Length - 1)
            {
                shortAmount /= 1000f;
                suffixIndex++;
            }

            var rounded = Math.Round(shortAmount, precision);

            if (rounded % 1 == 0)
                return ((int)rounded) + suffixes[suffixIndex];

            return rounded.ToString("0." + new string('#', precision)) + suffixes[suffixIndex];
        }
    }
}
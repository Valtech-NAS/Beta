namespace SFA.Apprenticeships.Application.Common.Helpers
{
    using System;

    /// <summary>
    /// String/Formatting Extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// The currency format
        /// </summary>
        public const string CurrencyFormat = @"&pound;{0:#,###,##0.00}";

        /// <summary>
        /// Converts string to byte array.
        /// </summary>
        /// <param name="toConvert">string to convert.</param>
        /// <returns>
        /// Byte array containing converted string.
        /// </returns>
        public static byte[] ToByteArray(this string toConvert)
        {
            var data = new byte[toConvert.Length * sizeof(char)];
            Buffer.BlockCopy(toConvert.ToCharArray(), 0, data, 0, data.Length);
            return data;
        }

        /// <summary>
        /// Gets the formatted currency from a decimal eg £9,999.99.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>a formatted string</returns>
        public static string GetFormattedCurrency(this decimal value)
        {
            if (value > 0m)
            {
                return string.Format(CurrencyFormat, value);
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the formatted currency from a double eg £9,999.99.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>a formatted string eg £9,999.99</returns>
        public static string GetFormattedCurrency(this double value)
        {
            if (value > 0d)
            {
                return string.Format(CurrencyFormat, value);
            }

            return string.Empty;
        }
    }
}

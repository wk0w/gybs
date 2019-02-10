using System.Text.RegularExpressions;

namespace Gybs.Extensions
{
    /// <summary>
    /// <see cref="string"/> extensions.
    /// </summary>
    public static class StringExtensions
    {
        private static readonly Regex WhitespaceRegex = new Regex(@"\s+", RegexOptions.Compiled);

        /// <summary>
        /// Compacts the string by trimming and replacing whitespace chains with a single space.
        /// </summary>
        /// <param name="str">The string to compact.</param>
        /// <returns>The compact string.</returns>
        public static string CompactWhitespaces(this string str)
        {
            return WhitespaceRegex.Replace(str.Trim(), " ");
        }

        /// <summary>
        /// Gets whether string is not null, not empty and not a whitespace.
        /// </summary>
        /// <param name="str">String to check.</param>
        /// <returns>True if string is present, false otherwise.</returns>
        public static bool IsPresent(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }
    }
}
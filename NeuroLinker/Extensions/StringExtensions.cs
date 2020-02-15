using System.Runtime.CompilerServices;
using System.Web;

[assembly: InternalsVisibleTo("NeuroLinker.Tests")]

namespace NeuroLinker.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="string"/>
    /// </summary>
    public static class StringExtensions
    {
        #region Private Methods

        /// <summary>
        /// HtmlDecode a string
        /// </summary>
        /// <param name="self">The string to decode</param>
        /// <returns>Decoded html string</returns>
        internal static string HtmlDecode(this string self)
        {
            var decodedText = HttpUtility.HtmlDecode(self);
            return decodedText?.Replace("&#039;", "'")
                ?? string.Empty;
        }

        #endregion
    }
}
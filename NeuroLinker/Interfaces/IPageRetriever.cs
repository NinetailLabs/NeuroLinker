using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace NeuroLinker.Interfaces
{
    /// <summary>
    /// Retrieves a web page from a URL and load it into an <see cref="HtmlDocument"/>
    /// </summary>
    public interface IPageRetriever
    {
        /// <summary>
        /// Retrieve web page from the specified URL without authentication.
        /// Assume the page is encoded as UTF-8
        /// </summary>
        /// <param name="url">URL from which page should be retrieved</param>
        /// <returns>Retrieved page</returns>
        Task<HtmlDocument> RetrieveHtmlPageAsync(string url);

        /// <summary>
        /// Retrieve web page from the specified URL with basic authentication.
        /// Assume the page is encoded as UTF-8
        /// </summary>
        /// <param name="url">URL from which the page should be retrieved</param>
        /// <param name="username">Username for authentication</param>
        /// <param name="password">Password for authentication</param>
        /// <returns>Retrieved page</returns>
        Task<HtmlDocument> RetrieveHtmlPageAsync(string url, string username, string password);

        /// <summary>
        /// Retrieve web page from spesified URL.
        /// If authentication is not required leave the username and password empty
        /// </summary>
        /// <param name="url">URL from which the page should be retrieved</param>
        /// <param name="username">Username for authentication</param>
        /// <param name="password">Password for authentication</param>
        /// <param name="pageEncoding">Encoding format of the page</param>
        /// <returns>Retrieved page</returns>
        Task<HtmlDocument> RetrieveHtmlPageAsync(string url, string username, string password,
            Encoding pageEncoding);

        /// <summary>
        /// Retrieve a web document as a string
        /// </summary>
        /// <param name="url">Url from which data should be retrieved</param>
        /// <returns>Document at the Url as a string</returns>
        Task<string> RetrieveDocumentAsStringAsync(string url);

        /// <summary>
        /// Retrieve a web document as a string
        /// </summary>
        /// <param name="url">Url from which data should be retrieved</param>
        /// <param name="username">Username for authentication</param>
        /// <param name="password">Password for authentication</param>
        /// <returns>Document at the Url as a string</returns>
        Task<string> RetrieveDocumentAsStringAsync(string url, string username, string password);
    }
}
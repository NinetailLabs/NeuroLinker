using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NeuroLinker.Interfaces;
using VaraniumSharp.Attributes;

namespace NeuroLinker.Helpers
{
    /// <summary>
    /// Retrieves a web page from a URL and load it into an <see cref="HtmlDocument"/>
    /// </summary>
    [AutomaticContainerRegistration(typeof(IPageRetriever))]
    public class PageRetriever : IPageRetriever
    {
        /// <summary>
        /// DI Constructor
        /// </summary>
        /// <param name="httpClientFactory"></param>
        public PageRetriever(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Retrieve web page from the specified URL without authentication.
        /// Assume the page is encoded as UTF-8
        /// </summary>
        /// <param name="url">URL from which page should be retrieved</param>
        /// <returns>Retrieved page</returns>
        public async Task<HtmlDocument> RetrieveHtmlPageAsync(string url)
        {
            return await RetrieveHtmlPageAsync(url, string.Empty, string.Empty);
        }

        /// <summary>
        /// Retrieve web page from the specified URL with basic authentication.
        /// Assume the page is encoded as UTF-8
        /// </summary>
        /// <param name="url">URL from which the page should be retrieved</param>
        /// <param name="username">Username for authentication</param>
        /// <param name="password">Password for authentication</param>
        /// <returns>Retrieved page</returns>
        public async Task<HtmlDocument> RetrieveHtmlPageAsync(string url, string username, string password)
        {
            return await RetrieveHtmlPageAsync(url, username, password, Encoding.UTF8);
        }

        /// <summary>
        /// Retrieve web page from spesified URL.
        /// If authentication is not required leave the username and password empty
        /// </summary>
        /// <param name="url">URL from which the page should be retrieved</param>
        /// <param name="username">Username for authentication</param>
        /// <param name="password">Password for authentication</param>
        /// <param name="pageEncoding">Encoding format of the page</param>
        /// <returns>Retrieved page</returns>
        public async Task<HtmlDocument> RetrieveHtmlPageAsync(string url, string username, string password,
            Encoding pageEncoding)
        {
            var client = _httpClientFactory.GetHttpClient(username, password);

            var data = await client.GetStreamAsync(url);

            var document = new HtmlDocument();
            document.Load(data, pageEncoding);

            return document;
        }

        /// <summary>
        /// Retrieve a web document as a string
        /// </summary>
        /// <param name="url">Url from which data should be retrieved</param>
        /// <returns>Document at the Url as a string</returns>
        public async Task<string> RetrieveDocumentAsStringAsync(string url)
        {
            return await RetrieveDocumentAsStringAsync(url, null, null);
        }

        /// <summary>
        /// Retrieve a web document as a string
        /// </summary>
        /// <param name="url">Url from which data should be retrieved</param>
        /// <param name="username">Username for authentication</param>
        /// <param name="password">Password for authentication</param>
        /// <returns>Document at the Url as a string</returns>
        public async Task<string> RetrieveDocumentAsStringAsync(string url, string username, string password)
        {
            var client = _httpClientFactory.GetHttpClient(username, password);

            var data = await client.GetStringAsync(url);
            return data;
        }

        private readonly IHttpClientFactory _httpClientFactory;
    }
}
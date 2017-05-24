using System.Net.Http;

namespace NeuroLinker.Interfaces
{
    /// <summary>
    /// Creates instance of HttpClient
    /// </summary>
    public interface IHttpClientFactory
    {
        #region Public Methods

        /// <summary>
        /// Get an instance of the HttpClient
        /// </summary>
        /// <param name="username">Username for authentication</param>
        /// <param name="password">Password for authentication</param>
        /// <returns>HttpClient instance</returns>
        HttpClient GetHttpClient(string username, string password);

        #endregion
    }
}
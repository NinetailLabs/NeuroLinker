using System.Net.Http;
using NeuroLinker.Interfaces;
using VaraniumSharp.Attributes;
using VaraniumSharp.Extensions;

namespace NeuroLinker.Factories
{
    /// <summary>
    /// Creates instance of HttpClient
    /// </summary>
    [AutomaticContainerRegistration(typeof(IHttpClientFactory))]
    public class HttpClientFactory : IHttpClientFactory
    {
        #region Public Methods

        /// <summary>
        /// Get an instance of the HttpClient
        /// </summary>
        /// <param name="username">Username for authentication</param>
        /// <param name="password">Password for authentication</param>
        /// <returns>HttpClient instance</returns>
        public HttpClient GetHttpClient(string username, string password)
        {
            var requiresAuth = !string.IsNullOrEmpty(username)
                               && !string.IsNullOrEmpty(password);

            //var client = requiresAuth
            //    ? new HttpClient(new HttpClientHandler
            //    {
            //        Credentials = new NetworkCredential(username, password)
            //    })
            //    : new HttpClient();

            var client = new HttpClient();
            if (requiresAuth)
            {
                client.SetBasicAuthHeader(username, password);
            }

            return client;
        }

        #endregion
    }
}
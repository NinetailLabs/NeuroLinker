using System.Net.Http;
using NeuroLinker.Interfaces;
using NeuroLinker.Interfaces.Configuration;
using NeuroLinker.Interfaces.Factories;
using VaraniumSharp.Attributes;
using VaraniumSharp.Enumerations;
using VaraniumSharp.Extensions;

namespace NeuroLinker.Factories
{
    /// <summary>
    /// Creates instance of HttpClient
    /// </summary>
    [AutomaticContainerRegistration(typeof(IHttpClientFactory), ServiceReuse.Default, true)]
    public class HttpClientFactory : IHttpClientFactory
    {
        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public HttpClientFactory()
        {
            _configuration = null;
        }

        /// <summary>
        /// DI Constructor
        /// </summary>
        /// <param name="configuration">Http Client Configuration instance</param>
        public HttpClientFactory(IHttpClientConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

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

            var client = new HttpClient();

            if (!string.IsNullOrEmpty(_configuration?.UserAgent))
            {
                client.DefaultRequestHeaders.Add(UserAgent, _configuration.UserAgent);
            }

            if (requiresAuth)
            {
                client.SetBasicAuthHeader(username, password);
            }

            return client;
        }

        #endregion

        #region Variables

        /// <summary>
        /// User-Agent header key
        /// </summary>
        private const string UserAgent = "User-Agent";

        /// <summary>
        /// Configuration instance
        /// </summary>
        private readonly IHttpClientConfiguration _configuration;

        #endregion
    }
}
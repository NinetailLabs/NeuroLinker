using NeuroLinker.Enumerations;
using NeuroLinker.Interfaces.Configuration;
using System.Net.Http;
using VaraniumSharp.Attributes;
using VaraniumSharp.Enumerations;
using VaraniumSharp.Extensions;

namespace NeuroLinker.Configuration
{
    /// <summary>
    /// Configuration for <see cref="HttpClient"/>
    /// </summary>
    [AutomaticContainerRegistration(typeof(IHttpClientConfiguration), ServiceReuse.Singleton)]
    public sealed class HttpClientConfiguration : IHttpClientConfiguration
    {
        #region Properties

        /// <summary>
        /// The UserAgent string that should be used by the HttpClient when contacting the MAL API
        /// </summary>
        public string UserAgent => ConfigurationKeys.UserAgent.GetConfigurationValue<string>();

        #endregion
    }
}
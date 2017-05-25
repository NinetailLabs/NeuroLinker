namespace NeuroLinker.Interfaces.Configuration
{
    /// <summary>
    /// Configuration for HttpClient
    /// </summary>
    public interface IHttpClientConfiguration
    {
        #region Properties

        /// <summary>
        /// The UserAgent string that should be used by the HttpClient when contacting the MAL API
        /// </summary>
        string UserAgent { get; }

        #endregion
    }
}
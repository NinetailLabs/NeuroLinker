namespace NeuroLinker.Interfaces.Models
{
    /// <summary>
    /// Identify class as MAL response data
    /// </summary>
    public interface IResponseData
    {
        #region Properties

        /// <summary>
        /// Indicates if an error occured during the data retrieval of the entry
        /// </summary>
        bool ErrorOccured { get; set; }

        #endregion
    }
}
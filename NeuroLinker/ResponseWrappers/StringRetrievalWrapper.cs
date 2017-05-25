using System;
using System.Net;

namespace NeuroLinker.ResponseWrappers
{
    /// <summary>
    /// Response provided by HttpClient when retrieving page content as a string
    /// </summary>
    public class StringRetrievalWrapper : ResponseWrapperBase
    {
        #region Constructor

        /// <summary>
        /// Construct with data retrieved as well as the Http status information
        /// </summary>
        /// <param name="responseStatusCode">Response code received while retrieving the data</param>
        /// <param name="success">Was the retrieval a success</param>
        /// <param name="retrievedBody">Page body</param>
        public StringRetrievalWrapper(HttpStatusCode responseStatusCode, bool success, string retrievedBody)
            : base(responseStatusCode, success)
        {
            RetrievedBody = retrievedBody;
        }

        /// <summary>
        /// Construct with exception
        /// </summary>
        /// <param name="exception">Exception that occured during data retrieval</param>
        public StringRetrievalWrapper(Exception exception)
            : base(exception)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Body that was retrieved from the endpoint
        /// </summary>
        public string RetrievedBody { get; }

        #endregion
    }
}
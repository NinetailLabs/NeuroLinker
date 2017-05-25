using System;
using System.Net;
using NeuroLinker.Interfaces.Models;

namespace NeuroLinker.ResponseWrappers
{
    /// <summary>
    /// Wraps Mal response data with status information
    /// </summary>
    public class RetrievalWrapper<T> : ResponseWrapperBase where T : IResponseData
    {
        #region Constructor

        /// <summary>
        /// Construct with response code, success status and the response data
        /// </summary>
        /// <param name="responseStatusCode">Http status code</param>
        /// <param name="success">Was the retrieval a success</param>
        /// <param name="response">Character information</param>
        public RetrievalWrapper(HttpStatusCode responseStatusCode, bool success, T response)
            : base(responseStatusCode, success)
        {
            ResponseData = response;
        }

        /// <summary>
        /// Construct with an exception and the (broken) response data
        /// </summary>
        /// <param name="exception">Exception information</param>
        /// <param name="response">Broken response information</param>
        public RetrievalWrapper(Exception exception, T response)
            : base(exception)
        {
            ResponseData = response;
        }

        #endregion

        #region Properties

        /// <summary>
        /// MAL response data
        /// </summary>
        public T ResponseData { get; }

        #endregion
    }
}
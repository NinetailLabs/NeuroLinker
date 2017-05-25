using System;
using System.Net;

namespace NeuroLinker.ResponseWrappers
{
    /// <summary>
    /// Contains result of a data push
    /// </summary>
    public class DataPushResponseWrapper : ResponseWrapperBase
    {
        #region Constructor

        /// <summary>
        /// Construct with a Status code and indicate if the push was a success
        /// </summary>
        /// <param name="responseStatusCode">HttpStatus code received from the remote server</param>
        /// <param name="success">Was the request successful or not</param>
        public DataPushResponseWrapper(HttpStatusCode responseStatusCode, bool success)
            : base(responseStatusCode, success)
        {
        }

        /// <summary>
        /// Construct with an exception
        /// </summary>
        /// <param name="exception">Exception that occured during retrieval</param>
        public DataPushResponseWrapper(Exception exception)
            : base(exception)
        {
        }

        #endregion
    }
}
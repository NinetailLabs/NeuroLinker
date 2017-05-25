using System;
using System.Net;
using HtmlAgilityPack;

namespace NeuroLinker.ResponseWrappers
{
    /// <summary>
    /// Wraps retrieval of <see cref="HtmlDocument"/>
    /// </summary>
    public class HtmlDocumentRetrievalWrapper : ResponseWrapperBase
    {
        #region Constructor

        /// <summary>
        /// Construct with response code, status and the Html document
        /// </summary>
        /// <param name="responseStatusCode">The status code received by the client</param>
        /// <param name="success">Was the retrieval successful</param>
        /// <param name="document">The received document</param>
        public HtmlDocumentRetrievalWrapper(HttpStatusCode responseStatusCode, bool success, HtmlDocument document)
            : base(responseStatusCode, success)
        {
            Document = document;
        }

        /// <summary>
        /// Construct with an exception
        /// </summary>
        /// <param name="exception">Exception that occured</param>
        public HtmlDocumentRetrievalWrapper(Exception exception)
            : base(exception)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// The retrieved document
        /// </summary>
        public HtmlDocument Document { get; }

        #endregion
    }
}
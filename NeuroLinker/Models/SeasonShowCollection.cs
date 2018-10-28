using System.Collections.Generic;
using NeuroLinker.Interfaces.Models;

namespace NeuroLinker.Models
{
    /// <summary>
    /// Wrapper for collection of season entries
    /// </summary>
    public class SeasonShowCollection : IResponseData
    {
        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SeasonShowCollection()
        {
            ErrorOccured = false,
            SeasonShows = new List<SeasonData>();
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public bool ErrorOccured { get; set; }

        /// <summary>
        /// Collection of shows for season
        /// </summary>
        public List<SeasonData> SeasonShows { get; }

        #endregion
    }
}
using System.Collections.Generic;

namespace NeuroLinker.Models
{
    /// <summary>
    /// User list information
    /// </summary>
    public class UserList
    {
        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public UserList()
        {
            Anime = new List<UserListAnime>();
            ErrorOccured = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// List of anime that is on the user`s list
        /// </summary>
        public List<UserListAnime> Anime { get; set; }

        /// <summary>
        /// Error message containing details about the error that occured during the retrieval of the user`s list information.
        /// Will be empty if no error occured
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Indicate if an error occured during the user list retrieval
        /// </summary>
        public bool ErrorOccured { get; set; }

        /// <summary>
        /// Additional information about the user`s list
        /// </summary>
        public UserListInformation Info { get; set; }

        #endregion
    }
}
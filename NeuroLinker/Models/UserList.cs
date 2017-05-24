using System.Collections.Generic;

namespace NeuroLinker.Models
{
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

        public List<UserListAnime> Anime { get; set; }
        public string ErrorMessage { get; set; }
        public bool ErrorOccured { get; set; }

        public UserListInformation Info { get; set; }

        #endregion
    }
}
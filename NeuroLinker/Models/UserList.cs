using System.Collections.Generic;

namespace NeuroLinker.Models
{
    public class UserList
    {
        #region Properties

        public UserListInformation Info { get; set; }
        public List<UserListAnime> Anime { get; set; }
        public bool ErrorOccured { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

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
    }
}
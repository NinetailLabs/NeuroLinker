using System.Threading.Tasks;
using NeuroLinker.Models;

namespace NeuroLinker.Interfaces.Workers
{
    /// <summary>
    /// Retrieve user list
    /// </summary>
    public interface IListRetrievalWorker
    {
        #region Public Methods

        /// <summary>
        /// Retrieve a user's MAL list
        /// </summary>
        /// <param name="username">User's username</param>
        /// <returns>Mal list</returns>
        Task<UserList> RetrieveUserListAsync(string username);

        #endregion
    }
}
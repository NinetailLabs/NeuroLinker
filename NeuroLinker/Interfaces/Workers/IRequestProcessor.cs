using System.Threading.Tasks;
using NeuroLinker.Models;
using NeuroLinker.ResponseWrappers;

namespace NeuroLinker.Interfaces.Workers
{
    /// <summary>
    /// Manage data requests
    /// </summary>
    public interface IRequestProcessor
    {
        #region Public Methods

        /// <summary>
        /// Retrieve a Character from MAL
        /// </summary>
        /// <param name="characterId">Character Id</param>
        /// <returns>Populated Character</returns>
        Task<RetrievalWrapper<Character>> DoCharacterRetrieval(int characterId);

        /// <summary>
        /// Retrieve a Seiyuu from MAL
        /// </summary>
        /// <param name="seiyuuId"></param>
        /// <returns></returns>
        Task<RetrievalWrapper<Seiyuu>> DoSeiyuuRetrieval(int seiyuuId);

        /// <summary>
        /// Retrieve an anime from MAL
        /// </summary>
        /// <param name="id">MAL Id</param>
        /// <returns>Anime instance</returns>
        Task<RetrievalWrapper<Anime>> GetAnime(int id);

        /// <summary>
        /// Retrieve an anime from MAL
        /// </summary>
        /// <param name="id">MAL Id</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>Anime instance</returns>
        Task<RetrievalWrapper<Anime>> GetAnime(int id, string username, string password);

        /// <summary>
        /// Verify user credentials
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>True - Credentials are valid, otherwise false</returns>
        Task<DataPushResponseWrapper> VerifyCredentials(string username, string password);

        #endregion
    }
}
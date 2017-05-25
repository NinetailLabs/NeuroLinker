using System.Threading.Tasks;
using NeuroLinker.Models;
using NeuroLinker.ResponseWrappers;

namespace NeuroLinker.Interfaces.Workers
{
    /// <summary>
    /// Push user updates to MAL
    /// </summary>
    public interface IDataPushWorker
    {
        /// <summary>
        /// Push user details to MAL.
        /// This method automatically figures out if the anime should be added to the user's list or if it should simply be updated
        /// </summary>
        /// <param name="details">Update details</param>
        /// <param name="username">Username for authentication</param>
        /// <param name="password">Password for authentication</param>
        /// <returns>True - Update succeeded, otherwise false</returns>
        Task<DataPushResponseWrapper> PushAnimeDetailsToMal(AnimeUpdate details, string username, string password);
    }
}
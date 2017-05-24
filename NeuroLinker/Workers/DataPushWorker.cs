using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NeuroLinker.Helpers;
using NeuroLinker.Interfaces;
using NeuroLinker.Models;

namespace NeuroLinker.Workers
{
    /// <summary>
    /// Push user updates to MAL
    /// </summary>
    public class DataPushWorker
    {
        #region Constructor

        /// <summary>
        /// DI Constructor
        /// </summary>
        /// <param name="httpClientFactory">HttpClientFactory instance</param>
        /// <param name="listRetrievalWorker">List retrieval worker instance</param>
        /// <param name="xmlHelper">XML Helper instance</param>
        public DataPushWorker(IHttpClientFactory httpClientFactory, IListRetrievalWorker listRetrievalWorker,
            IXmlHelper xmlHelper)
        {
            _httpClientFactory = httpClientFactory;
            _listRetrievalWorker = listRetrievalWorker;
            _xmlHelper = xmlHelper;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Push user details to MAL.
        /// This method automatically figures out if the anime should be added to the user's list or if it should simply be updated
        /// </summary>
        /// <param name="details">Update details</param>
        /// <param name="username">Username for authentication</param>
        /// <param name="password">Password for authentication</param>
        /// <returns>True - Update succeeded, otherwise false</returns>
        // TODO - This should rather return the HTTP status code and reason phrase otherwise the client has no idea if/what went wrong
        public async Task<bool> PushAnimeDetailsToMal(AnimeUpdate details, string username, string password)
        {
            var userlist = await _listRetrievalWorker.RetrieveUserListAsync(username);
            var item = userlist.Anime.FirstOrDefault(t => t.SeriesId == details.AnimeId);

            return item == null
                ? await UpdateAnimeDetails(details, username, password)
                : await UpdateAnimeDetails(details, username, password, true);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Push update/add details to MAL
        /// </summary>
        /// <param name="details">Update details</param>
        /// <param name="username">Username for authentication</param>
        /// <param name="password">Password for authentication</param>
        /// <param name="isupdate">Indicate if this is an update or an add</param>
        /// <returns>True - Update succeeded, otherwise false</returns>
        private async Task<bool> UpdateAnimeDetails(AnimeUpdate details, string username, string password,
            bool isupdate = false)
        {
            try
            {
                var url = isupdate
                    ? MalRouteBuilder.UpdateAnime(details.AnimeId)
                    : MalRouteBuilder.AddAnime(details.AnimeId);
                var client = _httpClientFactory.GetHttpClient(username, password);
                var result = await client.PostAsync(url, new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("data", _xmlHelper.SerializeData(details))
                }));

                return result.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region Variables

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IListRetrievalWorker _listRetrievalWorker;
        private readonly IXmlHelper _xmlHelper;

        #endregion
    }
}
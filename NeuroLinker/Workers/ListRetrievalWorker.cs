using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using NeuroLinker.Helpers;
using NeuroLinker.Interfaces;
using NeuroLinker.Interfaces.Helpers;
using NeuroLinker.Interfaces.Workers;
using NeuroLinker.Models;
using VaraniumSharp.Attributes;

namespace NeuroLinker.Workers
{
    /// <summary>
    /// Retrieve a user list from MAL
    /// </summary>
    [AutomaticContainerRegistration(typeof(IListRetrievalWorker))]
    public class ListRetrievalWorker : IListRetrievalWorker
    {
        #region Constructor

        /// <summary>
        /// DI Constructor
        /// </summary>
        /// <param name="pageRetriever">PageRetriever instance</param>
        public ListRetrievalWorker(IPageRetriever pageRetriever)
        {
            _pageRetriever = pageRetriever;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieve a user's MAL list
        /// </summary>
        /// <param name="username">User's username</param>
        /// <returns>Mal list</returns>
        public async Task<UserList> RetrieveUserListAsync(string username)
        {
            var userList = new UserList();
            var xmlString = await _pageRetriever.RetrieveDocumentAsStringAsync(MalRouteBuilder.UserListUrl(username));
            try
            {
                var xml = XDocument.Parse(xmlString);
                var userInfo = xml.Root?.Element("myinfo");
                var userAnime = xml.Root?.Elements("anime").ToList();

                if (userInfo == null || userAnime.Count == 0)
                {
                    throw new Exception("Failed to retrieve or parse User's My Anime List");
                }

                var xmlInfoSerializer = new XmlSerializer(typeof(UserListInformation));
                var info = (UserListInformation)xmlInfoSerializer.Deserialize(userInfo.CreateReader());
                userList.Info = info;

                var xmlAnimeSerializer = new XmlSerializer(typeof(UserListAnime));
                foreach (var item in userAnime)
                {
                    var anime = (UserListAnime)xmlAnimeSerializer.Deserialize(item.CreateReader());
                    userList.Anime.Add(anime);
                }
            }
            catch (Exception exception)
            {
                userList.ErrorOccured = true;
                userList.ErrorMessage = exception.Message;
            }

            return userList;
        }

        #endregion

        #region Variables

        private readonly IPageRetriever _pageRetriever;

        #endregion
    }
}
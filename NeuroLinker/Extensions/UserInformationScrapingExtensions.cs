using System.Linq;
using HtmlAgilityPack;
using NeuroLinker.Models;

namespace NeuroLinker.Extensions
{
    /// <summary>
    /// Scrape user information from an Anime page
    /// </summary>
    public static class UserInformationScrapingExtensions
    {
        #region Public Methods

        /// <summary>
        /// Retrieve the user's current watched episode
        /// </summary>
        /// <param name="anime">Anime instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Anime instance</returns>
        public static Anime RetrieveUserEpisode(this Anime anime, HtmlDocument doc)
        {
            var episodeNode = doc.DocumentNode
                .SelectSingleNode("//input[@type='text' and @name='myinfo_watchedeps']")
                .Attributes["value"]
                .Value;
            if (int.TryParse(episodeNode, out var episodeWatched))
            {
                anime.UserWatchedEpisodes = episodeWatched;
            }

            return anime;
        }

        /// <summary>
        /// Retrieves the user's Score
        /// </summary>
        /// <param name="anime">Anime instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Anime instance</returns>
        public static Anime RetrieveUserScore(this Anime anime, HtmlDocument doc)
        {
            var scoreNode = doc.DocumentNode
                .SelectSingleNode("//select[@name='myinfo_score']")
                .ChildNodes
                .FirstOrDefault(x => x.GetAttributeValue("selected", "") == "selected")
                ?.Attributes["value"]
                .Value;

            if (int.TryParse(scoreNode, out var userScore))
            {
                anime.UserScore = userScore;
            }

            return anime;
        }

        /// <summary>
        /// Retrieve the user's Watch status
        /// </summary>
        /// <param name="anime">Anime instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Anime instance</returns>
        public static Anime RetrieveUserStatus(this Anime anime, HtmlDocument doc)
        {
            var statusNode = doc.DocumentNode
                .SelectSingleNode("//select[@name='myinfo_status']")
                .ChildNodes
                .FirstOrDefault(x => x.GetAttributeValue("selected", "") == "selected");

            anime.UserWatchedStatus = statusNode?.NextSibling.InnerText;

            return anime;
        }

        #endregion
    }
}
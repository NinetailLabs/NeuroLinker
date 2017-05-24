using System.Globalization;
using NeuroLinker.Enumerations;

namespace NeuroLinker.Helpers
{
    /// <summary>
    /// Assist with creation of MAL urls
    /// </summary>
    public static class MalRouteBuilder
    {
        #region Public Methods

        /// <summary>
        /// Url for adding an anime to a user's list
        /// </summary>
        /// <param name="id">Id of anime to add</param>
        /// <returns>Anime add url</returns>
        public static string AddAnime(int id) => $"{Parts.Root}/{Parts.Api}/{Parts.AnimeList}/{Parts.Add}/{id}.xml";

        /// <summary>
        /// Adjust the Root url user by the Route Builder.
        /// The default root value is `https://myanimelist.net`
        /// </summary>
        /// <param name="newRoot">New value to set</param>
        public static void AdjustRoot(string newRoot)
        {
            Parts.Root = newRoot;
        }

        /// <summary>
        /// Get url for retrieving an Anime's cast
        /// </summary>
        /// <param name="id">MAL Id</param>
        /// <returns>Character url</returns>
        public static string AnimeCharacterUrl(int id) => $"{Parts.Root}/{Parts.Anime}/{id}/{Parts.Characters}";

        /// <summary>
        /// Get the url for retrieve an anime
        /// </summary>
        /// <param name="id">MAL Id</param>
        /// <returns>Anime Url</returns>
        public static string AnimeUrl(int id) => $"{Parts.Root}/{Parts.Anime}/{id}";

        /// <summary>
        /// Append a route onto the MAL root
        /// </summary>
        /// <param name="route">Route to append</param>
        /// <returns>Full MAL url for the route</returns>
        public static string MalCleanUrl(string route)
        {
            return route.StartsWith("/")
                ? $"{Parts.Root}{route}"
                : $"{Parts.Root}/{route}";
        }

        /// <summary>
        /// Url for retrieving season information
        /// </summary>
        /// <param name="year">Year for which data should be retrieved</param>
        /// <param name="season">Season for which data should be retrieved</param>
        /// <returns>Season lookup url</returns>
        public static string SeasonUrl(int year, Seasons season)
            =>
                $"{Parts.Root}/{Parts.Anime}/{Parts.Season}/{year}/{season.ToString().ToLower(CultureInfo.InvariantCulture)}";

        /// <summary>
        /// Url for retrieving Seiyuu
        /// </summary>
        /// <param name="id">Seiyuu Id</param>
        /// <returns>Seiyuu page url</returns>
        public static string SeiyuuUrl(int id) => $"{Parts.Root}/{Parts.People}/{id}";

        /// <summary>
        /// Url for updating a user's anime
        /// </summary>
        /// <param name="id">Id of anime to update</param>
        /// <returns>Anime update url</returns>
        public static string UpdateAnime(int id)
            => $"{Parts.Root}/{Parts.Api}/{Parts.AnimeList}/{Parts.Update}/{id}.xml";

        //TODO - Make more generic
        /// <summary>
        /// Url for retrieving a user's list
        /// </summary>
        /// <param name="username">Username for which list should be retrieved</param>
        /// <returns>User list url</returns>
        public static string UserListUrl(string username)
            => $"{Parts.Root}/{Parts.AppInfo}?u={username}&status=all&type=anime";

        /// <summary>
        /// Url for verifying account credentials
        /// </summary>
        public static string VerifyCredentialsUrl()
            => $"{Parts.Root}/{Parts.Api}/{Parts.Account}/{Parts.VerifyCredentialsPage}";

        #endregion

        /// <summary>
        /// Contains parts that can be used to construct Urls
        /// </summary>
        internal static class Parts
        {
            #region Properties

            /// <summary>
            /// MAL root url
            /// </summary>
            public static string Root { get; set; } = "https://myanimelist.net";

            #endregion

            #region Variables

            /// <summary>
            /// Anime route part
            /// </summary>
            public const string Anime = "anime";

            /// <summary>
            /// Characters route part
            /// </summary>
            public const string Characters = "characters";

            /// <summary>
            /// API route part
            /// </summary>
            public const string Api = "api";

            /// <summary>
            /// Account route part
            /// </summary>
            public const string Account = "account";

            /// <summary>
            /// Verify credentials route part
            /// </summary>
            public const string VerifyCredentialsPage = "verify_credentials.xml";

            /// <summary>
            /// App info route part
            /// </summary>
            public const string AppInfo = "malappinfo.php";

            /// <summary>
            /// Season route part
            /// </summary>
            public const string Season = "season";

            /// <summary>
            /// Anime list route part
            /// </summary>
            public const string AnimeList = "animelist";

            /// <summary>
            /// Add route part
            /// </summary>
            public const string Add = "add";

            /// <summary>
            /// Update route part
            /// </summary>
            public const string Update = "update";

            /// <summary>
            /// People route part
            /// </summary>
            public const string People = "people";

            #endregion
        }
    }
}
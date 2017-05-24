using System;
using System.Threading.Tasks;
using NeuroLinker.Extensions;
using NeuroLinker.Helpers;
using NeuroLinker.Interfaces;
using NeuroLinker.Models;
using VaraniumSharp.Attributes;

namespace NeuroLinker.Workers
{
    /// <summary>
    /// Wrapper class for request processing
    /// </summary>
    [AutomaticContainerRegistration(typeof(IRequestProcessor))]
    public class RequestProcessor : IRequestProcessor
    {
        /// <summary>
        /// DI Constructor
        /// </summary>
        /// <param name="pageRetriever">Page retriever instance</param>
        public RequestProcessor(IPageRetriever pageRetriever)
        {
            _pageRetriever = pageRetriever;
        }

        /// <summary>
        /// Retrieve an anime from MAL
        /// </summary>
        /// <param name="id">MAL Id</param>
        /// <returns>Anime instance</returns>
        public async Task<Anime> GetAnime(int id)
        {
            return await DoAnimeRetrieval(id, null);
        }

        /// <summary>
        /// Retrieve an anime from MAL
        /// </summary>
        /// <param name="id">MAL Id</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>Anime instance</returns>
        public async Task<Anime> GetAnime(int id, string username, string password)
        {
            return await DoAnimeRetrieval(id, new Tuple<string, string>(username, password));
        }

        /// <summary>
        /// Verify user credentials
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>True - Credentials are valid, otherwise false</returns>
        public async Task<bool> VerifyCredentials(string username, string password)
        {
            var page = await _pageRetriever.RetrieveDocumentAsStringAsync(MalRouteBuilder.VerifyCredentialsUrl(),
                username, password);
            return page.Contains(username);
        }

        /// <summary>
        /// Retrieve an anime from MAL
        /// </summary>
        /// <param name="id">MAL Id</param>
        /// <param name="loginDetails">Username and password for retrieving user information. Pass null to retrieve pulbic page</param>
        /// <returns>Anime instance</returns>
        private async Task<Anime> DoAnimeRetrieval(int id, Tuple<string, string> loginDetails)
        {
            var anime = new Anime();

            try
            {
                var animePageTask = loginDetails == null
                    ? _pageRetriever.RetrieveHtmlPageAsync(MalRouteBuilder.AnimeUrl(id))
                    : _pageRetriever.RetrieveHtmlPageAsync(MalRouteBuilder.AnimeUrl(id), loginDetails.Item1,
                        loginDetails.Item2);
                var characterTask = _pageRetriever.RetrieveHtmlPageAsync(MalRouteBuilder.AnimeCharacterUrl(id));

                var animeDoc = await animePageTask;
                var characterDoc = await characterTask;

                anime
                    .RetrieveAnimeId(animeDoc)
                    .RetrieveAnimeTitle(animeDoc)
                    .RetrieveAlternativeTitles(animeDoc)
                    .RetrieveImage(animeDoc)
                    .RetrieveType(animeDoc)
                    .RetrieveEpisodes(animeDoc)
                    .RetrieveStatus(animeDoc)
                    .RetrieveAirDates(animeDoc)
                    .RetrieveRating(animeDoc)
                    .RetrieveRank(animeDoc)
                    .RetrievePopularity(animeDoc)
                    .RetrieveScore(animeDoc)
                    .RetrieveMemberCount(animeDoc)
                    .RetrieveFavotireCount(animeDoc)
                    .RetrieveGenres(animeDoc)
                    .RetrieveInfoUrls(animeDoc)
                    .RetrieveRelatedAnime(animeDoc)
                    .PopulateCharacterAndSeiyuuInformation(characterDoc);

                if (loginDetails != null)
                {
                    anime
                        .RetrieveUserScore(animeDoc)
                        .RetrieveUserEpisode(animeDoc)
                        .RetrieveUserStatus(animeDoc);
                }
            }
            catch (Exception exception)
            {
                anime.ErrorOccured = true;
                anime.ErrorMessage = exception.Message;
            }

            // TODO - Add sanity check

            return anime;
        }

        /// <summary>
        /// Retrieve a Character from MAL
        /// </summary>
        /// <param name="characterId">Character Id</param>
        /// <returns>Populated Character</returns>
        public async Task<Character> DoCharacterRetrieval(int characterId)
        {
            var character = new Character
            {
                Id = characterId,
                Url = MalRouteBuilder.AnimeCharacterUrl(characterId)
            };

            try
            {
                var characterDoc = await _pageRetriever.RetrieveHtmlPageAsync(character.Url);

                character
                    .RetrieveCharacterName(characterDoc)
                    .RetrieveCharacterImage(characterDoc)
                    .RetrieveFavoriteCount(characterDoc)
                    .RetrieveBiography(characterDoc)
                    .RetrieveAnimeography(characterDoc)
                    .RetrieveMangaograhy(characterDoc)
                    .RetrieveSeiyuu(characterDoc);
            }
            catch (Exception exception)
            {
                character.ErrorOccured = true;
                character.ErrorMessage = exception.Message;
            }

            return character;
        }

        /// <summary>
        /// Retrieve a Seiyuu from MAL
        /// </summary>
        /// <param name="seiyuuId"></param>
        /// <returns></returns>
        public async Task<Seiyuu> DoSeiyuuRetrieval(int seiyuuId)
        {
            var seiyuu = new Seiyuu
            {
                Id = seiyuuId
            };

            try
            {
                var seiyuuDoc = await _pageRetriever.RetrieveHtmlPageAsync(MalRouteBuilder.SeiyuuUrl(seiyuuId));

                seiyuu
                    .RetrieveName(seiyuuDoc)
                    .RetrieveGivenName(seiyuuDoc)
                    .RetrieveFamilyName(seiyuuDoc)
                    .RetrieveBirthday(seiyuuDoc)
                    .RetrieveAdditionalInformation(seiyuuDoc)
                    .RetrieveWebsite(seiyuuDoc)
                    .RetrieveRoles(seiyuuDoc);
            }
            catch (Exception exception)
            {
                seiyuu.ErrorOccured = true;
                seiyuu.ErrorMessage = exception.Message;
            }

            return seiyuu;
        }

        private readonly IPageRetriever _pageRetriever;
    }
}
using NeuroLinker.Extensions;
using NeuroLinker.Helpers;
using NeuroLinker.Interfaces.Helpers;
using NeuroLinker.Interfaces.Workers;
using NeuroLinker.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NeuroLinker.ResponseWrappers;
using VaraniumSharp.Attributes;

namespace NeuroLinker.Workers
{
    /// <summary>
    /// Wrapper class for request processing
    /// </summary>
    [AutomaticContainerRegistration(typeof(IRequestProcessor))]
    public class RequestProcessor : IRequestProcessor
    {
        #region Constructor

        /// <summary>
        /// DI Constructor
        /// </summary>
        /// <param name="pageRetriever">Page retriever instance</param>
        public RequestProcessor(IPageRetriever pageRetriever)
        {
            _pageRetriever = pageRetriever;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieve a Character from MAL
        /// </summary>
        /// <param name="characterId">Character Id</param>
        /// <returns>Populated Character</returns>
        public async Task<RetrievalWrapper<Character>> DoCharacterRetrieval(int characterId)
        {
            var character = new Character
            {
                Id = characterId,
                Url = MalRouteBuilder.AnimeCharacterUrl(characterId)
            };

            try
            {
                var characterResponse = await _pageRetriever.RetrieveHtmlPageAsync(character.Url);
                if (characterResponse.ResponseStatusCode == null)
                {
                    throw characterResponse.Exception;
                }
                var characterDoc = characterResponse.Document;

                character
                    .RetrieveCharacterName(characterDoc)
                    .RetrieveCharacterImage(characterDoc)
                    .RetrieveFavoriteCount(characterDoc)
                    .RetrieveBiography(characterDoc)
                    .RetrieveAnimeography(characterDoc)
                    .RetrieveMangaograhy(characterDoc)
                    .RetrieveSeiyuu(characterDoc);
                return new RetrievalWrapper<Character>(characterResponse.ResponseStatusCode.Value,
                    characterResponse.Success,
                    character);
            }
            catch (Exception exception)
            {
                character.ErrorOccured = true;
                character.ErrorMessage = exception.Message;
                return new RetrievalWrapper<Character>(exception, character);
            }
        }

        /// <summary>
        /// Retrieve a Seiyuu from MAL
        /// </summary>
        /// <param name="seiyuuId"></param>
        /// <returns></returns>
        public async Task<RetrievalWrapper<Seiyuu>> DoSeiyuuRetrieval(int seiyuuId)
        {
            var seiyuu = new Seiyuu
            {
                Id = seiyuuId
            };

            try
            {
                var seiyuuResponse = await _pageRetriever.RetrieveHtmlPageAsync(MalRouteBuilder.SeiyuuUrl(seiyuuId));
                if (seiyuuResponse.ResponseStatusCode == null)
                {
                    throw seiyuuResponse.Exception;
                }
                var seiyuuDoc = seiyuuResponse.Document;

                seiyuu
                    .RetrieveName(seiyuuDoc)
                    .RetrieveGivenName(seiyuuDoc)
                    .RetrieveFamilyName(seiyuuDoc)
                    .RetrieveBirthday(seiyuuDoc)
                    .RetrieveAdditionalInformation(seiyuuDoc)
                    .RetrieveWebsite(seiyuuDoc)
                    .RetrieveRoles(seiyuuDoc);
                return new RetrievalWrapper<Seiyuu>(seiyuuResponse.ResponseStatusCode.Value, seiyuuResponse.Success,
                    seiyuu);
            }
            catch (Exception exception)
            {
                seiyuu.ErrorOccured = true;
                seiyuu.ErrorMessage = exception.Message;
                return new RetrievalWrapper<Seiyuu>(exception, seiyuu);
            }
        }

        /// <summary>
        /// Retrieve an anime from MAL
        /// </summary>
        /// <param name="id">MAL Id</param>
        /// <returns>Anime instance</returns>
        public async Task<RetrievalWrapper<Anime>> GetAnime(int id)
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
        public async Task<RetrievalWrapper<Anime>> GetAnime(int id, string username, string password)
        {
            return await DoAnimeRetrieval(id, new Tuple<string, string>(username, password));
        }

        /// <summary>
        /// Verify user credentials
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>True - Credentials are valid, otherwise false</returns>
        public async Task<DataPushResponseWrapper> VerifyCredentials(string username, string password)
        {
            var page = await _pageRetriever.RetrieveDocumentAsStringAsync(MalRouteBuilder.VerifyCredentialsUrl(),
                username, password);

            if (page.ResponseStatusCode == null)
            {
                return new DataPushResponseWrapper(page.Exception);
            }

            return new DataPushResponseWrapper(page.ResponseStatusCode.Value, page.Success);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Retrieve an anime from MAL
        /// </summary>
        /// <param name="id">MAL Id</param>
        /// <param name="loginDetails">Username and password for retrieving user information. Pass null to retrieve pulbic page</param>
        /// <returns>Anime instance</returns>
        private async Task<RetrievalWrapper<Anime>> DoAnimeRetrieval(int id, Tuple<string, string> loginDetails)
        {
            var anime = new Anime();

            try
            {
                var animePageTask = loginDetails == null
                    ? _pageRetriever.RetrieveHtmlPageAsync(MalRouteBuilder.AnimeUrl(id))
                    : _pageRetriever.RetrieveHtmlPageAsync(MalRouteBuilder.AnimeUrl(id), loginDetails.Item1,
                        loginDetails.Item2);
                var characterTask = _pageRetriever.RetrieveHtmlPageAsync(MalRouteBuilder.AnimeCharacterUrl(id));

                var animeResponse = await animePageTask;
                if (animeResponse.ResponseStatusCode == null)
                {
                    throw animeResponse.Exception;
                }

                if (!new HttpResponseMessage(animeResponse.ResponseStatusCode.Value)
                    .IsSuccessStatusCode)
                {
                    anime.ErrorOccured = true;
                    anime.ErrorMessage =
                        $"Status code {animeResponse.ResponseStatusCode.Value} does not indicate success";
                    return new RetrievalWrapper<Anime>(animeResponse.ResponseStatusCode.Value, false, anime);
                }

                var characterResponse = await characterTask;

                var animeDoc = animeResponse.Document;
                var characterDoc = characterResponse.Document;

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

                // TODO - Add sanity check

                return new RetrievalWrapper<Anime>(animeResponse.ResponseStatusCode.Value, animeResponse.Success,
                    anime);
            }
            catch (Exception exception)
            {
                anime.ErrorOccured = true;
                anime.ErrorMessage = exception.Message;
                return new RetrievalWrapper<Anime>(exception, anime);
            }
        }

        #endregion

        #region Variables

        private readonly IPageRetriever _pageRetriever;

        #endregion
    }
}
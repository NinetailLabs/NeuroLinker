using NeuroLinker.Enumerations;
using NeuroLinker.Extensions;
using NeuroLinker.Helpers;
using NeuroLinker.Interfaces.Helpers;
using NeuroLinker.Interfaces.Workers;
using NeuroLinker.Models;
using NeuroLinker.ResponseWrappers;
using System;
using System.Net;
using System.Threading.Tasks;
using VaraniumSharp.Attributes;

namespace NeuroLinker.Workers
{
    /// <summary>
    /// Retrieve season information
    /// </summary>
    [AutomaticContainerRegistration(typeof(ISeasonWorker))]
    public class SeasonWorker : ISeasonWorker
    {
        #region Constructor

        /// <summary>
        /// DI Constructor
        /// </summary>
        /// <param name="pageRetriever">PageRetriever instance</param>
        public SeasonWorker(IPageRetriever pageRetriever)
        {
            _pageRetriever = pageRetriever;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieve information about shows in a specific season
        /// </summary>
        /// <param name="year">Year for which season data should be retrieved</param>
        /// <param name="season">Season for which data should be retrieved</param>
        /// <returns>Collection of show for the selected season</returns>
        public async Task<RetrievalWrapper<SeasonShowCollection>> GetSeasonData(int year, Seasons season)
        {
            var collectionWrapper = new SeasonShowCollection();
            try
            {
                var doc = await _pageRetriever.RetrieveHtmlPageAsync(MalRouteBuilder.SeasonUrl(year, season));
                if (doc.ResponseStatusCode == null)
                {
                    throw doc.Exception;
                }

                var links = doc.Document.DocumentNode
                    .SelectNodes("//a[@class='link-title']");

                foreach (var link in links)
                {
                    var url = link.Attributes["href"].Value;
                    var idString = url.Split('/')[4];
                    int id;
                    int.TryParse(idString, out id);

                    var title = link.InnerHtml;

                    var tmpData = new SeasonData
                    {
                        Id = id,
                        Title = title
                    };
                    collectionWrapper.SeasonShows.Add(tmpData);
                }
                return new RetrievalWrapper<SeasonShowCollection>(doc.ResponseStatusCode.Value, doc.Success,
                    collectionWrapper);
            }
            catch (Exception exception)
            {
                return new RetrievalWrapper<SeasonShowCollection>(exception, collectionWrapper);
            }
        }

        /// <summary>
        /// Retrieve information for the current season.
        /// This information also includes data for the next two seasons
        /// </summary>
        /// <returns>Collection containing season data for the next three seasons</returns>
        public async Task<RetrievalWrapper<SeasonShowCollection>> RetrieveCurrentSeason()
        {
            var currentDate = DateTime.Now;
            var seasonWrapper = new SeasonShowCollection();
            var currentSeason = currentDate.CalculateCurrentSeason();
            var year = currentDate.Year;
            if (currentSeason == Seasons.Winter && currentDate.Month == 12)
            {
                year++;
            }

            for (var r = 0; r < 3; r++)
            {
                var seasonData = await GetSeasonData(year, currentSeason);
                seasonWrapper.SeasonShows.AddRange(seasonData.ResponseData.SeasonShows);

                //Get info for the next season
                year = currentSeason.NextSeasonYear(year);
                currentSeason = currentSeason.GetNextSeason();
            }

            return new RetrievalWrapper<SeasonShowCollection>(HttpStatusCode.OK, true, seasonWrapper);
        }

        #endregion

        #region Variables

        private readonly IPageRetriever _pageRetriever;

        #endregion
    }
}
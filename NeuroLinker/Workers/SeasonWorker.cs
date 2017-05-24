using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NeuroLinker.Enumerations;
using NeuroLinker.Extensions;
using NeuroLinker.Helpers;
using NeuroLinker.Interfaces;
using NeuroLinker.Models;
using VaraniumSharp.Attributes;

namespace NeuroLinker.Workers
{
    /// <summary>
    /// Retrieve season information
    /// </summary>
    [AutomaticContainerRegistration(typeof(ISeasonWorker))]
    public class SeasonWorker : ISeasonWorker
    {
        /// <summary>
        /// DI Constructor
        /// </summary>
        /// <param name="pageRetriever">PageRetriever instance</param>
        public SeasonWorker(IPageRetriever pageRetriever)
        {
            _pageRetriever = pageRetriever;
        }

        /// <summary>
        /// Retrieve information about shows in a specific season
        /// </summary>
        /// <param name="year">Year for which season data should be retrieved</param>
        /// <param name="season">Season for which data should be retrieved</param>
        /// <returns>Collection of show for the selected season</returns>
        public async Task<List<SeasonData>> GetSeasonData(int year, Seasons season)
        {
            var seasonList = new List<SeasonData>();
            try
            {
                var doc = await _pageRetriever.RetrieveHtmlPageAsync(MalRouteBuilder.SeasonUrl(year, season));

                var links = doc.DocumentNode
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
                    seasonList.Add(tmpData);
                }
            }
            catch (Exception)
            {
                //TODO - Log this exception
            }
            return seasonList;
        }

        /// <summary>
        /// Retrieve information for the current season.
        /// This information also includes data for the next two seasons
        /// </summary>
        /// <returns>Collection containing season data for the next three seasons</returns>
        public async Task<List<SeasonData>> RetrieveCurrentSeason()
        {
            var currentDate = DateTime.Now;
            var seasonData = new List<SeasonData>();
            var currentSeason = currentDate.CalculateCurrentSeason();
            var year = currentDate.Year;
            if (currentSeason == Seasons.Winter && currentDate.Month == 12)
            {
                //For some reason MAL classifies winter 2015 as winter 2016 so adjust for this fact
                year++;
            }

            for (var r = 0; r < 3; r++)
            {
                seasonData.AddRange(await GetSeasonData(year, currentSeason));

                //Get info for the next season
                year = currentSeason.NextSeasonYear(year);
                currentSeason = currentSeason.GetNextSeason();
            }

            return seasonData;
        }

        private readonly IPageRetriever _pageRetriever;
    }
}
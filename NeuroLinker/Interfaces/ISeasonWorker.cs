using System.Collections.Generic;
using System.Threading.Tasks;
using NeuroLinker.Enumerations;
using NeuroLinker.Models;

namespace NeuroLinker.Interfaces
{
    /// <summary>
    /// Retreive season information
    /// </summary>
    public interface ISeasonWorker
    {
        /// <summary>
        /// Retrieve information about shows in a specific season
        /// </summary>
        /// <param name="year">Year for which season data should be retrieved</param>
        /// <param name="season">Season for which data should be retrieved</param>
        /// <returns>Collection of show for the selected season</returns>
        Task<List<SeasonData>> GetSeasonData(int year, Seasons season);

        /// <summary>
        /// Retrieve information for the current season.
        /// This information also includes data for the next two seasons
        /// </summary>
        /// <returns>Collection containing season data for the next three seasons</returns>
        Task<List<SeasonData>> RetrieveCurrentSeason();
    }
}
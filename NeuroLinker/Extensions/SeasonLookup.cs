using System;
using NeuroLinker.Enumerations;

namespace NeuroLinker.Extensions
{
    /// <summary>
    /// Season information
    /// </summary>
    public static class SeasonLookup
    {
        #region Public Methods

        /// <summary>
        /// Convert a date to a season
        /// </summary>
        /// <param name="date">The Date to convert</param>
        /// <returns>The season as a SeasonEnum</returns>
        public static Seasons CalculateCurrentSeason(this DateTime date)
        {
            var currentSeason = Seasons.Winter;

            if (date.Month >= 10 && date.Month <= 12)
            {
                currentSeason = Seasons.Fall;
            }
            if (date.Month >= 4 && date.Month <= 6)
            {
                currentSeason = Seasons.Spring;
            }
            if (date.Month >= 7 && date.Month <= 9)
            {
                currentSeason = Seasons.Summer;
            }

            return currentSeason;
        }

        /// <summary>
        /// Get the season following the one provided
        /// </summary>
        /// <param name="currentSeason">One of the seasons</param>
        /// <returns>The next season</returns>
        public static Seasons GetNextSeason(this Seasons currentSeason)
        {
            switch (currentSeason)
            {
                case Seasons.Winter:
                    return Seasons.Spring;

                case Seasons.Fall:
                    return Seasons.Winter;

                case Seasons.Spring:
                    return Seasons.Summer;

                case Seasons.Summer:
                    return Seasons.Fall;

                default:
                    throw new ArgumentOutOfRangeException(nameof(currentSeason), currentSeason, "Invalid season");
            }
        }

        /// <summary>
        /// Given the current season and its year get the year for the following season
        /// </summary>
        /// <param name="currentSeason">One of the seasons</param>
        /// <param name="year">The year of the season</param>
        /// <returns>Year for the next season</returns>
        public static int NextSeasonYear(this Seasons currentSeason, int year)
        {
            switch (currentSeason)
            {
                case Seasons.Spring:
                case Seasons.Winter:
                case Seasons.Summer:
                    return year;

                case Seasons.Fall:
                    return year + 1;

                default:
                    return year;
            }
        }

        /// <summary>
        /// Get the start date of the season for a spesific date
        /// </summary>
        /// <param name="date">The date to lookup</param>
        /// <returns>The date on which the season started</returns>
        public static DateTime SeasonStart(this DateTime date)
        {
            DateTime seasonDate;

            if (date.Month >= 3 && date.Month <= 5)
            {
                seasonDate = new DateTime(date.Year, 3, 1);
            }
            else if (date.Month >= 6 && date.Month <= 8)
            {
                seasonDate = new DateTime(date.Year, 6, 1);
            }
            else if (date.Month >= 9 && date.Month <= 11)
            {
                seasonDate = new DateTime(date.Year, 9, 1);
            }
            else
            {
                seasonDate = date.Month == 12 ? new DateTime(date.Year, 12, 1) : new DateTime((date.Year - 1), 12, 1);
            }

            return seasonDate;
        }

        /// <summary>
        /// Get the end date of the season for a spesific date
        /// </summary>
        /// <param name="date">The date to lookup</param>
        /// <returns>The date on which the season ended</returns>
        public static DateTime SeasonEnd(this DateTime date)
        {
            DateTime endDate;

            if (date.Month >= 3 && date.Month <= 5)
            {
                endDate = new DateTime(date.Year, 5, DateTime.DaysInMonth(date.Year, 5));
            }
            else if (date.Month >= 6 && date.Month <= 8)
            {
                endDate = new DateTime(date.Year, 8, DateTime.DaysInMonth(date.Year, 8));
            }
            else if (date.Month >= 9 && date.Month <= 11)
            {
                endDate = new DateTime(date.Year, 11, DateTime.DaysInMonth(date.Year, 11));
            }
            else
            {
                endDate = date.Month == 12
                    ? new DateTime(date.Year + 1, 2, DateTime.DaysInMonth(date.Year + 1, 2))
                    : new DateTime(date.Year, 2, DateTime.DaysInMonth(date.Year, 2));
            }

            return endDate;
        }

        #endregion
    }
}
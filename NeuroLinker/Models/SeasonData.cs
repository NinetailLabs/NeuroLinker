namespace NeuroLinker.Models
{
    /// <summary>
    /// Anime information that is used to link to a season
    /// </summary>
    public class SeasonData
    {
        #region Properties

        /// <summary>
        /// Mal Id of the anime
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Anime`s title
        /// </summary>
        public string Title { get; set; }

        #endregion
    }
}
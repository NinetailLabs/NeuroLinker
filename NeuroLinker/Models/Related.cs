namespace NeuroLinker.Models
{
    /// <summary>
    /// Information about other anime that relate to an anime
    /// </summary>
    public class Related
    {
        #region Properties

        /// <summary>
        /// MAL Id for the related show
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title of the related show
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Mal URL for the related show
        /// </summary>
        public string Url { get; set; }

        #endregion
    }
}
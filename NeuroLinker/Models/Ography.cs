namespace NeuroLinker.Models
{
    /// <summary>
    /// Model to store information about media that a character stars in
    /// </summary>
    public class Ography
    {
        #region Properties

        /// <summary>
        /// Id of the entry (eg a Mal anime Id)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Poster URL of the content
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Name of the media
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of role the character fulfills in the media
        /// </summary>
        public string RoleType { get; set; }

        /// <summary>
        /// URL where the entry is located
        /// </summary>
        public string Url { get; set; }

        #endregion
    }
}
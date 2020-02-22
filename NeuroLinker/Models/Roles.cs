namespace NeuroLinker.Models
{
    /// <summary>
    /// Role information for a character for a specific anime
    /// </summary>
    public class Roles
    {
        #region Properties

        /// <summary>
        /// Mal Id that the role information is for
        /// </summary>
        public int AnimeId { get; set; }

        /// <summary>
        /// URL that links to the anime`s poster
        /// </summary>
        public string AnimePicUrl { get; set; }

        /// <summary>
        /// Anime`s title
        /// </summary>
        public string AnimeTitle { get; set; }

        /// <summary>
        /// Mal URL for the anime
        /// </summary>
        public string AnimeUrl { get; set; }

        /// <summary>
        /// Mal Id for the character
        /// </summary>
        public int CharacterId { get; set; }

        /// <summary>
        /// The character`s name
        /// </summary>
        public string CharacterName { get; set; }

        /// <summary>
        /// URL linking to the character`s poster image
        /// </summary>
        public string CharacterPic { get; set; }

        /// <summary>
        /// MAL URL for the character
        /// </summary>
        public string CharacterUrl { get; set; }

        /// <summary>
        /// The type of role the character has in the anime
        /// </summary>
        public string RoleType { get; set; }

        #endregion
    }
}
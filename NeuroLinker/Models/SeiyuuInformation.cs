using Newtonsoft.Json;

namespace NeuroLinker.Models
{
    /// <summary>
    /// Seiyuu information
    /// </summary>
    public class SeiyuuInformation
    {
        #region Properties

        /// <summary>
        /// Mal Id for the Seiyuu
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The seiyuu`s language
        /// </summary>
        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        /// <summary>
        /// The seiyuu`s full name
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// URL linking to a picture of the seiyuu
        /// </summary>
        [JsonProperty(PropertyName = "image_url")]
        public string PictureUrl { get; set; }

        /// <summary>
        /// Mal URL where the seiyuu`s information can be found
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        #endregion
    }
}
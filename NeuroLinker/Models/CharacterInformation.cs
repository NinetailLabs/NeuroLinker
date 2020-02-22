using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeuroLinker.Models
{
    /// <summary>
    /// Information about a character
    /// </summary>
    public class CharacterInformation
    {
        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CharacterInformation()
        {
            Seiyuu = new List<SeiyuuInformation>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The name of the character
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string CharacterName { get; set; }

        /// <summary>
        /// The url where the character`s poster image can be accessed
        /// </summary>
        [JsonProperty(PropertyName = "image_url")]
        public string CharacterPicture { get; set; }

        /// <summary>
        /// The type of role the character has in the show
        /// </summary>
        [JsonProperty(PropertyName = "character_type")]
        public string CharacterType { get; set; }

        /// <summary>
        /// Mal URL where the character can be accessed
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string CharacterUrl { get; set; }

        /// <summary>
        /// Mal Id for the character
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Information about the Seiyuu that voice the character
        /// </summary>
        [JsonProperty(PropertyName = "seiyuu")]
        public List<SeiyuuInformation> Seiyuu { get; set; }

        #endregion
    }
}
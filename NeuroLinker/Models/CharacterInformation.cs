using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeuroLinker.Models
{
    public class CharacterInformation
    {
        #region Constructor

        public CharacterInformation()
        {
            Seiyuu = new List<SeiyuuInformation>();
        }

        #endregion

        #region Properties

        [JsonProperty(PropertyName = "name")]
        public string CharacterName { get; set; }

        [JsonProperty(PropertyName = "image_url")]
        public string CharacterPicture { get; set; }

        [JsonProperty(PropertyName = "character_type")]
        public string CharacterType { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string CharacterUrl { get; set; }

        public int Id { get; set; }

        [JsonProperty(PropertyName = "seiyuu")]
        public List<SeiyuuInformation> Seiyuu { get; set; }

        #endregion
    }
}
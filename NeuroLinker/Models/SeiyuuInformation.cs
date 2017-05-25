using Newtonsoft.Json;

namespace NeuroLinker.Models
{
    public class SeiyuuInformation
    {
        #region Properties

        public int Id { get; set; }

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "image_url")]
        public string PictureUrl { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        #endregion
    }
}
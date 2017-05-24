using System.Xml.Serialization;
using Newtonsoft.Json;

namespace NeuroLinker.Models
{
    [XmlRoot(ElementName = "entry")]
    public class AnimeUpdate
    {
        #region Properties

        [JsonProperty(PropertyName = "anime_id")]
        [XmlIgnore]
        public int AnimeId { get; set; }

        [JsonProperty(PropertyName = "enable_discussion")]
        [XmlElement(ElementName = "enable_discussion")]
        public int EnableDiscussion { get; set; }

        [JsonProperty(PropertyName = "enable_rewatching")]
        [XmlElement(ElementName = "enable_rewatching")]
        public int EnableRewatching { get; set; }

        [JsonProperty(PropertyName = "comments")]
        [XmlElement(ElementName = "comments")]
        public string Comments { get; set; }

        [JsonProperty(PropertyName = "rewatched")]
        [XmlElement(ElementName = "times_rewatched")]
        public int Rewatched { get; set; }

        [JsonProperty(PropertyName = "rewatch_value")]
        [XmlElement(ElementName = "rewatch_value")]
        public int RewatchValue { get; set; }

        [JsonProperty(PropertyName = "date_start")]
        [XmlElement(ElementName = "date_start")]
        public string DateStart { get; set; }

        [JsonProperty(PropertyName = "date_finished")]
        [XmlElement(ElementName = "date_finish")]
        public string DateFinish { get; set; }

        [JsonProperty(PropertyName = "priority")]
        [XmlElement(ElementName = "priority")]
        public int Priority { get; set; }

        [JsonProperty(PropertyName = "episodes")]
        [XmlElement(ElementName = "episode")]
        public int Episodes { get; set; }

        [JsonProperty(PropertyName = "storage_type")]
        [XmlElement(ElementName = "storage_type")]
        public int StorageType { get; set; }

        [JsonProperty(PropertyName = "storage_value")]
        [XmlElement(ElementName = "storage_value")]
        public float StorageValue { get; set; }

        [JsonProperty(PropertyName = "status")]
        [XmlElement(ElementName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "score")]
        [XmlElement(ElementName = "score")]
        public int Score { get; set; }

        [JsonProperty(PropertyName = "tags")]
        [XmlElement(ElementName = "tags")]
        public string Tags { get; set; }

        #endregion
    }
}
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace NeuroLinker.Models
{
    /// <summary>
    /// Model used to update a Mal anime
    /// </summary>
    [XmlRoot(ElementName = "entry")]
    public class AnimeUpdate
    {
        #region Properties

        /// <summary>
        /// Mal Id of the anime to udpate
        /// </summary>
        [JsonProperty(PropertyName = "anime_id")]
        [XmlIgnore]
        public int AnimeId { get; set; }

        /// <summary>
        /// Indicate if discussion should be enabled or not
        /// </summary>
        [JsonProperty(PropertyName = "enable_discussion")]
        [XmlElement(ElementName = "enable_discussion")]
        public int EnableDiscussion { get; set; }

        /// <summary>
        /// Indicate if the user is rewatching the show
        /// </summary>
        [JsonProperty(PropertyName = "enable_rewatching")]
        [XmlElement(ElementName = "enable_rewatching")]
        public int EnableRewatching { get; set; }

        /// <summary>
        /// Comments the user wants to add about the show
        /// </summary>
        [JsonProperty(PropertyName = "comments")]
        [XmlElement(ElementName = "comments")]
        public string Comments { get; set; }

        /// <summary>
        /// The number of times the user has rewatched the show
        /// </summary>
        [JsonProperty(PropertyName = "rewatched")]
        [XmlElement(ElementName = "times_rewatched")]
        public int Rewatched { get; set; }

        /// <summary>
        /// The rewatch value for the show
        /// </summary>
        [JsonProperty(PropertyName = "rewatch_value")]
        [XmlElement(ElementName = "rewatch_value")]
        public int RewatchValue { get; set; }

        /// <summary>
        /// The date when the user started watching the show
        /// </summary>
        [JsonProperty(PropertyName = "date_start")]
        [XmlElement(ElementName = "date_start")]
        public string DateStart { get; set; }

        /// <summary>
        /// The date when the user finished watching the show
        /// </summary>
        [JsonProperty(PropertyName = "date_finished")]
        [XmlElement(ElementName = "date_finish")]
        public string DateFinish { get; set; }

        /// <summary>
        /// The priority the user has assigned to the show
        /// </summary>
        [JsonProperty(PropertyName = "priority")]
        [XmlElement(ElementName = "priority")]
        public int Priority { get; set; }

        /// <summary>
        /// The latest episode the user has watched
        /// </summary>
        [JsonProperty(PropertyName = "episodes")]
        [XmlElement(ElementName = "episode")]
        public int Episodes { get; set; }

        /// <summary>
        /// The type of storage that the user is storing the episodes on
        /// </summary>
        [JsonProperty(PropertyName = "storage_type")]
        [XmlElement(ElementName = "storage_type")]
        public int StorageType { get; set; }

        /// <summary>
        /// The size of the files the user stores for the show
        /// </summary>
        [JsonProperty(PropertyName = "storage_value")]
        [XmlElement(ElementName = "storage_value")]
        public float StorageValue { get; set; }

        /// <summary>
        /// The user`s status for the show
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        [XmlElement(ElementName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// The score the user has assigned to the show
        /// </summary>
        [JsonProperty(PropertyName = "score")]
        [XmlElement(ElementName = "score")]
        public int Score { get; set; }

        /// <summary>
        /// Tags the user has assigned to the show
        /// </summary>
        [JsonProperty(PropertyName = "tags")]
        [XmlElement(ElementName = "tags")]
        public string Tags { get; set; }

        #endregion
    }
}
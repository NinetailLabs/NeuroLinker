using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace NeuroLinker.Models
{
    /// <summary>
    /// Anime information entry for the user`s anime list
    /// </summary>
    [Serializable]
    [XmlRoot("anime")]
    public class UserListAnime
    {
        #region Properties

        /// <summary>
        /// Indicate if the user is rewatching the show or not
        /// </summary>
        [XmlElement(ElementName = "my_rewatching")]
        [JsonProperty(PropertyName = "my_rewatching")]
        public string MyRewatching { get; set; }

        /// <summary>
        /// The current rewatch episode
        /// </summary>
        [XmlElement(ElementName = "my_rewatching_ep")]
        [JsonProperty(PropertyName = "my_rewatching_episodes")]
        public int RewatchingEpisode { get; set; }

        /// <summary>
        /// The last time the user list entry was updated
        /// </summary>
        [XmlElement(ElementName = "my_last_updated")]
        [JsonProperty(PropertyName = "last_update")]
        public string LastUpdated { get; set; }

        /// <summary>
        /// The user`s Id
        /// </summary>
        [XmlElement(ElementName = "my_id")]
        [JsonProperty(PropertyName = "my_id")]
        public int MyId { get; set; }

        /// <summary>
        /// The number of episodes the user has watched
        /// </summary>
        [XmlElement(ElementName = "my_watched_episodes")]
        [JsonProperty(PropertyName = "watched_episodes")]
        public int WatchedEpisodes { get; set; }

        /// <summary>
        /// The date when the user started watching the show
        /// </summary>
        [XmlElement(ElementName = "my_start_date")]
        [JsonProperty(PropertyName = "my_start_date")]
        public string MyStartDate { get; set; }

        /// <summary>
        /// The date when the user finished watching the show
        /// </summary>
        [XmlElement(ElementName = "my_finish_date")]
        [JsonProperty(PropertyName = "my_finish_date")]
        public string MyFinishDate { get; set; }

        /// <summary>
        /// The date when the series finished screening
        /// </summary>
        [XmlElement(ElementName = "series_end")]
        [JsonProperty(PropertyName = "series_end")]
        public string SeriesEnd { get; set; }

        /// <summary>
        /// URL to the anime`s poster
        /// </summary>
        [XmlElement(ElementName = "series_image")]
        [JsonProperty(PropertyName = "series_image")]
        public string SeriesImage { get; set; }

        /// <summary>
        /// Score the user assigned to the anime
        /// </summary>
        [XmlElement(ElementName = "my_score")]
        [JsonProperty(PropertyName = "my_score")]
        public int MyScore { get; set; }

        /// <summary>
        /// The user`s watch status for the show
        /// </summary>
        [XmlElement(ElementName = "my_status")]
        [JsonProperty(PropertyName = "my_status")]
        public int MyStatus { get; set; }

        /// <summary>
        /// Tags the user has assigned to the show
        /// </summary>
        [XmlElement(ElementName = "my_tags")]
        [JsonProperty(PropertyName = "my_tags")]
        public string MyTags { get; set; }

        /// <summary>
        /// Number of episodes the series has
        /// </summary>
        [XmlElement(ElementName = "series_episodes")]
        [JsonProperty(PropertyName = "series_episodes")]
        public int SeriesEpisodes { get; set; }

        /// <summary>
        /// Synonyms for the series
        /// </summary>
        [XmlElement(ElementName = "series_synonyms")]
        [JsonProperty(PropertyName = "series_synonyms")]
        public string SeriesSynonyms { get; set; }

        /// <summary>
        /// The type of the series (eg TV)
        /// </summary>
        [XmlElement(ElementName = "series_type")]
        [JsonProperty(PropertyName = "series_type")]
        public int SeriesType { get; set; }

        /// <summary>
        /// The Mal Id for the show
        /// </summary>
        [XmlElement(ElementName = "series_animedb_id")]
        [JsonProperty(PropertyName = "series_id")]
        public int SeriesId { get; set; }

        /// <summary>
        /// The series screening status
        /// </summary>
        [XmlElement(ElementName = "series_status")]
        [JsonProperty(PropertyName = "series_status")]
        public int SeriesStatus { get; set; }

        /// <summary>
        /// Date when the series started screening
        /// </summary>
        [XmlElement(ElementName = "series_start")]
        [JsonProperty(PropertyName = "series_start")]
        public string SeriesStart { get; set; }

        /// <summary>
        /// The series title
        /// </summary>
        [XmlElement(ElementName = "series_title")]
        [JsonProperty(PropertyName = "series_title")]
        public string SeriesTitle { get; set; }

        #endregion
    }
}
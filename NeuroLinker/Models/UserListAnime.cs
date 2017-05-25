using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace NeuroLinker.Models
{
    [Serializable][XmlRoot("anime")]
    public class UserListAnime
    {
        #region Properties

        [XmlElement(ElementName = "my_rewatching")]
        [JsonProperty(PropertyName = "my_rewatching")]
        public string MyRewatching { get; set; }

        [XmlElement(ElementName = "my_rewatching_ep")]
        [JsonProperty(PropertyName = "my_rewatching_episodes")]
        public int RewatchingEpisode { get; set; }

        [XmlElement(ElementName = "my_last_updated")]
        [JsonProperty(PropertyName = "last_update")]
        public string LastUpdated { get; set; }

        [XmlElement(ElementName = "my_id")]
        [JsonProperty(PropertyName = "my_id")]
        public int MyId { get; set; }

        [XmlElement(ElementName = "my_watched_episodes")]
        [JsonProperty(PropertyName = "watched_episodes")]
        public int WatchedEpisodes { get; set; }

        [XmlElement(ElementName = "my_start_date")]
        [JsonProperty(PropertyName = "my_start_date")]
        public string MyStartDate { get; set; }

        [XmlElement(ElementName = "my_finish_date")]
        [JsonProperty(PropertyName = "my_finish_date")]
        public string MyFinishDate { get; set; }

        [XmlElement(ElementName = "series_end")]
        [JsonProperty(PropertyName = "series_end")]
        public string SeriesEnd { get; set; }

        [XmlElement(ElementName = "series_image")]
        [JsonProperty(PropertyName = "series_image")]
        public string SeriesImage { get; set; }

        [XmlElement(ElementName = "my_score")]
        [JsonProperty(PropertyName = "my_score")]
        public int MyScore { get; set; }

        [XmlElement(ElementName = "my_status")]
        [JsonProperty(PropertyName = "my_status")]
        public int MyStatus { get; set; }

        [XmlElement(ElementName = "my_tags")]
        [JsonProperty(PropertyName = "my_tags")]
        public string MyTags { get; set; }

        [XmlElement(ElementName = "series_episodes")]
        [JsonProperty(PropertyName = "series_episodes")]
        public int SeriesEpisodes { get; set; }

        [XmlElement(ElementName = "series_synonyms")]
        [JsonProperty(PropertyName = "series_synonyms")]
        public string SeriesSynonyms { get; set; }

        [XmlElement(ElementName = "series_type")]
        [JsonProperty(PropertyName = "series_type")]
        public int SeriesType { get; set; }

        [XmlElement(ElementName = "series_animedb_id")]
        [JsonProperty(PropertyName = "series_id")]
        public int SeriesId { get; set; }

        [XmlElement(ElementName = "series_status")]
        [JsonProperty(PropertyName = "series_status")]
        public int SeriesStatus { get; set; }

        [XmlElement(ElementName = "series_start")]
        [JsonProperty(PropertyName = "series_start")]
        public string SeriesStart { get; set; }

        [XmlElement(ElementName = "series_title")]
        [JsonProperty(PropertyName = "series_title")]
        public string SeriesTitle { get; set; }

        #endregion
    }
}
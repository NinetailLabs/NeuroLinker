using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace NeuroLinker.Models
{
    /// <summary>
    /// Additional user list information
    /// </summary>
    [Serializable]
    [XmlRoot("myinfo")]
    public class UserListInformation
    {
        #region Properties

        /// <summary>
        /// The username of the user the list belongs to
        /// </summary>
        [XmlElement(ElementName = "user_name")]
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        /// <summary>
        /// The number of shows the user is watching
        /// </summary>
        [XmlElement(ElementName = "user_watching")]
        [JsonProperty(PropertyName = "watching")]
        public int Watching { get; set; }

        /// <summary>
        /// The number of episodes the user has finished watching
        /// </summary>
        [XmlElement(ElementName = "user_completed")]
        [JsonProperty(PropertyName = "completed")]
        public int Completed { get; set; }

        /// <summary>
        /// The number of shows the user has dropped
        /// </summary>
        [XmlElement(ElementName = "user_dropped")]
        [JsonProperty(PropertyName = "dropped")]
        public int Dropped { get; set; }

        /// <summary>
        /// The number of shows the user plan to watch
        /// </summary>
        [XmlElement(ElementName = "user_plantowatch")]
        [JsonProperty(PropertyName = "plan_to_watch")]
        public int PlanToWatch { get; set; }

        /// <summary>
        /// The number of days the user has watched anime
        /// </summary>
        [XmlElement(ElementName = "user_days_spent_watching")]
        [JsonProperty(PropertyName = "days_watching")]
        public double DaysWatching { get; set; }

        /// <summary>
        /// The number of shows the user has placed on hold
        /// </summary>
        [XmlElement(ElementName = "user_onhold")]
        [JsonProperty(PropertyName = "on_hold")]
        public int OnHold { get; set; }

        /// <summary>
        /// The user`s Mal Id
        /// </summary>
        [XmlElement(ElementName = "user_id")]
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

        #endregion
    }
}
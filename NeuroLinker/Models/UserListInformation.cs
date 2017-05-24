using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace NeuroLinker.Models
{
    [Serializable][XmlRoot("myinfo")]
    public class UserListInformation
    {
        #region Properties

        [XmlElement(ElementName = "user_name")]
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [XmlElement(ElementName = "user_watching")]
        [JsonProperty(PropertyName = "watching")]
        public int Watching { get; set; }

        [XmlElement(ElementName = "user_completed")]
        [JsonProperty(PropertyName = "completed")]
        public int Completed { get; set; }

        [XmlElement(ElementName = "user_dropped")]
        [JsonProperty(PropertyName = "dropped")]
        public int Dropped { get; set; }

        [XmlElement(ElementName = "user_plantowatch")]
        [JsonProperty(PropertyName = "plan_to_watch")]
        public int PlanToWatch { get; set; }

        [XmlElement(ElementName = "user_days_spent_watching")]
        [JsonProperty(PropertyName = "days_watching")]
        public double DaysWatching { get; set; }

        [XmlElement(ElementName = "user_onhold")]
        [JsonProperty(PropertyName = "on_hold")]
        public int OnHold { get; set; }

        [XmlElement(ElementName = "user_id")]
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeuroLinker.Models
{
    public class Anime
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "synopsis")]
        public string Synopsis { get; set; }

        [JsonProperty(PropertyName = "other_titles")]
        public Dictionary<string, List<string>> OtherTitles { get; set; }

        [JsonProperty(PropertyName = "summary_stats")]
        public List<string> SummaryStats { get; set; }

        [JsonProperty(PropertyName = "score_stats")]
        public List<string> ScoreStats { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "episodes")]
        public int? Episodes { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "classification")]
        public string Classification { get; set; }

        [JsonProperty(PropertyName = "start_date")]
        public DateTime StartDate { get; set; }

        [JsonProperty(PropertyName = "end_date")]
        public DateTime EndDate { get; set; }

        [JsonProperty(PropertyName = "popularity_rank")]
        public int Popularity { get; set; }

        [JsonProperty(PropertyName = "rank")]
        public int Rank { get; set; }

        [JsonProperty(PropertyName = "image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty(PropertyName = "high_res_image_url")]
        public string HighResImageUrl { get; set; }

        [JsonProperty(PropertyName = "members_score")]
        public double MemberScore { get; set; }

        [JsonProperty(PropertyName = "members_count")]
        public int MemberCount { get; set; }

        [JsonProperty(PropertyName = "favorited_count")]
        public int FavoriteCount { get; set; }

        [JsonProperty(PropertyName = "watched_status")]
        public string UserWatchedStatus { get; set; }

        [JsonProperty(PropertyName = "watched_episodes")]
        public int UserWatchedEpisodes { get; set; }

        [JsonProperty(PropertyName = "score")]
        public int UserScore { get; set; }

        [JsonProperty(PropertyName = "genres")]
        public List<string> Genres { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public List<string> Tags { get; set; }

        [JsonProperty(PropertyName = "additional_info_urls")]
        public InfoUrls AdditionalInfoUrls { get; set; }

        [JsonProperty(PropertyName = "manga_adaptations")]
        public List<Related> MangaAdaptation { get; set; }

        [JsonProperty(PropertyName = "prequels")]
        public List<Related> Prequels { get; set; }

        [JsonProperty(PropertyName = "sequels")]
        public List<Related> Sequels { get; set; }

        [JsonProperty(PropertyName = "side_stories")]
        public List<Related> SideStories { get; set; }

        [JsonProperty(PropertyName = "parent_story")]
        public Related ParentStory { get; set; }

        [JsonProperty(PropertyName = "character_anime")]
        public List<Related> CharacterAnime { get; set; }

        [JsonProperty(PropertyName = "spin_offs")]
        public List<Related> SpinOffs { get; set; }

        [JsonProperty(PropertyName = "summaries")]
        public List<Related> Summaries { get; set; }

        [JsonProperty(PropertyName = "alternative_versions")]
        public List<Related> AlternativeVersion { get; set; }

        [JsonProperty(PropertyName = "alternative_settings")]
        public List<Related> AlternativeSetting { get; set; }

        [JsonProperty(PropertyName = "full_stories")]
        public List<Related> FullStories { get; set; }

        [JsonProperty(PropertyName = "others")]
        public List<Related> Others { get; set; }

        [JsonProperty(PropertyName = "character_voice_actors")]
        public List<CharacterInformation> CharacterInformation { get; set; }

        [JsonProperty(PropertyName = "error_occured")]
        public bool ErrorOccured { get; set; }

        [JsonProperty(PropertyName = "error_message")]
        public string ErrorMessage { get; set; }

        #endregion

        #region Constructor

        public Anime()
        {
            OtherTitles = new Dictionary<string, List<string>>
            {
                {"Japanese", new List<string>()},
                {"English", new List<string>()},
                {"Synonyms", new List<string>()}
            };
            CharacterInformation = new List<CharacterInformation>();
            SummaryStats = new List<string>();
            ScoreStats = new List<string>();
            Genres = new List<string>();
            Tags = new List<string>();
            AdditionalInfoUrls = new InfoUrls();
            MangaAdaptation = new List<Related>();
            Prequels = new List<Related>();
            Sequels = new List<Related>();
            SideStories = new List<Related>();
            ParentStory = null;
            CharacterAnime = new List<Related>();
            SpinOffs = new List<Related>();
            Summaries = new List<Related>();
            AlternativeVersion = new List<Related>();
            Others = new List<Related>();
            ErrorOccured = false;
        }

        #endregion
    }
}
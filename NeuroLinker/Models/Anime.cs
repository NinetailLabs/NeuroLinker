using System;
using System.Collections.Generic;
using NeuroLinker.Interfaces.Models;
using Newtonsoft.Json;

namespace NeuroLinker.Models
{
    /// <summary>
    /// Anime information
    /// </summary>
    public class Anime : IResponseData
    {
        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
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
            FullStories = new List<Related>();
            MangaAdaptation = new List<Related>();
            Prequels = new List<Related>();
            Sequels = new List<Related>();
            SideStories = new List<Related>();
            ParentStory = null;
            CharacterAnime = new List<Related>();
            SpinOffs = new List<Related>();
            Summaries = new List<Related>();
            AlternativeVersion = new List<Related>();
            AlternativeSetting = new List<Related>();
            Others = new List<Related>();
            ErrorOccured = false;
            YearOnlyDate = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Urls that contain additional information about the anime
        /// </summary>
        [JsonProperty(PropertyName = "additional_info_urls")]
        public InfoUrls AdditionalInfoUrls { get; set; }

        /// <summary>
        /// Anime that are related to this show but in an alternative setting
        /// </summary>
        [JsonProperty(PropertyName = "alternative_settings")]
        public List<Related> AlternativeSetting { get; set; }

        /// <summary>
        /// Anime that are related to this show and are an alternative version of the events
        /// </summary>
        [JsonProperty(PropertyName = "alternative_versions")]
        public List<Related> AlternativeVersion { get; set; }

        /// <summary>
        /// Anime that are related to this show that stars some of the same characters
        /// </summary>
        [JsonProperty(PropertyName = "character_anime")]
        public List<Related> CharacterAnime { get; set; }

        /// <summary>
        /// Information about the characters that star in the anime
        /// </summary>
        [JsonProperty(PropertyName = "character_voice_actors")]
        public List<CharacterInformation> CharacterInformation { get; set; }

        /// <summary>
        /// Age rating for the show
        /// </summary>
        [JsonProperty(PropertyName = "classification")]
        public string Classification { get; set; }

        /// <summary>
        /// Date when the anime finished airing
        /// </summary>
        [JsonProperty(PropertyName = "end_date")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// The number of episodes that are in the show.
        /// If the value is unknown it will be null.
        /// </summary>
        [JsonProperty(PropertyName = "episodes")]
        public int? Episodes { get; set; }

        /// <summary>
        /// Error message indicating what went wrong during the anime`s retrieval and parsing
        /// </summary>
        [JsonProperty(PropertyName = "error_message")]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Indicate if an error occured during retrieval or parsing
        /// </summary>
        [JsonProperty(PropertyName = "error_occured")]
        public bool ErrorOccured { get; set; }

        /// <summary>
        /// The number of Mal users that have marked the show as a favorite
        /// </summary>
        [JsonProperty(PropertyName = "favorited_count")]
        public int FavoriteCount { get; set; }

        /// <summary>
        /// Anime that relate to the show that contains the full story rather than an abridged version
        /// </summary>
        [JsonProperty(PropertyName = "full_stories")]
        public List<Related> FullStories { get; set; }

        /// <summary>
        /// Genres that the anime fall in
        /// </summary>
        [JsonProperty(PropertyName = "genres")]
        public List<string> Genres { get; set; }

        /// <summary>
        /// URL to the high quality poster for the anime
        /// </summary>
        [JsonProperty(PropertyName = "high_res_image_url")]
        public string HighResImageUrl { get; set; }

        /// <summary>
        /// Mal Id for the anime
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        /// <summary>
        /// URL to the low quality poster for the anime
        /// </summary>
        [JsonProperty(PropertyName = "image_url")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Manga that the anime was adapted from/to
        /// </summary>
        [JsonProperty(PropertyName = "manga_adaptations")]
        public List<Related> MangaAdaptation { get; set; }

        /// <summary>
        /// Number of Mal members that have watched the show
        /// </summary>
        [JsonProperty(PropertyName = "members_count")]
        public int MemberCount { get; set; }

        /// <summary>
        /// Average score of all Mal members that rated the show
        /// </summary>
        [JsonProperty(PropertyName = "members_score")]
        public double MemberScore { get; set; }

        /// <summary>
        /// Anime that related to this show in some way besides the more general ones
        /// </summary>
        [JsonProperty(PropertyName = "others")]
        public List<Related> Others { get; set; }

        /// <summary>
        /// Alternative titles for the anime
        /// </summary>
        [JsonProperty(PropertyName = "other_titles")]
        public Dictionary<string, List<string>> OtherTitles { get; set; }

        /// <summary>
        /// Anime that is the parent story for this show
        /// </summary>
        [JsonProperty(PropertyName = "parent_story")]
        public Related ParentStory { get; set; }

        /// <summary>
        /// Popularity rank on Mal
        /// </summary>
        [JsonProperty(PropertyName = "popularity_rank")]
        public int Popularity { get; set; }

        /// <summary>
        /// Anime that are prequels to this show
        /// </summary>
        [JsonProperty(PropertyName = "prequels")]
        public List<Related> Prequels { get; set; }

        /// <summary>
        /// Rank on Mal
        /// </summary>
        [JsonProperty(PropertyName = "rank")]
        public int Rank { get; set; }

        /// <summary>
        /// Score statistics from Mal
        /// </summary>
        [JsonProperty(PropertyName = "score_stats")]
        public List<string> ScoreStats { get; set; }

        /// <summary>
        /// Anime that are sequels of this show
        /// </summary>
        [JsonProperty(PropertyName = "sequels")]
        public List<Related> Sequels { get; set; }

        /// <summary>
        /// Anime that are side stories to this show
        /// </summary>
        [JsonProperty(PropertyName = "side_stories")]
        public List<Related> SideStories { get; set; }

        /// <summary>
        /// Anime that are spin-offs from this show
        /// </summary>
        [JsonProperty(PropertyName = "spin_offs")]
        public List<Related> SpinOffs { get; set; }

        /// <summary>
        /// The date when the anime started screening
        /// </summary>
        [JsonProperty(PropertyName = "start_date")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Current Mal status for the show (eg Screening)
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Anime that provides a summary of this show
        /// </summary>
        [JsonProperty(PropertyName = "summaries")]
        public List<Related> Summaries { get; set; }

        /// <summary>
        /// Summary statistics for Mal
        /// </summary>
        [JsonProperty(PropertyName = "summary_stats")]
        public List<string> SummaryStats { get; set; }

        /// <summary>
        /// Synopsis for the show
        /// </summary>
        [JsonProperty(PropertyName = "synopsis")]
        public string Synopsis { get; set; }

        /// <summary>
        /// Mal tags assigned to the show
        /// </summary>
        [JsonProperty(PropertyName = "tags")]
        public List<string> Tags { get; set; }

        /// <summary>
        /// The shows primary title
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// The type of the Anime (eg TV or Movie)
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Score that the user assigned the show.
        /// This value is only retrieved when logged in retrieval is used.
        /// </summary>
        [JsonProperty(PropertyName = "score")]
        public int UserScore { get; set; }

        /// <summary>
        /// Number of episodes the user has watched of the show.
        /// This value is only retrieved when logged in retrieval is used.
        /// </summary>
        [JsonProperty(PropertyName = "watched_episodes")]
        public int UserWatchedEpisodes { get; set; }

        /// <summary>
        /// The user`s current watch status for the show.
        /// This value is only retrieved when logged in retrieval is used.
        /// </summary>
        [JsonProperty(PropertyName = "watched_status")]
        public string UserWatchedStatus { get; set; }

        /// <summary>
        /// Gets or sets whether the start date is a year only date
        /// </summary>
        public bool YearOnlyDate { get; set; }

        #endregion
    }
}
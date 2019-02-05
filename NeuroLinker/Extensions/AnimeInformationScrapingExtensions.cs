using HtmlAgilityPack;
using NeuroLinker.Helpers;
using NeuroLinker.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace NeuroLinker.Extensions
{
    /// <summary>
    /// Process scraped Mal anime pages
    /// </summary>
    public static class AnimeInformationScrapingExtensions
    {
        #region Public Methods

        /// <summary>
        /// Retrieve the air dates for an Anime
        /// </summary>
        /// <param name="anime">Anime instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Anime instance</returns>
        public static Anime RetrieveAirDates(this Anime anime, HtmlDocument doc)
        {
            var dateString = doc
                .RetrieveNodesForInnerSpan("Aired")
                .ChildNodes.Where(t => t.Name == "#text")
                .Select(t => t.InnerText.Replace("\r\n", "").Trim())
                .FirstOrDefault(x => !string.IsNullOrEmpty(x));

            if (dateString == null)
            {
                return anime;
            }

            var dates = Regex.Split(dateString, " to ");
            var startDate = DateTime.MinValue;
            var endDate = DateTime.MinValue;
            var yearOnlyStart = false;
            if (dates.Any())
            {
                DateTime.TryParse(dates[0], out startDate);
            }

            if (dates.Length > 1)
            {
                DateTime.TryParse(dates[1], out endDate);
            }

            if (dates.Any() && int.TryParse(dates.First(), out var yearOnly))
            {
                startDate = new DateTime(yearOnly, 1, 1);
                endDate = new DateTime(yearOnly, 1, 1);
                yearOnlyStart = true;
            }

            anime.StartDate = startDate;
            anime.EndDate = endDate;
            anime.YearOnlyDate = yearOnlyStart;

            return anime;
        }

        /// <summary>
        /// Retrieve all other titles for an Anime
        /// </summary>
        /// <param name="anime">Anime instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Anime instance</returns>
        public static Anime RetrieveAlternativeTitles(this Anime anime, HtmlDocument doc)
        {
            var divNodes = doc.DocumentNode
                               .SelectNodes("//div[@class='spaceit_pad']")
                           ?? doc.DocumentNode.SelectNodes("//div");

            foreach (var node in divNodes)
            {
                var lang = node.ChildNodes
                    .Descendants()
                    .FirstOrDefault()
                    ?.InnerText
                    .Trim(':');

                if (string.IsNullOrEmpty(lang) || AcceptedLanguages.All(x => x != lang))
                {
                    continue;
                }

                var titles = new List<string>();
                var titleNode = node.ChildNodes.Where(t => t.Name == "#text");
                foreach (
                    var title in
                    titleNode.Select(title => title.InnerText.Replace("\r\n", "").Trim())
                        .Where(jTitle => !string.IsNullOrEmpty(jTitle)))
                {
                    titles.AddRange(title.Split(',').Select(t => t.Trim().HtmlDecode()));
                }

                anime.OtherTitles[lang].AddRange(titles);
            }

            return anime;
        }

        /// <summary>
        /// Retrieve Mal Id for the anime
        /// </summary>
        /// <param name="anime">Anime instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Anime instance</returns>
        public static Anime RetrieveAnimeId(this Anime anime, HtmlDocument doc)
        {
            var idString = doc.DocumentNode
                .SelectSingleNode("//input[@type='hidden' and @id='myinfo_anime_id']")
                .Attributes["value"]
                .Value;

            if (int.TryParse(idString, out var aid))
            {
                anime.Id = aid;
            }

            return anime;
        }

        /// <summary>
        /// Retrieve the Anime's title
        /// </summary>
        /// <param name="anime">Anime instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Anime instance</returns>
        public static Anime RetrieveAnimeTitle(this Anime anime, HtmlDocument doc)
        {
            anime.Title = doc.DocumentNode
                .SelectSingleNode("//h1")
                .SelectSingleNode("//span[@itemprop='name']")
                .InnerText
                .HtmlDecode();

            return anime;
        }

        /// <summary>
        /// Retrieve the number of episode for the Anime
        /// </summary>
        /// <param name="anime">Anime instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Anime instance</returns>
        public static Anime RetrieveEpisodes(this Anime anime, HtmlDocument doc)
        {
            var node = doc.RetrieveNodesForInnerSpan("Episodes");
            var epString = node
                .ChildNodes["#text"]
                .InnerText
                .TrimEnd("\n\t".ToCharArray())
                .Trim()
                .ToLower();

            if (epString == "unknown")
            {
                anime.Episodes = -1;
                return anime;
            }

            int.TryParse(epString, out var eps);
            if (eps == 0)
            {
                epString = node.ChildNodes[2].InnerText.Replace("\r\n", "").Trim();
                int.TryParse(epString, out eps);
            }

            anime.Episodes = eps == 0 ? null : (int?)eps;

            return anime;
        }

        /// <summary>
        /// Retrieve the favorite count for Anime
        /// </summary>
        /// <param name="anime">Anime instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Anime instance</returns>
        public static Anime RetrieveFavotireCount(this Anime anime, HtmlDocument doc)
        {
            var favoriteString = doc
                .RetrieveNodesForInnerSpan("Favorites")
                .ChildNodes
                .Where(t => t.Name == "#text")
                .Select(x => x.InnerText.Replace("\r\n", "").Trim().Replace(",", ""))
                .FirstOrDefault(x => !string.IsNullOrEmpty(x));

            if (int.TryParse(favoriteString, out var favorites))
            {
                anime.FavoriteCount = favorites;
            }

            return anime;
        }

        /// <summary>
        /// Retrieve genres for the Anime
        /// </summary>
        /// <param name="anime">Anime instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Anime instance</returns>
        public static Anime RetrieveGenres(this Anime anime, HtmlDocument doc)
        {
            doc
                .RetrieveNodesForInnerSpan("Genres")
                .ChildNodes
                .Where(t => t.Name == "a")
                .ToList()
                .ForEach(x => anime.Genres.Add(x.InnerText.HtmlDecode()));

            return anime;
        }

        /// <summary>
        /// Retrive poster image for an Anime
        /// </summary>
        /// <param name="anime">Anime instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Anime instance</returns>
        public static Anime RetrieveImage(this Anime anime, HtmlDocument doc)
        {
            var img = doc.DocumentNode
                          .SelectSingleNode("//img[@itemprop='image']")
                          ?.Attributes["data-src"]
                          ?.Value
                      ?? doc.DocumentNode
                          .SelectSingleNode("//img[@itemprop='image']")
                          ?.Attributes["src"]
                          ?.Value;

            //If we cannot find an image check if there is a na_series image
            if (string.IsNullOrEmpty(img))
            {
                img = doc.DocumentNode
                          .SelectSingleNode("//img[@src='http://cdn.myanimelist.net/images/qm_50.gif']")
                          ?.Attributes["src"]
                          ?.Value
                      ?? doc.DocumentNode
                          .SelectSingleNode("//img[@src='http://cdn.myanimelist.net/images/na_series.gif']")
                          ?.Attributes["src"]
                          ?.Value;
            }

            anime.ImageUrl = img;
            anime.HighResImageUrl = img?.Insert(img.Length - 4, "l");

            return anime;
        }

        /// <summary>
        /// Retrieve information urls for Anime
        /// </summary>
        /// <param name="anime">Anime instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Anime instance</returns>
        public static Anime RetrieveInfoUrls(this Anime anime, HtmlDocument doc)
        {
            anime.AdditionalInfoUrls.Episodes = doc.GetInfoUrlFor("Episodes");
            anime.AdditionalInfoUrls.Reviews = doc.GetInfoUrlFor("Reviews");
            anime.AdditionalInfoUrls.Recommendation = doc.GetInfoUrlFor("Recommendations");
            anime.AdditionalInfoUrls.Stats = doc.GetInfoUrlFor("Stats");
            anime.AdditionalInfoUrls.News = doc.GetInfoUrlFor("News");
            anime.AdditionalInfoUrls.Forum = doc.GetInfoUrlFor("Forum");
            anime.AdditionalInfoUrls.Featured = doc.GetInfoUrlFor("Featured");
            anime.AdditionalInfoUrls.Clubs = doc.GetInfoUrlFor("Clubs");
            anime.AdditionalInfoUrls.Pictures = doc.GetInfoUrlFor("Pictures");

            var charactersAndStaff = doc.GetInfoUrlFor("Characters & Staff");
            if (string.IsNullOrEmpty(charactersAndStaff))
            {
                charactersAndStaff = doc.GetInfoUrlFor("Characters &amp; Staff");
            }

            anime.AdditionalInfoUrls.CharactersAndStaff = charactersAndStaff;

            return anime;
        }

        /// <summary>
        /// Retrieve member count for an Anime
        /// </summary>
        /// <param name="anime">Anime instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Anime instance</returns>
        public static Anime RetrieveMemberCount(this Anime anime, HtmlDocument doc)
        {
            var memberCountString = doc
                .RetrieveNodesForInnerSpan("Members")
                .ChildNodes
                .Where(t => t.Name == "#text")
                .Select(x => x.InnerText.Replace("\r\n", "").Trim().Replace(",", ""))
                .FirstOrDefault(x => !string.IsNullOrEmpty(x));

            if (int.TryParse(memberCountString, out var members))
            {
                anime.MemberCount = members;
            }

            return anime;
        }

        /// <summary>
        /// Retrieve the Popularity of an Anime
        /// </summary>
        /// <param name="anime">Anime instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Anime instance</returns>
        public static Anime RetrievePopularity(this Anime anime, HtmlDocument doc)
        {
            var popNodes = doc
                .RetrieveNodesForInnerSpan("Popularity")
                .ChildNodes
                .Where(t => t.Name == "#text")
                .Select(x => x.InnerText.Trim().TrimStart('#'))
                .FirstOrDefault(x => !string.IsNullOrEmpty(x));

            if (int.TryParse(popNodes, out var popularity))
            {
                anime.Popularity = popularity;
            }

            return anime;
        }

        /// <summary>
        /// Retrieve the Rank of an Anime
        /// </summary>
        /// <param name="anime">Anime instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Anime instance</returns>
        public static Anime RetrieveRank(this Anime anime, HtmlDocument doc)
        {
            var rankString = doc
                .RetrieveNodesForInnerSpan("Ranked")
                .ChildNodes
                .Where(t => t.Name == "#text")
                .Select(x => x.InnerText.Replace("\r\n", "").Trim().TrimStart('#'))
                .FirstOrDefault(x => !string.IsNullOrEmpty(x));

            if (int.TryParse(rankString, out var rank))
            {
                anime.Rank = rank;
            }

            return anime;
        }

        /// <summary>
        /// Retrieve Anime's rating
        /// </summary>
        /// <param name="anime">Anime instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Anime instance</returns>
        public static Anime RetrieveRating(this Anime anime, HtmlDocument doc)
        {
            var txt = doc
                .RetrieveNodesForInnerSpan("Rating")
                .InnerText.Replace("\r\n", "");

            anime.Classification =
                Regex
                    .Split(txt, ":")
                    .Last()
                    .Trim()
                    .Replace("Rating:", "")
                    .Replace("Rating:", "")
                    .Trim(Environment.NewLine.ToCharArray())
                    .Trim()
                    .HtmlDecode();

            return anime;
        }

        /// <summary>
        /// Retrieve related Anime and Manga for the Anime
        /// </summary>
        /// <param name="anime">Anime instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Anime instance</returns>
        public static Anime RetrieveRelatedAnime(this Anime anime, HtmlDocument doc)
        {
            var relatedNodes = doc.DocumentNode
                .SelectSingleNode("//table[@class='anime_detail_related_anime']");

            return relatedNodes == null
                ? anime
                : relatedNodes.ChildNodes.Count > 1
                    ? anime.ParseRelatedTableRows(relatedNodes)
                    : anime.ParseRelatedTableCells(relatedNodes.FirstChild);
        }

        /// <summary>
        /// Retrieve the community score for an anime
        /// </summary>
        /// <param name="anime">Anime instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Anime instance</returns>
        public static Anime RetrieveScore(this Anime anime, HtmlDocument doc)
        {
            var node = doc.RetrieveNodesForInnerSpan("Score");
            var scoreNode = node.SelectNodes("//span[@itemprop='ratingValue']");

            var scoreString = scoreNode?.Count >= 1
                ? scoreNode[0].InnerText
                : node.ChildNodes["#text"].InnerText;

            if (double.TryParse(scoreString, NumberStyles.Any, CultureInfo.InvariantCulture, out var scoreVal))
            {
                anime.MemberScore = scoreVal;
            }

            return anime;
        }

        /// <summary>
        /// Retrieve the Anime's current status
        /// </summary>
        /// <param name="anime">Anime instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Anime instance</returns>
        public static Anime RetrieveStatus(this Anime anime, HtmlDocument doc)
        {
            anime.Status = doc
                .RetrieveNodesForInnerSpan("Status")
                .ChildNodes
                .Where(t => t.Name == "#text")
                .Select(t => t.InnerText.Replace("\r\n", "").Trim())
                .FirstOrDefault(x => !string.IsNullOrEmpty(x))
                .HtmlDecode();

            return anime;
        }

        /// <summary>
        /// Retrieve synopsis information for an Anime
        /// </summary>
        /// <param name="anime">Anime instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Anime instance</returns>
        public static Anime RetrieveSynopsis(this Anime anime, HtmlDocument doc)
        {
            var synopsis = doc.DocumentNode
                               .SelectSingleNode("//span[@itemprop='description']")
                               ?.InnerText
                           ?? string.Empty;

            if (string.IsNullOrEmpty(synopsis))
            {
                var tableRows = doc.DocumentNode.SelectNodes("//td[@valign='top']");
                foreach (var row in tableRows)
                {
                    var header = row.ChildNodes["h2"];
                    if (header == null || !header.InnerText.Contains("Synopsis"))
                    {
                        continue;
                    }

                    var synopsisData = row.ChildNodes
                        .Where(t => t.Name == "#text")
                        .Select(t => t.InnerText)
                        .ToList();
                    synopsis = synopsisData[1];
                    break;
                }
            }

            synopsis = synopsis
                .TrimStart("\r\n".ToCharArray())
                .Trim()
                .HtmlDecode();
            if (!synopsis.Contains("\r\n") && synopsis.Contains("\n"))
            {
                synopsis = synopsis.Replace("\n", "\r\n");
            }

            anime.Synopsis = synopsis;

            return anime;
        }

        /// <summary>
        /// Retrieve the Mal Type of the Anime
        /// </summary>
        /// <param name="anime">Anime instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Anime instance</returns>
        public static Anime RetrieveType(this Anime anime, HtmlDocument doc)
        {
            anime.Type = doc
                .RetrieveNodesForInnerSpan("Type")
                .ChildNodes
                .Where(t => t.Name == "a")
                .FirstOrDefault(
                    innerNode =>
                        innerNode.Attributes["href"].Value.StartsWith("https://myanimelist.net/topanime.php?type="))?
                .InnerText.Replace("\r\n", "")
                .Trim();

            if (string.IsNullOrEmpty(anime.Type))
            {
                anime.Type = doc
                    .RetrieveNodesForInnerSpan("Type")
                    .ChildNodes
                    .LastOrDefault(x => x.Name == "#text")
                    ?.InnerHtml
                    .Replace("\r\n", "")
                    .Trim()
                    .HtmlDecode();
            }

            return anime;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Retrieve info url
        /// </summary>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <param name="infoUrlName">Name of the info url to retrieve</param>
        /// <returns>Info url</returns>
        private static string GetInfoUrlFor(this HtmlDocument doc, string infoUrlName)
        {
            return doc
                .DocumentNode
                .SelectNodes("//div[@id='horiznav_nav']")
                .SelectMany(x => x.ChildNodes["ul"].ChildNodes)
                .Where(x => x.ChildNodes["a"]?.InnerText == infoUrlName)
                .Select(x => x.ChildNodes["a"]?.Attributes["href"].Value)
                .FirstOrDefault();
        }

        /// <summary>
        /// Parse the node data for Related items into a list of <see cref="Related"/> items
        /// </summary>
        /// <param name="node">Node that should be processed</param>
        /// <returns>List of related anime</returns>
        private static IEnumerable<Related> ParseRelatedNodeData(HtmlNode node)
        {
            var relatedShows = new List<Related>();

            var linkNodes = node
                .ChildNodes[1]
                .ChildNodes
                .Where(x => x.Name == "a")
                .ToList();

            foreach (var link in linkNodes)
            {
                var url = MalRouteBuilder.MalCleanUrl(link.Attributes["href"].Value);
                int.TryParse(url.Split('/')[4], out var id);

                relatedShows.Add(new Related
                {
                    Id = id,
                    Title = link.InnerText,
                    Url = url
                });
            }

            return relatedShows;
        }

        /// <summary>
        /// Parses the cell data in related table's rows
        /// </summary>
        /// <param name="node">Node containing the first row</param>
        /// <param name="anime">Anime instance to populate</param>
        /// <returns>Anime instance</returns>
        private static Anime ParseRelatedTableCells(this Anime anime, HtmlNode node)
        {
            while (true)
            {
                switch (node.ChildNodes[0].InnerText.Replace(":", "").ToLower())
                {
                    case "adaptation":
                        anime.MangaAdaptation.AddRange(ParseRelatedNodeData(node));
                        break;

                    case "prequel":
                        anime.Prequels.AddRange(ParseRelatedNodeData(node));
                        break;

                    case "sequel":
                        anime.Sequels.AddRange(ParseRelatedNodeData(node));
                        break;

                    case "side story":
                        anime.SideStories.AddRange(ParseRelatedNodeData(node));
                        break;

                    case "parent story":
                        anime.ParentStory = ParseRelatedNodeData(node).FirstOrDefault();
                        break;

                    case "character":
                        anime.CharacterAnime.AddRange(ParseRelatedNodeData(node));
                        break;

                    case "spin-off":
                        anime.SpinOffs.AddRange(ParseRelatedNodeData(node));
                        break;

                    case "summary":
                        anime.Summaries.AddRange(ParseRelatedNodeData(node));
                        break;

                    case "alternative version":
                        anime.AlternativeVersion.AddRange(ParseRelatedNodeData(node));
                        break;

                    case "alternative setting":
                        anime.AlternativeSetting.AddRange(ParseRelatedNodeData(node));
                        break;

                    case "full story":
                        anime.FullStories.AddRange(ParseRelatedNodeData(node));
                        break;

                    default:
                        anime.Others.AddRange(ParseRelatedNodeData(node));
                        break;
                }

                var nextNode = node.ChildNodes.Last();
                if (nextNode.Name == "tr")
                {
                    node = nextNode;
                    continue;
                }

                break;
            }

            return anime;
        }

        /// <summary>
        /// If the related items is a proper table with multiple ```tr``` rows instead of one ```tr``` row with another embedded ```tr``` then the direct parsing method fails,
        /// instead this method should be called to parse each individual ```tr```
        /// </summary>
        /// <param name="anime">Anime instance to populate</param>
        /// <param name="rowsToParse">Node containing the rows</param>
        /// <returns>Anime instance</returns>
        private static Anime ParseRelatedTableRows(this Anime anime, HtmlNode rowsToParse)
        {
            foreach (var row in rowsToParse.ChildNodes)
            {
                if (row.Name == "tr")
                {
                    anime.ParseRelatedTableCells(row);
                }
            }

            return anime;
        }

        /// <summary>
        /// Retrieve a node collection for a specific inner span
        /// </summary>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <param name="spanText">String to look for in the inner text</param>
        /// <returns>Node that contains the requested text</returns>
        private static HtmlNode RetrieveNodesForInnerSpan(this HtmlDocument doc, string spanText)
        {
            var nodeCollection = doc.DocumentNode
                .SelectNodes("//div")
                .FirstOrDefault(node => node.ChildNodes.Descendants()
                                            .FirstOrDefault()
                                            ?.InnerText.Trim(':') == spanText);

            return nodeCollection;
        }

        #endregion

        #region Variables

        private static readonly List<string> AcceptedLanguages = new List<string> { "English", "Japanese", "Synonyms" };

        #endregion
    }
}
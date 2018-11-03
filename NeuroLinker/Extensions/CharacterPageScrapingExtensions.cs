using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using NeuroLinker.Models;

namespace NeuroLinker.Extensions
{
    /// <summary>
    /// Scrape information from Character's page
    /// </summary>
    public static class CharacterPageScrapingExtensions
    {
        #region Public Methods

        /// <summary>
        /// Retrieve character's Animeography
        /// </summary>
        /// <param name="character">Character instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Character instance</returns>
        public static Character RetrieveAnimeography(this Character character, HtmlDocument doc)
        {
            var rows = doc
                .GetOgraphyTables()[0]
                .ChildNodes
                .Where(x => x.Name == "tr");

            character.Animeography = ParseOgraphy(rows);

            return character;
        }

        /// <summary>
        /// Retrieves a character's biography
        /// </summary>
        /// <param name="character">Character instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Character instance</returns>
        public static Character RetrieveBiography(this Character character, HtmlDocument doc)
        {
            var bioData = doc.DocumentNode
                .SelectNodes("//table")[0]
                .ChildNodes["tr"]
                .ChildNodes[3]
                .ChildNodes
                .Where(t => t.Name == "#text");

            //We also need to pull in any spoiler info
            var spoilerData = doc.DocumentNode.SelectNodes("//div[@class='spoiler']");

            var sBuilder = new StringBuilder();
            foreach (var item in bioData)
            {
                var tmpText = item.InnerText.Replace("\r\n", "").Trim();
                sBuilder.Append($"{tmpText} ");
            }

            if (spoilerData?.Count > 0)
            {
                sBuilder.Append(" <SPOILER>");
                foreach (var spoiler in spoilerData)
                {
                    sBuilder.Append($"{spoiler.InnerText.Replace("<!--spoiler-->", "")} ");
                }

                sBuilder.Append("</SPOILER>");
            }

            character.Biography = sBuilder.ToString().Trim().HtmlDecode();

            return character;
        }

        /// <summary>
        /// Get the Character's image URL
        /// </summary>
        /// <param name="character">Character instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Character instance</returns>
        public static Character RetrieveCharacterImage(this Character character, HtmlDocument doc)
        {
            var image = doc.DocumentNode
                .SelectNodes("//table")[0]
                .ChildNodes["tr"]
                .ChildNodes["td"]
                .ChildNodes["div"]
                .ChildNodes["a"]
                .ChildNodes["img"];

            character.ImageUrl = (image.Attributes["data-src"] ?? image.Attributes["src"]).Value;
            return character;
        }

        /// <summary>
        /// Get the Character's name
        /// </summary>
        /// <param name="character">Character instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Character instance</returns>
        public static Character RetrieveCharacterName(this Character character, HtmlDocument doc)
        {
            character.Name = doc.DocumentNode
                .SelectNodes("//div[@class='normal_header']")
                .ToList()[2]
                .InnerText
                .HtmlDecode();

            return character;
        }

        /// <summary>
        /// Get the Character's favorite count
        /// </summary>
        /// <param name="character">Character instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Character instance</returns>
        public static Character RetrieveFavoriteCount(this Character character, HtmlDocument doc)
        {
            var count = doc.DocumentNode
                .SelectNodes("//table")[0]
                .ChildNodes["tr"]
                .ChildNodes["td"]
                .ChildNodes
                .FirstOrDefault(t => t.InnerText.Contains("Member Favorites"))
                ?.InnerText
                .Replace("\r\n", "")
                .Replace("Member Favorites:", "")
                .Replace(",", "")
                .Trim();

            if (int.TryParse(count, out var memberCount))
            {
                character.FavoriteCount = memberCount;
            }

            return character;
        }

        /// <summary>
        /// Retrieve character's Mangaography
        /// </summary>
        /// <param name="character">Character instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Character instance</returns>
        public static Character RetrieveMangaograhy(this Character character, HtmlDocument doc)
        {
            var rows = doc
                .GetOgraphyTables()[1]
                .ChildNodes
                .Where(x => x.Name == "tr");

            character.Mangaography = ParseOgraphy(rows);

            return character;
        }

        /// <summary>
        /// Retrieve character's seiyuu
        /// </summary>
        /// <param name="character">Character instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Character instance</returns>
        public static Character RetrieveSeiyuu(this Character character, HtmlDocument doc)
        {
            var tables = doc.DocumentNode
                .SelectNodes("//table")
                .Skip(3);

            foreach (var table in tables)
            {
                var seiyuu = new SeiyuuInformation();
                var info = table
                    .ChildNodes["tr"]
                    .ChildNodes
                    .Where(x => x.Name == "td")
                    .ToList();

                seiyuu.PictureUrl = info[0]
                    .ChildNodes["div"]
                    .ChildNodes["a"]
                    .ChildNodes["img"]
                    .Attributes["src"]
                    .Value;

                seiyuu.Name = info[1]
                    .ChildNodes["a"]
                    .InnerText
                    .HtmlDecode();

                seiyuu.Url = info[1]
                    .ChildNodes["a"]
                    .Attributes["href"]
                    .Value;

                seiyuu.Language = info[1]
                    .ChildNodes["div"]
                    .ChildNodes["small"]
                    .InnerText;

                if (int.TryParse(seiyuu.Url.Split('/')[4], out var id))
                {
                    seiyuu.Id = id;
                }

                character.Seiyuu.Add(seiyuu);
            }

            return character;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Return tables that contain anime- and mangaography
        /// </summary>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Tables that contain Ography data</returns>
        private static List<HtmlNode> GetOgraphyTables(this HtmlDocument doc)
        {
            return doc.DocumentNode
                .SelectNodes("//table")[0]
                .ChildNodes["tr"]
                .ChildNodes["td"]
                .ChildNodes
                .Where(t => t.Name == "table")
                .ToList();
        }

        /// <summary>
        /// Parse rows containing Ography
        /// </summary>
        /// <param name="ographyNodes">Nodes that should be parsed for ography details</param>
        /// <returns>List of ography items</returns>
        private static List<Ography> ParseOgraphy(IEnumerable<HtmlNode> ographyNodes)
        {
            var results = new List<Ography>();

            foreach (var row in ographyNodes)
            {
                var tmpEntry = new Ography();

                var cells = row.ChildNodes
                    .Where(x => x.Name == "td")
                    .ToList();

                tmpEntry.ImageUrl = cells
                    .First(x => x.FirstChild.Name == "div")
                    .FirstChild
                    .ChildNodes["a"]
                    .ChildNodes["img"]
                    .Attributes["src"]
                    .Value;

                var details = cells
                    .First(x => x.Name == "td" && (x.FirstChild.Name == "#text" || x.FirstChild.Name == "a"))
                    .ChildNodes
                    .First(x => x.Name == "a");

                tmpEntry.Url = details.Attributes["href"].Value;
                tmpEntry.Name = details.InnerText.HtmlDecode();
                if (int.TryParse(tmpEntry.Url.Split('/')[4], out var id))
                {
                    tmpEntry.Id = id;
                }

                results.Add(tmpEntry);
            }

            return results;
        }

        #endregion
    }
}
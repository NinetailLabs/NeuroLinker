using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using NeuroLinker.Models;

namespace NeuroLinker.Extensions
{
    /// <summary>
    /// Scrapes character information from the Information page in the anime.
    /// This information is not for individual characters but rather basic information for all characters in a show
    /// </summary>
    public static class CharacterInformationScrapingExtensions
    {
        #region Public Methods

        /// <summary>
        /// Populates the Character and Seiyuu information for an anime
        /// </summary>
        /// <param name="anime">Anime for which the Seiyuu and Characters should be populated</param>
        /// <param name="doc">HtmlDocument from which information should be retrieved</param>
        /// <returns>Anime populated with the Seiyuu and characters</returns>
        public static Anime PopulateCharacterAndSeiyuuInformation(this Anime anime, HtmlDocument doc)
        {
            var rows = doc.DocumentNode
                .SelectNodes("//table")
                .SelectMany(table => table.ChildNodes)
                .Where(row => row.Name == "tr" && row.ChildNodes.Count(x => x.Name == "td") == 3);

            foreach (var row in rows)
            {
                var columns = row.ChildNodes
                    .Where(t => t.Name == "td")
                    .ToList();
                var vaDetail = columns[2]
                                   .ChildNodes["table"]
                                   ?.ChildNodes.Where(t => t.Name == "tr")
                                   .ToList()
                               ?? Enumerable.Empty<HtmlNode>();

                var tmpChar = CreateCharacter(columns)
                    .PopulateSeiyuu(vaDetail);

                if (anime.CharacterInformation.All(t => t.CharacterUrl != tmpChar.CharacterUrl))
                {
                    anime.CharacterInformation.Add(tmpChar);
                }
            }

            return anime;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Create a new Character instance from HtmlNodes
        /// </summary>
        /// <param name="nodes">HtmlNodes containing the character information</param>
        /// <returns>Character instance</returns>
        private static CharacterInformation CreateCharacter(IList<HtmlNode> nodes)
        {
            var picLocation = nodes[0]
                .ChildNodes["div"]
                .ChildNodes["a"]
                .ChildNodes["img"];

            var url = nodes[0]
                .ChildNodes["div"]
                .ChildNodes["a"]
                .Attributes["href"]
                .Value;
            int.TryParse(url.Split('/')[4], out var id);

            var name = nodes[1].ChildNodes
                .First(x => x.Name == "div" && x.ChildNodes.Any(z => z.Name == "a"))
                .ChildNodes["a"]
                .ChildNodes["h3"]
                .InnerText
                .HtmlDecode();

            var charType = nodes[1].ChildNodes
                .Where(x => x.Name == "div")
                .ToList()[3]
                .InnerText
                .Replace("\r\n", "")
                .Replace("\n", "")
                .Replace(" ", "")
                .HtmlDecode()
                .Trim();

            var newChar = new CharacterInformation
            {
                CharacterPicture = (picLocation.Attributes["data-src"] ?? picLocation.Attributes["src"])?.Value,
                CharacterName = name,
                CharacterUrl = url,
                CharacterType = charType,
                Id = id
            };

            return newChar;
        }

        /// <summary>
        /// Populate Seiyuu information for a character
        /// </summary>
        /// <param name="character">Character to which the Seiyuu information should be added</param>
        /// <param name="seiyuuInfoNodes">HtmlNodes containing the Seiyuu information</param>
        /// <returns>Character instance</returns>
        private static CharacterInformation PopulateSeiyuu(this CharacterInformation character,
            IEnumerable<HtmlNode> seiyuuInfoNodes)
        {
            foreach (var detail in seiyuuInfoNodes)
            {
                var picNode = detail.ChildNodes[3]
                    .ChildNodes["div"]
                    .ChildNodes["a"]
                    .ChildNodes["img"];

                var language = detail.ChildNodes["td"].ChildNodes
                    .First(x => x.Attributes.Any(z => z.Value == "spaceit_pad js-anime-character-language"))
                    .InnerText
                    .Replace("\r\n", "")
                    .Replace("\n", "")
                    .Replace(" ", "");

                var tmpSeiyuu = new SeiyuuInformation
                {
                    Language = language,
                    Name = detail.ChildNodes["td"].ChildNodes["div"].ChildNodes["a"].InnerText.HtmlDecode(),
                    Url = detail.ChildNodes["td"].ChildNodes["div"].ChildNodes["a"].Attributes["href"].Value,
                    PictureUrl = (picNode.Attributes["data-src"] ?? picNode.Attributes["src"])?.Value
                };

                if (int.TryParse(tmpSeiyuu.Url.Split('/')[4], out var id))
                {
                    tmpSeiyuu.Id = id;
                }

                character.Seiyuu.Add(tmpSeiyuu);
            }

            return character;
        }

        #endregion
    }
}
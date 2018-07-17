using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using NeuroLinker.Helpers;
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

            var url = nodes[1].ChildNodes["a"].Attributes["href"].Value;
            int.TryParse(url.Split('/')[4], out var id);

            return new CharacterInformation
            {
                CharacterPicture = (picLocation.Attributes["data-src"] ?? picLocation.Attributes["src"])?.Value,
                CharacterName = nodes[1].ChildNodes["a"].InnerText.HtmlDecode(),
                CharacterUrl = url,
                CharacterType = nodes[1]
                    .ChildNodes["div"]
                    .InnerText
                    .Replace("\r\n", "")
                    .Replace("\n", "")
                    .Replace(" ", "")
                    .HtmlDecode(),
                Id = id
            };
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

                var tmpSeiyuu = new SeiyuuInformation
                {
                    Language = detail.ChildNodes["td"].ChildNodes["small"].InnerText,
                    Name = detail.ChildNodes["td"].ChildNodes["a"].InnerText.HtmlDecode(),
                    Url = detail.ChildNodes["td"].ChildNodes["a"].Attributes["href"].Value,
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using NeuroLinker.Helpers;
using NeuroLinker.Models;

namespace NeuroLinker.Extensions
{
    /// <summary>
    /// Scrape Seiyuu pages
    /// </summary>
    public static class SeiyuuPageScraperExtensions
    {
        #region Public Methods

        /// <summary>
        /// Retrieve Seiyuu information under the more heading
        /// </summary>
        /// <param name="seiyuu">Seiyuu instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Seiyuu instance</returns>
        public static Seiyuu RetrieveAdditionalInformation(this Seiyuu seiyuu, HtmlDocument doc)
        {
            var node = doc
                .DocumentNode
                .SelectNodes("//div")
                .FirstOrDefault(x => x.Attributes["class"]?.Value.Contains("people-informantion-more") ?? false)
                ?.InnerText;

            var info = node?.Split("\r\n".ToCharArray()).ToList() ?? new List<string>();
            var sb = new StringBuilder();
            foreach (var entry in info)
            {
                if (entry == "")
                {
                    continue;
                }

                if (entry.EndsWith(":"))
                {
                    var currentList = sb.ToString().TrimEnd(',');
                    sb.Clear();
                    if (!string.IsNullOrEmpty(currentList))
                    {
                        seiyuu.More.Add(currentList);
                    }

                    sb.Append($"{entry} ");
                }
                else if (entry.Contains(":"))
                {
                    seiyuu.More.Add(entry);
                }
                else
                {
                    sb.Append($"{entry},");
                }

            }
            if (!string.IsNullOrEmpty(sb.ToString()))
            {
                seiyuu.More.Add(sb.ToString().TrimEnd(','));
            }

            return seiyuu;
        }

        /// <summary>
        /// Retrieve Seiyuu's birthday
        /// </summary>
        /// <param name="seiyuu">Seiyuu instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Seiyuu instance</returns>
        public static Seiyuu RetrieveBirthday(this Seiyuu seiyuu, HtmlDocument doc)
        {
            var stringBirthday = doc
                .DocumentNode
                .SelectNodes("//div")
                .FirstOrDefault(x => x.FirstChild?.Name == "span" && x.FirstChild?.InnerText == "Birthday:")
                ?.ChildNodes["#text"]
                ?.InnerText
                .Trim();

            DateTime bday;
            if (DateTime.TryParse(stringBirthday, out bday))
            {
                seiyuu.BirthDay = bday;
            }

            return seiyuu;
        }

        /// <summary>
        /// Retrieve Seiyuu's family name
        /// </summary>
        /// <param name="seiyuu">Seiyuu instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Seiyuu instance</returns>
        public static Seiyuu RetrieveFamilyName(this Seiyuu seiyuu, HtmlDocument doc)
        {
            seiyuu.FamilyName = doc
                .DocumentNode
                .SelectNodes("//span")
                .FirstOrDefault(x => x.InnerText == "Family name:")
                ?.NextSibling
                ?.InnerText
                .Trim();

            return seiyuu;
        }

        /// <summary>
        /// Retrieve Seiyuu's given name
        /// </summary>
        /// <param name="seiyuu">Seiyuu instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Seiyuu instance</returns>
        public static Seiyuu RetrieveGivenName(this Seiyuu seiyuu, HtmlDocument doc)
        {
            seiyuu.GivenName = doc
                .DocumentNode
                .SelectNodes("//div")
                .FirstOrDefault(x => x.FirstChild?.Name == "span" && x.FirstChild?.InnerText == "Given name:")
                ?.ChildNodes["#text"]
                ?.InnerText
                .Trim();

            return seiyuu;
        }

        /// <summary>
        /// Retrieve Seiyuu's name
        /// </summary>
        /// <param name="seiyuu">Seiyuu instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Seiyuu instance</returns>
        public static Seiyuu RetrieveName(this Seiyuu seiyuu, HtmlDocument doc)
        {
            seiyuu.Name = doc.DocumentNode
                .SelectNodes("//meta")
                .FirstOrDefault(x => x.Attributes["property"]?.Value == "og:title")
                ?.Attributes["content"]
                ?.Value;

            return seiyuu;
        }

        /// <summary>
        /// Retrieve the Seiyuu's roles
        /// </summary>
        /// <param name="seiyuu">Seiyuu instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Seiyuu instance</returns>
        public static Seiyuu RetrieveRoles(this Seiyuu seiyuu, HtmlDocument doc)
        {
            var rows = doc.DocumentNode
                .SelectNodes("//table")[1]
                .ChildNodes
                .Where(x => x.Name == "tr");

            foreach (var row in rows)
            {
                var cells = row.ChildNodes
                    .Where(x => x.Name == "td")
                    .ToList();

                var role = CreateRoleAndPopulateWithAnimeInformation(cells)
                    .PopulateRoleWithCharacterInformation(cells);
                
                seiyuu.Roles.Add(role);
            }

            return seiyuu;
        }

        /// <summary>
        /// Retrieve Seiyuu's website
        /// </summary>
        /// <param name="seiyuu">Seiyuu instance to populate</param>
        /// <param name="doc">Html document from which data should be pulled</param>
        /// <returns>Seiyuu instance</returns>
        public static Seiyuu RetrieveWebsite(this Seiyuu seiyuu, HtmlDocument doc)
        {
           seiyuu.Website = doc
                .DocumentNode
                .SelectNodes("//span")
                .FirstOrDefault(x => x.InnerText == "Website:")
                ?.NextSibling
                ?.NextSibling
                ?.Attributes["href"]
                .Value;

            return seiyuu;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Create a <see cref="Roles"/> instance and populate it with anime data
        /// </summary>
        /// <param name="roleNodes">Nodes containing role information</param>
        /// <returns>Role instance with populated anime data</returns>
        private static Roles CreateRoleAndPopulateWithAnimeInformation(IReadOnlyList<HtmlNode> roleNodes)
        {
            var role = new Roles
            {
                AnimeUrl = MalRouteBuilder.MalCleanUrl(roleNodes[0]
                    .ChildNodes["div"]
                    .ChildNodes["a"]
                    ?.Attributes["href"]
                    .Value)
            };

            int id;
            int.TryParse(role.AnimeUrl.Split('/')[4], out id);
            role.AnimeId = id;

            var animeImg = roleNodes[0]
                .ChildNodes["div"]
                .ChildNodes["a"]
                ?.ChildNodes["img"];

            role.AnimePicUrl = (animeImg?.Attributes["data-src"] ?? animeImg?.Attributes["src"])?.Value;
            role.AnimeTitle = roleNodes[1]
                .ChildNodes["a"]
                ?.InnerText;

            return role;
        }

        /// <summary>
        /// Populate a <see cref="Roles"/> with character information
        /// </summary>
        /// <param name="role">Role instance that should be populated</param>
        /// <param name="roleNodes">Nodes containing role information</param>
        private static Roles PopulateRoleWithCharacterInformation(this Roles role, IReadOnlyList<HtmlNode> roleNodes)
        {
            role.CharacterName = roleNodes[2]
                    .ChildNodes["a"]
                    .InnerText
                    .Trim();

            role.CharacterUrl = MalRouteBuilder.MalCleanUrl(roleNodes[2]
                .ChildNodes["a"]
                .Attributes["href"]
                .Value);

            int id;
            int.TryParse(role.CharacterUrl.Split('/')[4], out id);
            role.CharacterId = id;

            role.RoleType = roleNodes[2]
                .ChildNodes["div"]
                .InnerText
                .Replace("&nbsp;", "")
                .Trim();

            var charImage = roleNodes[3]
                .ChildNodes["div"]
                .ChildNodes["a"]
                .ChildNodes["img"];

            role.CharacterPic = (charImage?.Attributes["data-src"] ?? charImage?.Attributes["src"])?.Value;

            return role;
        }

        #endregion
    }
}
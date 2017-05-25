using System.Collections.Generic;
using NeuroLinker.Interfaces.Models;

namespace NeuroLinker.Models
{
    /// <summary>
    /// Character information
    /// </summary>
    public class Character : IResponseData
    {
        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Character()
        {
            Seiyuu = new List<SeiyuuInformation>();
            Animeography = new List<Ography>();
            Mangaography = new List<Ography>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Anime that the character has appeared int
        /// </summary>
        public List<Ography> Animeography { get; set; }

        /// <summary>
        /// Character biography
        /// </summary>
        public string Biography { get; set; }

        /// <summary>
        /// Message indicating what went wrong while retrieving the character
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Did an error occur during data parsing
        /// </summary>
        public bool ErrorOccured { get; set; }

        /// <summary>
        /// Number of users that have favorited the character
        /// </summary>
        public int FavoriteCount { get; set; }

        /// <summary>
        /// Character Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Image Url for the character
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Manga that the character has appeared in
        /// </summary>
        public List<Ography> Mangaography { get; set; }

        /// <summary>
        /// The characters name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Seiyuu for the character
        /// </summary>
        public List<SeiyuuInformation> Seiyuu { get; set; }

        /// <summary>
        /// Url where the character can be accessed
        /// </summary>
        public string Url { get; set; }

        #endregion
    }
}
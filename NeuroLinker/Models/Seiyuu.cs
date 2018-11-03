using System;
using System.Collections.Generic;
using NeuroLinker.Interfaces.Models;

namespace NeuroLinker.Models
{
    /// <summary>
    /// Seiyuu information
    /// </summary>
    public class Seiyuu : IResponseData
    {
        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Seiyuu()
        {
            Roles = new List<Roles>();
            More = new List<string>();
            ErrorOccured = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Seiyuu`s birthday
        /// </summary>
        public DateTime BirthDay { get; set; }

        /// <summary>
        /// Information about errors that occured during Seiyuu data retrieval
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Indicate if an error occured during retrieval or parsing
        /// </summary>
        public bool ErrorOccured { get; set; }

        /// <summary>
        /// Seiyuu`s family name
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// Seiyuu`s favorite stuff
        /// </summary>
        public string Favorites { get; set; }

        /// <summary>
        /// Seiyuu`s given name
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// Mal Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Seiyuu`s Mal image URL
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Other information
        /// </summary>
        public List<string> More { get; set; }

        /// <summary>
        /// Seiyuu`s name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Character that the Seiyuu has played
        /// </summary>
        public List<Roles> Roles { get; set; }

        /// <summary>
        /// Mal URL where the Seiyuu can be accessed
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Seiyuu`s website
        /// </summary>
        public string Website { get; set; }

        #endregion
    }
}
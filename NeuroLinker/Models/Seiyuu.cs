using System;
using System.Collections.Generic;

namespace NeuroLinker.Models
{
    public class Seiyuu
    {
        #region Constructor

        public Seiyuu()
        {
            Roles = new List<Roles>();
            More = new List<string>();
            ErrorOccured = false;
        }

        #endregion

        #region Properties

        public DateTime BirthDay { get; set; }

        public string ErrorMessage { get; set; }

        public bool ErrorOccured { get; set; }

        public string FamilyName { get; set; }

        public string Favorites { get; set; }

        public string GivenName { get; set; }
        public int Id { get; set; }

        public List<string> More { get; set; }

        public string Name { get; set; }

        public List<Roles> Roles { get; set; }

        public string Website { get; set; }

        #endregion
    }
}
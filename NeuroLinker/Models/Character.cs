using System.Collections.Generic;

namespace NeuroLinker.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Biography { get; set; }
        public int FavoriteCount { get; set; }
        public string Url { get; set; }

        public List<SeiyuuInformation> Seiyuu { get; set; }
        public List<Ography> Animeography { get; set; }
        public List<Ography> Mangaography { get; set; }

        public string ErrorMessage { get; set; }
        public bool ErrorOccured { get; set; }

        public Character()
        {
            Seiyuu = new List<SeiyuuInformation>();
            Animeography = new List<Ography>();
            Mangaography = new List<Ography>();
        }
    }
}
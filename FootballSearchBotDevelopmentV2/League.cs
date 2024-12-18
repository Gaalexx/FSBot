using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    class League
    {
        public string Link { get; set; }
        public string Name { get; set; }

        public string Pin { get; set; }
        public string Data { get; set; }

        public short CountryID { get; set; }
        public short TeamsSum { get; set; }

        //public List<string> TeamList { get; set; } = new List<string>();
        public Dictionary<int, string> TeamDic { get; set; } = new Dictionary<int, string>();

        public League(string leagueLink, string leagueName, short countryID)
        {
            Link = leagueLink;
            Name = leagueName;
            CountryID = countryID;
        }
    }
}

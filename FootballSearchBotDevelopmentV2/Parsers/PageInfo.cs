using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;


namespace Program
{
    class PageInfo
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public DateTime Time { get; set; }
        public List<string> Tag { get; set; } = new List<string>();
        public string Data { get; set; }


    }
}

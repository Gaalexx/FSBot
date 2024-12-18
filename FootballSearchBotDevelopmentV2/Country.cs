using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    class Country
    {
        public Dictionary<short, League> Divisions { get; set; } = new Dictionary<short, League>();
        public string Pin { get; set; }
        public string Name { get; set; }
        public short ID { get; set; }
        public Country(string name, string pin, short id)
        {
            Name = name;
            Pin = pin;
            ID = id;
        }
    }
}

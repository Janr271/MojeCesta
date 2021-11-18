

using System;

namespace MojeCesta.Models
{
    class Route : IConstructor
    {
        public string Route_id { get; set; } 
        public int Agency_id { get; set; } 
        public string Route_short_name { get; set; } 
        public string Route_long_name { get; set; } 
        public Type Route_type { get; set; } 
        public string Route_url { get; set; } 
        public string Route_color { get; set; } 
        public string Route_text_color { get; set; } 
        public bool Is_night { get; set; } 
        public bool Is_regional { get; set; }
        public bool Is_substitute_transport { get; set; }

        public void Consturctor(string radek)
        {
            string[] hodnoty = radek.Split(',');
            Route_id = hodnoty[0];
            Agency_id = int.Parse(hodnoty[1]);
            Route_short_name = hodnoty[2];
            Route_long_name = hodnoty[3];
            Route_type = (Type)Enum.Parse(typeof(Type),hodnoty[4]);
            Route_url = hodnoty[5];
            Route_color = hodnoty[6];
            Route_text_color = hodnoty[7];
            Is_night = hodnoty[8].Equals("1");
            Is_regional = hodnoty[9].Equals("1");
            Is_substitute_transport = hodnoty[10].Equals("1");
        }

        public enum Type : ushort
        {
            Tram = 0,
            Metro = 1,
            Rail = 2,
            Bus = 3,
            Ferry = 4,
            CableTram = 5,
            CableCar = 6,
            Funicular = 7,
            Trolleybus = 11,
            Monorail = 12
        }
    }
}

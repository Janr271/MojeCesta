

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

        public void Consturctor(string[] radek)
        {
            Route_id = radek[0];
            Agency_id = int.Parse(radek[1]);
            Route_short_name = radek[2];
            Route_long_name = radek[3];
            Route_type = (Type)Enum.Parse(typeof(Type),radek[4]);
            Route_url = radek[5];
            Route_color = radek[6];
            Route_text_color = radek[7];
            Is_night = radek[8].Equals("1");
            Is_regional = radek[9].Equals("1");
            Is_substitute_transport = radek[10].Equals("1");
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

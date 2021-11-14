

namespace MojeCesta.Models
{
    class Route : IConstructor
    {
        public string Route_id { get; set; } 
        public int Agency_id { get; set; } 
        public string Route_short_name { get; set; } 
        public string Route_long_name { get; set; } 
        public int Route_type { get; set; } 
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
            Route_type = int.Parse(hodnoty[4]);
            Route_url = hodnoty[5];
            Route_color = hodnoty[6];
            Route_text_color = hodnoty[7];
            Is_night = bool.Parse(hodnoty[8]);
            Is_regional = bool.Parse(hodnoty[9]);
            Is_substitute_transport = bool.Parse(hodnoty[10]);
        }
    }
}

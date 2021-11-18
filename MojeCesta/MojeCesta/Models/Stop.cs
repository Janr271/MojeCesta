

using System.Globalization;

namespace MojeCesta.Models
{
    class Stop : IConstructor
    {
        public string Stop_id { get; set; } 
        public string Stop_name { get; set; }
        public double Stop_lat { get; set; } 
        public double Stop_lon { get; set; }
        public string Zone_id { get; set; }
        public string Stop_url { get; set; }
        public string Location_type { get; set; } 
        public string Parent_station { get; set; }
        public bool Wheelchair_boarding { get; set; }
        public string Level_id { get; set; }
        public string Platform_code { get; set; }
        public string Asw_node_id { get; set; }
        public string Asw_stop_id { get; set; }

        public void Consturctor(string radek)
        {
            string[] hodnoty = radek.Split(',');
            CultureInfo provider = CultureInfo.InvariantCulture;
            Stop_id = hodnoty[0];
            Stop_name = hodnoty[1];
            Stop_lat = double.Parse(hodnoty[2], provider);
            Stop_lon = double.Parse(hodnoty[3], provider);
            Zone_id = hodnoty[4];
            Stop_url = hodnoty[5];
            Location_type = hodnoty[6];
            Parent_station = hodnoty[7];
            Wheelchair_boarding = hodnoty[8].Equals("1");
            Level_id = hodnoty[9];
            Platform_code = hodnoty[10];
            Asw_node_id = hodnoty[11];
            Asw_stop_id = hodnoty[12];
        }
    }
}

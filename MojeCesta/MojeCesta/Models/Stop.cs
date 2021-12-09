﻿

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

        public void Consturctor(string[] radek)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            Stop_id = radek[0];
            Stop_name = radek[1];
            Stop_lat = double.Parse(radek[2], provider);
            Stop_lon = double.Parse(radek[3], provider);
            Zone_id = radek[4];
            Stop_url = radek[5];
            Location_type = radek[6];
            Parent_station = radek[7];
            Wheelchair_boarding = radek[8].Equals("1");
            Level_id = radek[9];
            Platform_code = radek[10];
            Asw_node_id = radek[11];
            if(radek.Length == 13)
            {
                Asw_stop_id = radek[12];
            }
        }
    }
}
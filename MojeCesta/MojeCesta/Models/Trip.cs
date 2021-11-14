
namespace MojeCesta.Models
{
    class Trip : IConstructor
    {
        public string Route_id { get; set; } 
        public string Service_id { get; set; } 
        public string Trip_id { get; set; }
        public string Trip_headsign { get; set; }
        public string Trip_short_name { get; set; }
        public string Direction_id { get; set; }
        public string Block_id { get; set; }
        public string Shape_id { get; set; }
        public bool Wheelchair_accessible { get; set; }
        public bool Bikes_allowed { get; set; }
        public bool Exceptional { get; set; }

        public void Consturctor(string radek)
        {
            string[] hodnoty = radek.Split(',');
            Route_id = hodnoty[0];
            Service_id = hodnoty[1];
            Trip_id = hodnoty[2];
            Trip_headsign = hodnoty[3];
            Trip_short_name = hodnoty[4];
            Direction_id = hodnoty[5];
            Block_id = hodnoty[6];
            Shape_id = hodnoty[7];
            Wheelchair_accessible = bool.Parse(hodnoty[8]);
            Bikes_allowed = bool.Parse(hodnoty[9]);
            Exceptional = bool.Parse(hodnoty[10]);
        }
    }
}

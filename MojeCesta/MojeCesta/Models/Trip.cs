
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

        public void Consturctor(string[] radek)
        {
            Route_id = radek[0];
            Service_id = radek[1];
            Trip_id = radek[2];
            Trip_headsign = radek[3];
            Trip_short_name = radek[4];
            Direction_id = radek[5];
            Block_id = radek[6];
            Shape_id = radek[7];
            Wheelchair_accessible = radek[8].Equals("1");
            Bikes_allowed = radek[9].Equals("1");
            Exceptional = radek[10].Equals("1");
        }
    }
}

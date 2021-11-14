using System;

namespace MojeCesta.Models
{
    class Stop_time : IConstructor
    {
        public string Trip_id { get; set; } 
        public DateTime Arrival_time { get; set; } 
        public DateTime Departure_time { get; set; }
        public string Stop_id { get; set; }
        public int Stop_sequence { get; set; }
        public string Stop_headsign { get; set; }
        public int Pickup_type { get; set; }
        public int Drop_off_type { get; set; }
        public double Shape_dist_traveled { get; set; }
        public int Trip_operation_type { get; set; }
        public bool Bikes_allowed { get; set; }

        public void Consturctor(string radek)
        {
            string[] hodnoty = radek.Split(',');
            Trip_id = hodnoty[0];
            Arrival_time = DateTime.Parse(hodnoty[1]);
            Departure_time = DateTime.Parse(hodnoty[2]);
            Stop_id = hodnoty[3];
            Stop_sequence = int.Parse(hodnoty[4]);
            Stop_headsign = hodnoty[5];
            Pickup_type = int.Parse(hodnoty[6]);
            Drop_off_type = int.Parse(hodnoty[7]);
            Shape_dist_traveled = double.Parse(hodnoty[8]);
            Trip_operation_type = int.Parse(hodnoty[9]);
            Bikes_allowed = bool.Parse(hodnoty[10]);
        }
    }
}

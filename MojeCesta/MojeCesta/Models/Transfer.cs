using System;

namespace MojeCesta.Models
{
    class Transfer : IConstructor
    {
        public string From_stop_id { get; set; } 
        public string To_stop_id { get; set; } 
        public int Transfer_type { get; set; } 
        public TimeSpan Min_transfer_time { get; set; } 
        public string From_trip_id { get; set; }
        public string To_trip_id { get; set; }

        public void Consturctor(string radek)
        {
            string[] hodnoty = radek.Split(',');
            From_stop_id = hodnoty[0];
            To_stop_id = hodnoty[1];
            Transfer_type = int.Parse(hodnoty[2]);
            Min_transfer_time = TimeSpan.Parse(hodnoty[3]);
            From_trip_id = hodnoty[4];
            To_trip_id = hodnoty[5];
        }
    }
}

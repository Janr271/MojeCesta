using System;

namespace MojeCesta.Models
{
    class Transfer : IConstructor
    {
        public string From_stop_id { get; set; } 
        public string To_stop_id { get; set; } 
        public Type Transfer_type { get; set; } 
        public TimeSpan Min_transfer_time { get; set; } 
        public string From_trip_id { get; set; }
        public string To_trip_id { get; set; }

        public void Consturctor(string radek)
        {
            string[] hodnoty = radek.Split(',');
            From_stop_id = hodnoty[0];
            To_stop_id = hodnoty[1];
            Transfer_type = (Type)Enum.Parse(typeof(Type),hodnoty[2]);
            Min_transfer_time = new TimeSpan(int.Parse(hodnoty[3])*10);
            From_trip_id = hodnoty[4];
            To_trip_id = hodnoty[5];
        }

        public enum Type
        {
            Recommended = 0,
            TimedTransfer = 1,
            TimeRequired = 2,
            NotPossible = 3
        }
    }
}

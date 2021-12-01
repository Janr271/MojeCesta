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

        public void Consturctor(string[] radek)
        {
            From_stop_id = radek[0];
            To_stop_id = radek[1];
            Transfer_type = (Type)Enum.Parse(typeof(Type),radek[2]);
            if(radek[3] != "")
            {
                Min_transfer_time = new TimeSpan(int.Parse(radek[3]) * 10);
            }
            From_trip_id = radek[4];
            if(radek.Length == 6)
            {
                To_trip_id = radek[5];
            }
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

using System;
using System.Globalization;

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
        public Pickup Pickup_type { get; set; }
        public Drop Drop_off_type { get; set; }
        public double Shape_dist_traveled { get; set; }
        public Operation Trip_operation_type { get; set; }
        public Bike Bikes_allowed { get; set; }

        public void Consturctor(string radek)
        {
            string[] hodnoty = radek.Split(',');
            string format = "HH:mm:ss";
            CultureInfo provider = CultureInfo.InvariantCulture;
            Trip_id = hodnoty[0];
            Arrival_time = DateTime.ParseExact(hodnoty[1], format, provider);
            Departure_time = DateTime.ParseExact(hodnoty[2], format, provider);
            Stop_id = hodnoty[3];
            Stop_sequence = int.Parse(hodnoty[4]);
            Stop_headsign = hodnoty[5];
            Pickup_type = (Pickup)Enum.Parse(typeof(Pickup), hodnoty[6]);
            Drop_off_type = (Drop)Enum.Parse(typeof(Pickup), hodnoty[7]);
            Shape_dist_traveled = double.Parse(hodnoty[8], provider);
            Trip_operation_type = (Operation)Enum.Parse(typeof(Operation), hodnoty[9]);
            Bikes_allowed = (Bike)Enum.Parse(typeof(Bike), hodnoty[10]);
        }
        public enum Pickup : ushort
        {
            Regular = 0,
            NoPickup = 1,
            PhoneAgency = 2,
            CoordinateDriver = 3
        }
        public enum Drop : ushort
        {
            Regular = 0,
            NoDrop = 1,
            PhoneAgency = 2,
            CoordinateDriver = 3
        }
        public enum Operation : ushort
        {
            Regular = 1,
            IrregularStart = 7,
            IrregularEnd = 8,
            IrregularPath = 9,
            ChangeLine = 10
        }

        public enum Bike : ushort
        {
            NoInfo = 0,
            Allowed = 1,
            Prohibited = 2,
            CantGetInOut = 3,
            CantGetOut = 4,
            CantGetIn = 5
        }
    }
}

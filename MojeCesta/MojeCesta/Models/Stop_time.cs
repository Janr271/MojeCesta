using System;
using FileHelpers;

namespace MojeCesta.Models
{
    [DelimitedRecord(",")]
    class Stop_time
    {
        public string Trip_id { get; set; }
        [FieldConverter(typeof(TimeConverter))]
        public TimeSpan Arrival_time { get; set; }
        [FieldConverter(typeof(TimeConverter))]
        public TimeSpan Departure_time { get; set; }
        public string Stop_id { get; set; }
        public int Stop_sequence { get; set; }
        [FieldQuoted]
        public string Stop_headsign { get; set; }
        public Pickup? Pickup_type { get; set; }
        public Drop? Drop_off_type { get; set; }
        public double? Shape_dist_traveled { get; set; }
        public Operation? Trip_operation_type { get; set; }
        public Bike? Bikes_allowed { get; set; }

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

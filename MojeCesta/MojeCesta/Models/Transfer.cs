using SQLite;
using FileHelpers;

namespace MojeCesta.Models
{
    [DelimitedRecord(",")]
    class Transfer
    {
        [PrimaryKey]
        public string From_stop_id { get; set; } 
        public string To_stop_id { get; set; } 
        public Type? Transfer_type { get; set; } 
        public int? Min_transfer_time { get; set; } 
        public string From_trip_id { get; set; }
        public string To_trip_id { get; set; }

        public enum Type
        {
            Recommended = 0,
            TimedTransfer = 1,
            TimeRequired = 2,
            NotPossible = 3
        }
    }
}

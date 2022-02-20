using SQLite;
using FileHelpers;

namespace MojeCesta.Models
{
    [DelimitedRecord(",")]
    public class Trip
    {
        public string Route_id { get; set; } 
        public string Service_id { get; set; }
        [PrimaryKey]
        public string Trip_id { get; set; }
        [FieldQuoted]
        public string Trip_headsign { get; set; }
        public string Trip_short_name { get; set; }
        public Direction? Direction_id { get; set; }
        public string Block_id { get; set; }
        public string Shape_id { get; set; }
        public Type? Wheelchair_accessible { get; set; }
        public Type? Bikes_allowed { get; set; }
        public Type? Exceptional { get; set; }

        public enum Type
        {
            Unknown = 0,
            Allowed = 1,
            Prohibited = 2
        }

        public enum Direction
        {
            OneDirection = 0,
            OppositeDirection = 1
        }
    }
}

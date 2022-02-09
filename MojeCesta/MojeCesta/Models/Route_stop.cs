using SQLite;
using FileHelpers;

namespace MojeCesta.Models
{
    [DelimitedRecord(",")]
    class Route_stop
    {
        public string Route_id { get; set; } 
        public Direction Direction_id { get; set; }
        public string Stop_id { get; set; }
        public int Stop_sequence { get; set; }

        public enum Direction
        {
            OneDirection = 0,
            OppositeDirection = 1
        }
    }
}

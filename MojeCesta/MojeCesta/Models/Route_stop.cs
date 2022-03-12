using SQLite;
using FileHelpers;

namespace MojeCesta.Models
{
    [DelimitedRecord(",")]
    public class Route_stop
    {
        public string Route_id { get; set; } 
        public Direction Direction_id { get; set; }
        public string Stop_id { get; set; }
        public int Stop_sequence { get; set; }

        public Route_stop()
        {

        }

        public Route_stop(string route_id, Direction direction_id, string stop_id, int stop_sequence)
        {
            Route_id = route_id;
            Direction_id = direction_id;
            Stop_id = stop_id;
            Stop_sequence = stop_sequence;
        }

        public enum Direction
        {
            OneDirection = 0,
            OppositeDirection = 1
        }
    }
}

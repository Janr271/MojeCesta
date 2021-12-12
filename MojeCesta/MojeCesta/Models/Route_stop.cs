using SQLite;
using FileHelpers;

namespace MojeCesta.Models
{
    [DelimitedRecord(",")]
    class Route_stop
    {
        public string Route_id { get; set; } 
        public int Direction_id { get; set; }
        [PrimaryKey]
        public string Stop_id { get; set; }
        public int Stop_sequence { get; set; }
    }
}

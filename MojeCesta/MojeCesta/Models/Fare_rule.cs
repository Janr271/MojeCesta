using SQLite;
using FileHelpers;

namespace MojeCesta.Models
{
    [DelimitedRecord(",")]
    class Fare_rule
    {
        public string Fare_id { get; set; }
        public string Contains_id { get; set; }
        public string Route_id { get; set; }
    }
}

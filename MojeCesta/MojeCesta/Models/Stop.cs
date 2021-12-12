using SQLite;
using FileHelpers;

namespace MojeCesta.Models
{
    [DelimitedRecord(",")]
    class Stop
    {
        [PrimaryKey]
        public string Stop_id { get; set; } 
        public string Stop_name { get; set; }
        public double Stop_lat { get; set; } 
        public double Stop_lon { get; set; }
        public string Zone_id { get; set; }
        public string Stop_url { get; set; }
        public string Location_type { get; set; } 
        public string Parent_station { get; set; }
        public bool? Wheelchair_boarding { get; set; }
        public string Level_id { get; set; }
        public string Platform_code { get; set; }
        public string Asw_node_id { get; set; }
        public string Asw_stop_id { get; set; }
    }
}

using SQLite;
using FileHelpers;
using System.Collections.Generic;

namespace MojeCesta.Models
{
    [DelimitedRecord(",")]
    public class Stop
    {
        [PrimaryKey]
        public string Stop_id { get; set; } 
        [FieldQuoted]
        public string Stop_name { get; set; }
        public double Stop_lat { get; set; } 
        public double Stop_lon { get; set; }
        [FieldQuoted]
        public string Zone_id { get; set; }
        [FieldQuoted]
        public string Stop_url { get; set; }
        public LocationType? Location_type { get; set; } 
        public string Parent_station { get; set; }
        public Type? Wheelchair_boarding { get; set; }
        public string Level_id { get; set; }
        public string Platform_code { get; set; }
        public string Asw_node_id { get; set; }
        public string Asw_stop_id { get; set; }

        public Stop()
        {

        }

        public Stop(Stop s)
        {
            Stop_id = s.Stop_id;
            Stop_name = s.Stop_name;
            Stop_lat = s.Stop_lat;
            Stop_lon = s.Stop_lon;
            Zone_id = s.Zone_id;
            Location_type = s.Location_type;
            Parent_station = s.Parent_station;
            Platform_code = s.Platform_code;
            Asw_node_id = s.Asw_node_id;
            Asw_stop_id = s.Asw_stop_id;
        }

        public enum LocationType
        {
            Stop = 0,
            Station = 1,
            EntranceExit = 2,
            GenericNode = 3,
            BoardingArea = 4
        }
        public enum Type
        {
            Unknown = 0,
            Allowed = 1,
            Prohibited = 2
        }

        public override string ToString()
        {
            return Stop_name;
        }
    }

    public class Zastavka : Stop
    {
        public Zastavka(Stop s) : base(s)
        {
            Linky = new List<int>();
        }

        public List<int> Linky;
    }
}

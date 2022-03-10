using SQLite;
using FileHelpers;

namespace MojeCesta.Models
{
    [DelimitedRecord(",")]
    public class Route
    {
        [PrimaryKey]
        public string Route_id { get; set; } 
        public string Agency_id { get; set; } 
        public string Route_short_name { get; set; } 
        [FieldQuoted]
        public string Route_long_name { get; set; } 
        public RouteType Route_type { get; set; } 
        [FieldQuoted]
        public string Route_url { get; set; } 
        public string Route_color { get; set; } 
        public string Route_text_color { get; set; } 
        public bool Is_night { get; set; } 
        public bool Is_regional { get; set; }
        public bool Is_substitute_transport { get; set; }

        public enum RouteType : ushort
        {
            Tram = 0,
            Metro = 1,
            Rail = 2,
            Bus = 3,
            Ferry = 4,
            CableTram = 5,
            CableCar = 6,
            Funicular = 7,
            Trolleybus = 11,
            Monorail = 12
        }
    }
}

using SQLite;
using FileHelpers;

namespace MojeCesta.Models
{
    [DelimitedRecord(",")]
    class Route_sub_agency
    {
        [PrimaryKey]
        public string Route_id { get; set; } 
        public int? Route_licence_number { get; set; } 
        public int Sub_agency_id { get; set; }
        [FieldQuoted]
        public string Sub_agency_name { get; set; }
    }
}

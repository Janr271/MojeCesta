using SQLite;
using FileHelpers;

namespace MojeCesta.Models
{
    [DelimitedRecord(",")]
    class Agency
    {
        [PrimaryKey]
        public string Agency_id { get; set; }
        [FieldQuoted]
        public string Agency_name { get; set; }
        [FieldQuoted]
        public string Agency_url { get; set; }
        public string Agency_timezone { get; set; }
        public string Agency_lang { get; set; }
        [FieldQuoted]
        public string Agency_phone { get; set; }
    }
}

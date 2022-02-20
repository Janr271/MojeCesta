using SQLite;
using FileHelpers;

namespace MojeCesta.Models
{
    [DelimitedRecord(",")]
    public class Level
    {
        [PrimaryKey]
        public string Level_id { get; set; } 
        public int Level_index { get; set; }
        [FieldQuoted]
        public string Level_name { get; set; }
    }
}

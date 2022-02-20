using SQLite;
using FileHelpers;

namespace MojeCesta.Models
{
    [DelimitedRecord(",")]
    public class Pathway
    {
        [PrimaryKey]
        public string Pathway_id { get; set; }
        public string From_stop_id { get; set; } 
        public string To_stop_id { get; set; } 
        public Mode Pathway_mode { get; set; }
        public bool Is_bidirectional { get; set; }
        public int? Traversal_time { get; set; } // čas v sekundách
        [FieldQuoted]
        public string Signposted_as { get; set; }
        [FieldQuoted]
        public string Reverse_signposted_as { get; set; }

        public enum Mode : ushort
        {
            Walkway = 1,
            Stairs = 2,
            Travelator = 3,
            Ëscalator = 4,
            Elevator = 5,
            FareGate = 6,
            ExitGate = 7
        }
    }
}

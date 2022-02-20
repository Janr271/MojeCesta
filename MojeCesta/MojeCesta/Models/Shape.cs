using FileHelpers;

namespace MojeCesta.Models
{
    [DelimitedRecord(",")]
    public class Shape
    {
        public string Shape_id { get; set; } 
        public double Shape_pt_lat { get; set; }
        public double Shape_pt_lon { get; set; }
        public int Shape_pt_sequence { get; set; }
        public double Shape_dist_traveled { get; set; }
    }
}

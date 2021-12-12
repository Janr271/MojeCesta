using SQLite;
using FileHelpers;
using System.Globalization;

namespace MojeCesta.Models
{
    [DelimitedRecord(",")]
    class Shape : IConstructor
    {
        [PrimaryKey]
        public string Shape_id { get; set; } 
        public double Shape_pt_lat { get; set; }
        public double Shape_pt_lon { get; set; }
        public int Shape_pt_sequence { get; set; }
        public double? Shape_dist_traveled { get; set; }

        public void Consturctor(string[] radek)
        {
            this.Shape_id = radek[0];
            this.Shape_pt_lat = double.Parse(radek[1], CultureInfo.InvariantCulture);
            this.Shape_pt_lon = double.Parse(radek[2], CultureInfo.InvariantCulture);
            this.Shape_pt_sequence = int.Parse(radek[3]);
            this.Shape_dist_traveled = double.Parse(radek[4], CultureInfo.InvariantCulture);
        }
    }
}

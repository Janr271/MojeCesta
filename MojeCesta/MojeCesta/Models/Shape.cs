

using System.Globalization;

namespace MojeCesta.Models
{
    class Shape : IConstructor
    {
        public string Shape_id { get; set; } 
        public double Shape_pt_lat { get; set; }
        public double Shape_pt_lon { get; set; }
        public int Shape_pt_sequence { get; set; }
        public double Shape_dist_traveled { get; set; }

        public void Consturctor(string[] radek)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            Shape_id = radek[0];
            Shape_pt_lat = double.Parse(radek[1], provider);
            Shape_pt_lon = double.Parse(radek[2], provider);
            Shape_pt_sequence = int.Parse(radek[3]);
            Shape_dist_traveled = double.Parse(radek[4], provider);
        }
    }
}

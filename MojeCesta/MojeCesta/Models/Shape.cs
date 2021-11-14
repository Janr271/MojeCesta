

namespace MojeCesta.Models
{
    class Shape : IConstructor
    {
        public string Shape_id { get; set; } 
        public double Shape_pt_lat { get; set; }
        public double Shape_pt_lon { get; set; }
        public int Shape_pt_sequence { get; set; }
        public double Shape_dist_traveled { get; set; }

        public void Consturctor(string radek)
        {
            string[] hodnoty = radek.Split(',');
            Shape_id = hodnoty[0];
            Shape_pt_lat = double.Parse(hodnoty[1]);
            Shape_pt_lon = double.Parse(hodnoty[2]);
            Shape_pt_sequence = int.Parse(hodnoty[3]);
            Shape_dist_traveled = double.Parse(hodnoty[4]);
        }
    }
}

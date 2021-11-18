

namespace MojeCesta.Models
{
    class Route_stop : IConstructor
    {
        public string Route_id { get; set; } 
        public int Direction_id { get; set; }
        public string Stop_id { get; set; }
        public int Stop_sequence { get; set; }

        public void Consturctor(string radek)
        {
            string[] hodnoty = radek.Split(',');
            Route_id = hodnoty[0];
            Direction_id = int.Parse(hodnoty[1]);
            Stop_id = hodnoty[2];
            Stop_sequence = int.Parse(hodnoty[3]);
        }
    }
}

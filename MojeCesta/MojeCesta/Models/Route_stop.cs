

namespace MojeCesta.Models
{
    class Route_stop : IConstructor
    {
        public string Route_id { get; set; } 
        public int Direction_id { get; set; }
        public string Stop_id { get; set; }
        public int Stop_sequence { get; set; }

        public void Consturctor(string[] radek)
        {
            Route_id = radek[0];
            Direction_id = int.Parse(radek[1]);
            Stop_id = radek[2];
            Stop_sequence = int.Parse(radek[3]);
        }
    }
}

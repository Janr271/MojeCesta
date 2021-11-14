using System;

namespace MojeCesta.Models
{
    class Calendar : IConstructor
    {
        public int Service_id { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }

        public void Consturctor(string radek)
        {
            string[] hodnoty = radek.Split(',');
            Service_id = int.Parse(hodnoty[0]);
            Monday = bool.Parse(hodnoty[1]);
            Tuesday = bool.Parse(hodnoty[2]);
            Wednesday = bool.Parse(hodnoty[3]);
            Thursday = bool.Parse(hodnoty[4]);
            Friday = bool.Parse(hodnoty[5]);
            Saturday = bool.Parse(hodnoty[6]);
            Sunday = bool.Parse(hodnoty[7]);
            Start_date = DateTime.Parse(hodnoty[8]);
            End_date = DateTime.Parse(hodnoty[9]);
        }
    }
}

using System;
using System.Globalization;

namespace MojeCesta.Models
{
    class Calendar : IConstructor
    {
        public string Service_id { get; set; }
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
            string format = "yyyyMMdd";
            CultureInfo provider = CultureInfo.InvariantCulture;
            string[] hodnoty = radek.Split(',');
            Service_id = hodnoty[0];
            Monday = bool.Parse(hodnoty[1]);
            Tuesday = bool.Parse(hodnoty[2]);
            Wednesday = bool.Parse(hodnoty[3]);
            Thursday = bool.Parse(hodnoty[4]);
            Friday = bool.Parse(hodnoty[5]);
            Saturday = bool.Parse(hodnoty[6]);
            Sunday = bool.Parse(hodnoty[7]);
            Start_date = DateTime.ParseExact(hodnoty[8],format,provider);
            End_date = DateTime.ParseExact(hodnoty[9], format, provider);
        }
    }
}

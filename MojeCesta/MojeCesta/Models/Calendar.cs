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

        public void Consturctor(string[] radek)
        {
            string format = "yyyyMMdd";
            CultureInfo provider = CultureInfo.InvariantCulture;
            Service_id = radek[0];
            Monday = radek[1].Equals("1");
            Tuesday = radek[2].Equals("1");
            Wednesday = radek[3].Equals("1");
            Thursday = radek[4].Equals("1");
            Friday = radek[5].Equals("1");
            Saturday = radek[6].Equals("1");
            Sunday = radek[7].Equals("1");
            Start_date = DateTime.ParseExact(radek[8],format,provider);
            End_date = DateTime.ParseExact(radek[9], format, provider);
        }
    }
}

using System;
using System.Globalization;

namespace MojeCesta.Models
{
    class Calendar_date : IConstructor
    {
        public string Service_id { get; set; }
        public DateTime Date { get; set; }
        public int Exception_type { get; set; }

        public void Consturctor(string[] radek)
        {
            string format = "yyyyMMdd";
            CultureInfo provider = CultureInfo.InvariantCulture;
            Service_id = radek[0];
            Date = DateTime.ParseExact(radek[1], format, provider);
            Exception_type = int.Parse(radek[2]);
        }
    }
}

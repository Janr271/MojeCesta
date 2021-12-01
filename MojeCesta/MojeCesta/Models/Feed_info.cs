using System;
using System.Globalization;

namespace MojeCesta.Models
{
    class Feed_info : IConstructor
    {
        public string Feed_publisher_name { get; set; } 
        public string Feed_publisher_url { get; set; } 
        public string Feed_lang { get; set; } 
        public DateTime Feed_start_date { get; set; } 
        public DateTime Feed_end_date { get; set; }
        public string Feed_contact_email { get; set; }

        public void Consturctor(string[] radek)
        {
            string format = "yyyyMMdd";
            CultureInfo provider = CultureInfo.InvariantCulture;
            Feed_publisher_name = radek[0];
            Feed_publisher_url = radek[1];
            Feed_lang = radek[2];
            Feed_start_date = DateTime.ParseExact(radek[3],format,provider);
            Feed_end_date = DateTime.ParseExact(radek[4],format,provider);
            Feed_contact_email = radek[5];
        }
    }
}

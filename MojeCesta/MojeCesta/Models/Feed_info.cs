using System;

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

        public void Consturctor(string radek)
        {
            string[] hodnoty = radek.Split(',');
            Feed_publisher_name = hodnoty[0];
            Feed_publisher_url = hodnoty[1];
            Feed_lang = hodnoty[2];
            Feed_start_date = DateTime.Parse(hodnoty[3]);
            Feed_end_date = DateTime.Parse(hodnoty[4]);
            Feed_contact_email = hodnoty[5];
        }
    }
}

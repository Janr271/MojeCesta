
namespace MojeCesta.Models
{
    class Agency : IConstructor
    {
        public int Agency_id { get; set; }
        public string Agency_name { get; set; }
        public string Agency_url { get; set; }
        public string Agency_timezone { get; set; }
        public string Agency_lang { get; set; }
        public string Agency_phone { get; set; }

        public void Consturctor(string radek)
        {
            string[] hodnoty = radek.Split(',');
            Agency_id = int.Parse(hodnoty[0]);
            Agency_name = hodnoty[1];
            Agency_url = hodnoty[2];
            Agency_timezone = hodnoty[3];
            Agency_lang = hodnoty[4];
            Agency_phone = hodnoty[5];
        }
    }
}

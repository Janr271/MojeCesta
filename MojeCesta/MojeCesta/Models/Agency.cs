using SQLite;

namespace MojeCesta.Models
{
    class Agency : IConstructor
    {
        [PrimaryKey]
        public int Agency_id { get; set; }
        public string Agency_name { get; set; }
        public string Agency_url { get; set; }
        public string Agency_timezone { get; set; }
        public string Agency_lang { get; set; }
        public string Agency_phone { get; set; }

        public void Consturctor(string[] radek)
        {
            Agency_id = int.Parse(radek[0]);
            Agency_name = radek[1];
            Agency_url = radek[2];
            Agency_timezone = radek[3];
            Agency_lang = radek[4];
            Agency_phone = radek[5];
        }
    }
}

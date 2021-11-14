

namespace MojeCesta.Models
{
    class Calendar_date : IConstructor
    {
        public string Service_id { get; set; }
        public string Date { get; set; }
        public int Exception_type { get; set; }

        public void Consturctor(string radek)
        {
            string[] hodnoty = radek.Split(',');
            Service_id = hodnoty[0];
            Date = hodnoty[1];
            Exception_type = int.Parse(hodnoty[2]);
        }
    }
}

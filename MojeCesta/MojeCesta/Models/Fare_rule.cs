

namespace MojeCesta.Models
{
    class Fare_rule: IConstructor
    {
        public string Fare_id { get; set; }
        public string Contains_id { get; set; }
        public string Route_id { get; set; }

        public void Consturctor(string[] radek)
        {
            Fare_id = radek[0];
            Contains_id = radek[1];
            if(radek.Length == 3)
            {
                Route_id = radek[2];
            }
        }
    }
}

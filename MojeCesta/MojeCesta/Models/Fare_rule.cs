

namespace MojeCesta.Models
{
    class Fare_rule: IConstructor
    {
        public string Fare_id { get; set; }
        public string Contains_id { get; set; }
        public string Route_id { get; set; }

        public void Consturctor(string radek)
        {
            string[] hodnoty = radek.Split(',');
            Fare_id = hodnoty[0];
            Contains_id = hodnoty[1];
            Route_id = hodnoty[2];
        }
    }
}

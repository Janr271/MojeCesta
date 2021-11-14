using System;

namespace MojeCesta.Models
{
    class Fare_attribute: IConstructor
    {
        public string Fare_id { get; set; }
        public float Price { get; set; }
        public string Currency_type { get; set; }
        public bool Payment_method { get; set; }
        public int Transfers { get; set; }
        public TimeSpan Transfer_duration { get; set; }

        public void Consturctor(string radek)
        {
            string[] hodnoty = radek.Split(',');
            Fare_id = hodnoty[0];
            Price = float.Parse(hodnoty[1]);
            Currency_type = hodnoty[2];
            Payment_method = bool.Parse(hodnoty[3]);
            Transfers = int.Parse(hodnoty[4]);
            Transfer_duration = TimeSpan.Parse(hodnoty[5]);
        }
    }
}

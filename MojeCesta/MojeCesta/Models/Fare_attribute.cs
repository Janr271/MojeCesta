using System;

namespace MojeCesta.Models
{
    class Fare_attribute: IConstructor
    {
        public string Fare_id { get; set; }
        public float Price { get; set; }
        public string Currency_type { get; set; }
        public Payment Payment_method { get; set; }
        public Transfer Transfers { get; set; }
        public TimeSpan Transfer_duration { get; set; }

        public void Consturctor(string radek)
        {
            string[] hodnoty = radek.Split(',');
            Fare_id = hodnoty[0];
            Price = float.Parse(hodnoty[1]);
            Currency_type = hodnoty[2];
            Payment_method = (Payment)Enum.Parse(typeof(Payment), hodnoty[3]);
            Transfers = hodnoty[4] == "" ? Transfer.Unlimited : (Transfer)Enum.Parse(typeof(Transfer),hodnoty[4]);
            Transfer_duration = new TimeSpan(int.Parse(hodnoty[5])*10);
        }

        public enum Payment : ushort
        {
            OnBoard = 0,
            BeforeBoarding = 1
        }

        public enum Transfer : ushort
        {
            No = 0,
            Once = 1,
            Twice = 2,
            Unlimited = 3
        }
    }
}

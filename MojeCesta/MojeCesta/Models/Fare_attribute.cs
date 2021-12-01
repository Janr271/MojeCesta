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

        public void Consturctor(string[] radek)
        {
            Fare_id = radek[0];
            Price = float.Parse(radek[1]);
            Currency_type = radek[2];
            Payment_method = (Payment)Enum.Parse(typeof(Payment), radek[3]);
            Transfers = radek[4] == "" ? Transfer.Unlimited : (Transfer)Enum.Parse(typeof(Transfer),radek[4]);
            if(radek.Length != 6)
            {
                Transfer_duration = new TimeSpan(0);
            }
            else
            {
                Transfer_duration = new TimeSpan(int.Parse(radek[5]) * 10);
            }
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

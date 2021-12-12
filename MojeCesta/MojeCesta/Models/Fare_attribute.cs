using SQLite;
using FileHelpers;

namespace MojeCesta.Models
{
    [DelimitedRecord(",")]
    class Fare_attribute
    {
        [PrimaryKey]
        public string Fare_id { get; set; }
        public float Price { get; set; }
        public string Currency_type { get; set; }
        public Payment Payment_method { get; set; }
        public Transfer? Transfers { get; set; }
        public int? Transfer_duration { get; set; }

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

using System;
using SQLite;
using FileHelpers;

namespace MojeCesta.Models
{
    [DelimitedRecord(",")]
    class Calendar_date
    {
        public string Service_id { get; set; }
        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        public DateTime Date { get; set; }
        public ExceptionType Exception_type { get; set; }

        public enum ExceptionType
        {
            Added = 1,
            Removed = 2
        }
    }
}

using System;
using SQLite;
using FileHelpers;

namespace MojeCesta.Models
{
    [DelimitedRecord(",")]
    class Calendar_date
    {
        [PrimaryKey]
        public string Service_id { get; set; }
        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        public DateTime Date { get; set; }
        public int Exception_type { get; set; }
    }
}

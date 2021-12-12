using System;
using SQLite;
using FileHelpers;

namespace MojeCesta.Models
{
    [DelimitedRecord(",")]
    class Calendar
    {
        [PrimaryKey]
        public string Service_id { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        public DateTime Start_date { get; set; }
        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        public DateTime End_date { get; set; }

    }
}

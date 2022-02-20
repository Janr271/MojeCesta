using System;
using FileHelpers;

namespace MojeCesta.Models
{
    [DelimitedRecord(",")]
    public class Feed_info
    {
        [FieldQuoted]
        public string Feed_publisher_name { get; set; } 
        [FieldQuoted]
        public string Feed_publisher_url { get; set; } 
        public string Feed_lang { get; set; }
        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        public DateTime Feed_start_date { get; set; }
        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        public DateTime Feed_end_date { get; set; }
        [FieldQuoted]
        public string Feed_contact_email { get; set; }
    }
}

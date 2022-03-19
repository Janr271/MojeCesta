using SQLite;
using System;

namespace MojeCesta.Models
{
    public class HistorieOdjezdu
    {
        public HistorieOdjezdu()
        {

        }
        public HistorieOdjezdu(string zeZastavkyId, string zeZastavky, DateTime datum, TimeSpan cas)
        {
            ZeZastavkyId = zeZastavkyId;
            ZeZastavky = zeZastavky;
            Datum = datum;
            Cas = cas;
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string ZeZastavkyId { get; set; }
        public string ZeZastavky { get; set; }
        public DateTime Datum { get; set; }
        public TimeSpan Cas { get; set; }
    }
}

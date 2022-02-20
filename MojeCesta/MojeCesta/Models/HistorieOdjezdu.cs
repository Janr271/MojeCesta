using System;

namespace MojeCesta.Models
{
    public class HistorieOdjezdu
    {
        public HistorieOdjezdu()
        {

        }
        public HistorieOdjezdu(string zeZastavky, DateTime datum, TimeSpan cas)
        {
            ZeZastavky = zeZastavky;
            Datum = datum;
            Cas = cas;
        }

        public string ZeZastavky { get; set; }
        public DateTime Datum { get; set; }
        public TimeSpan Cas { get; set; }
    }
}

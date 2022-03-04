﻿using System;

namespace MojeCesta.Models
{
    public class HistorieSpojeni
    {
        public HistorieSpojeni()
        {

        }

        public HistorieSpojeni(string zeZastavky, string naZastavku, int pocetPrestupu, DateTime datum, TimeSpan cas)
        {
            ZeZastavky = zeZastavky;
            NaZastavku = naZastavku;
            PocetPrestupu = pocetPrestupu;
            Datum = datum;
            Cas = cas;
        }

        public string ZeZastavky { get; set; }
        public string NaZastavku { get; set; }
        public int PocetPrestupu { get; set; }
        public DateTime Datum { get; set; }
        public TimeSpan Cas { get; set; }
    }
}
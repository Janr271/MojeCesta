using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MojeCesta.Models
{
    public class OdjezdyZeStanice : List<Odjezd>
    {
        public OdjezdyZeStanice(string nazevStanice)
        {
            NazevStanice = nazevStanice;
        }
        public string NazevStanice { get; set; }
        public List<Odjezd> Odjezdy => this;
    }

    public class Odjezd
    {
        public Odjezd(string linka, string smer, string cas)
        {
            Linka = linka;
            Smer = smer;
            Cas = cas;
        }
        public string Linka { get; set; }
        public string Smer { get; set; }
        public string Cas { get; set; }
    }
}

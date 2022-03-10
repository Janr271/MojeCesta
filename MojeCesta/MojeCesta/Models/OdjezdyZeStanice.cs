using System.Collections.Generic;

namespace MojeCesta.Models
{
    public class OdjezdyZeStanice : List<Odjezd>
    {
        public string NazevStanice { get; set; }
        public OdjezdyZeStanice(string nazevStanice, List<Odjezd> odjezdy) : base(odjezdy)
        {
            NazevStanice = nazevStanice;
        }
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

using System;
using System.Collections.Generic;
using System.Text;

namespace MojeCesta.Models
{
    public class SpojeniMeziStanicemi : List<Spojeni>
    {
        public SpojeniMeziStanicemi(string doOdjezdu, string metrika)
        {
            DoOdjezdu = doOdjezdu;
            Metrika = metrika;
        }
        public string DoOdjezdu { get; set; }
        public string Metrika { get; set; }
    }

    public class Spojeni
    {
        public Spojeni(string linka, string smer, string zeZast, string naZast, string odjezd, string prijezd, string prestup)
        {
            Linka = linka;
            Smer = smer;
            ZeZastavky = zeZast;
            NaZastavku = naZast;
            Odjezd = odjezd;
            Prijezd = prijezd;
            Prestup = prestup;
        }
        public string Linka { get; set; }
        public string Smer { get; set; }
        public string ZeZastavky { get; set; }
        public string NaZastavku { get; set; }
        public string Odjezd { get; set; }
        public string Prijezd { get; set; }
        public string Prestup { get; set; }
    }
}

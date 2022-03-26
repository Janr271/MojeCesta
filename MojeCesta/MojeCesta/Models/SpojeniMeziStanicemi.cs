using System;
using System.Collections.Generic;

namespace MojeCesta.Models
{
    public class SpojeniMeziStanicemi : List<Spojeni>, IComparable
    {
        public SpojeniMeziStanicemi()
        {
        }

        public string DoOdjezdu 
        { get 
            {
                TimeSpan doOdjezdu = Odjezd.Subtract(DateTime.Now);

                return MaReseni ? $"{(doOdjezdu > TimeSpan.Zero ? "za" : "před")} " +
                       $"{(doOdjezdu.Days != 0 ? $"{Math.Abs(doOdjezdu.Days)} {(doOdjezdu.Days > 4 ? "dní" : ((doOdjezdu.Days > 1 || doOdjezdu.Days < -1) ? "dny" : (doOdjezdu.Days > -1 ? "den" : "dnem")))} " : "")}" +
                       $"{(doOdjezdu.Hours != 0 ? $"{Math.Abs(doOdjezdu.Hours)} hod " : "")}" +
                       $"{Math.Abs(doOdjezdu.Minutes)} min"
                       : "";
            } 
        }

        public double Vzdalenost { get; set; }

        public TimeSpan Cas { get; set; }

        public string Metrika { get => MaReseni ? $"{(int)Cas.TotalMinutes} min, {(int)Vzdalenost} km" : ""; }

        public DateTime Odjezd { get; set; }

        public bool MaReseni = true;

        public int CompareTo(object obj)
        {
            if (obj is SpojeniMeziStanicemi b)
            {
                int dst = Vzdalenost.CompareTo(b.Vzdalenost);
                if(dst == 0)
                {
                    return Odjezd.CompareTo(b.Odjezd);
                }
                return dst;
            }
            else
            {
               throw new ArgumentException("Chybný typ!");
            }
        }
    }

    public class Spojeni
    {
        public Spojeni(string linka, string smer, string zeZast, string naZast, TimeSpan odjezd, TimeSpan prijezd)
        {
            Linka = linka;
            Smer = smer;
            ZeZastavky = zeZast;
            NaZastavku = naZast;
            this.CasOdjezdu = odjezd;
            this.CasPrijezdu = prijezd;
        }

        // Nepřijímá žádné parametry pokud neexistuje řešení
        public Spojeni()
        {
            Smer = "Nebylo nalezeno žádné spojení";
        }

        public TimeSpan CasOdjezdu { get; private set; }
        public TimeSpan CasPrijezdu { get; private set; }
        public string Linka { get; set; }
        public string Smer { get; set; }
        public string ZeZastavky { get; set; }
        public string NaZastavku { get; set; }
        public string Odjezd { get => CasOdjezdu != default ? CasOdjezdu.ToString(@"hh\:mm") : ""; }
        public string Prijezd { get => CasPrijezdu != default ? CasPrijezdu.ToString(@"hh\:mm") : ""; }
        public string Prestup { get; set; }
    }
}

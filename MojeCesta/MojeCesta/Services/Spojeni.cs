using System;
using System.Collections.Generic;
using MojeCesta.Models;

namespace MojeCesta.Services
{
    static class Spojeni
    {
        public static List<Tuple<Stop_time, Stop_time>[]> NajitSpojeni(Stop zastavkaOd, Stop zastavkaDo, DateTime cas, bool casOdjezdu)
        {
            List<Tuple<Stop_time, Stop_time>[]> nalezeneSpojeni = new List<Tuple<Stop_time, Stop_time>[]>();
            if (casOdjezdu)
            {
                Stop_time[] odjezdy  =  Database.NajitOdjezdy(zastavkaOd, cas).Result;
                foreach(Stop_time odjezd in odjezdy)
                {
                    Stop_time cilovaZastavka = Database.NajitDalsiZastavku(odjezd.Trip_id, zastavkaDo.Stop_id, odjezd.Stop_sequence).Result;
                    if(cilovaZastavka != null)
                    {
                        nalezeneSpojeni.Add(new Tuple<Stop_time,Stop_time>[] { new Tuple<Stop_time,Stop_time>(odjezd, cilovaZastavka)});
                    }
                }
            }
            return nalezeneSpojeni;
        }
    }
}

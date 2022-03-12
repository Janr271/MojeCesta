using System.Collections.Generic;

namespace MojeCesta.Models
{
    public class Cache
    {
        public Dictionary<string, int> Zastavky = new Dictionary<string, int>();
        public List<Zastavka> SeznamZastavek = new List<Zastavka>();
        public List<Linka> SeznamLinek = new List<Linka>();

        public Cache()
        {

        }

        public Cache(Dictionary<string, int> zastavky, List<Zastavka> seznamZast, List<Linka> seznamLin)
        {
            Zastavky = zastavky;
            SeznamZastavek = seznamZast;
            SeznamLinek = seznamLin;
        }
    }
}

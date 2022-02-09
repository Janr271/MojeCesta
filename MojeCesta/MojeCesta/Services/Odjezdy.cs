using System;
using System.Collections.Generic;

namespace MojeCesta.Services
{
    static class Odjezdy
    {
        public static List<Models.Stop_time> NajitOdjezdy(Models.Stop zastavka, TimeSpan cas)
        {
            return new List<Models.Stop_time>(Database.NajitOdjezdy(zastavka, cas).Result);
        }
    }
}

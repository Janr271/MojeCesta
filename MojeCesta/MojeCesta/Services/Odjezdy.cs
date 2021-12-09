using System;

namespace MojeCesta.Services
{
    static class Odjezdy
    {
        public static void NajitOdjezdy(Models.Stop zastavka, DateTime cas)
        {
            Models.Stop_time[] odjezdy = Database.NajitOdjezdy(zastavka, cas).Result;
        }
    }
}

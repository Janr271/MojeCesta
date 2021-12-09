using System;
using System.Net;
using System.Threading.Tasks;

namespace MojeCesta.Services
{
    public static class AktualizaceDat
    {
        public static bool PouzeWifi { get => pouzeWifi; set => pouzeWifi = value; }
        private static bool pouzeWifi = true;

        public static bool AutomatickaAktualizace { get => automatickaAktualizace; set => automatickaAktualizace = value; }
        private static bool automatickaAktualizace = true;

        public static TimeSpan Frekvence { get => frekvence; set => frekvence = value; }
        private static TimeSpan frekvence = new TimeSpan( 7, 0, 0, 0);

        public static DateTime PosledniAktualizace { get => posledniAktualizace; set => posledniAktualizace = value; }
        private static DateTime posledniAktualizace;

       
        
        public static void Stahnout(string cestaKZipu)
        {
            using(WebClient client = new WebClient())
            {
                client.DownloadFile(@"http://data.pid.cz/PID_GTFS.zip", cestaKZipu);
            }
        }

        public static async Task ZkontrolovatAktualizace() // Metoda se volá po startu a ověřuje, zda není potřeba aktualizovat databázi
        {
            if (AutomatickaAktualizace || PosledniAktualizace == null)
            {
                if(PosledniAktualizace == null || DateTime.Now.Subtract(Frekvence) >= PosledniAktualizace) // Pokud poslední aktualizace neproběhla, nebo proběhla dávno
                {
                    await Database.Aktualizovat();
                    PosledniAktualizace = DateTime.Now;
                }
            }
        }
    }
}

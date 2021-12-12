using System;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MojeCesta.Services
{
    public static class AktualizaceDat
    {
        public static bool PouzeWifi
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("pouzeWifi"))
                {
                    return (bool)Application.Current.Properties["pouzeWifi"];
                }
                return true;
            }
            set
            {
                Application.Current.Properties["pouzeWifi"] = value;
            }
        }

        public static bool AutomatickaAktualizace
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("automatickaAktualizace"))
                {
                    return (bool)Application.Current.Properties["automatickaAktualizace"];
                }
                return true;
            }
            set
            {
                Application.Current.Properties["automatickaAktualizace"] = value;
            }
        }

        public static TimeSpan Frekvence
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("frekvence"))
                {
                    return (TimeSpan)Application.Current.Properties["frekvence"];
                }
                return new TimeSpan(7, 0, 0, 0);
            }
            set
            {
                Application.Current.Properties["frekvence"] = value;
            }
        }

        public static DateTime? PosledniAktualizace 
        {
            get 
            {
                if (Application.Current.Properties.ContainsKey("posledniAktualizace"))
                {
                    return (DateTime)Application.Current.Properties["posledniAktualizace"];
                }
                return null;
            } 
            set 
            { 
                Application.Current.Properties["posledniAktualizace"] = value; 
            } 
        }



        public static bool Stahnout(string cestaKZipu)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(@"http://data.pid.cz/PID_GTFS.zip"), cestaKZipu);
            }
            return true;
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

using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Linq;
using MojeCesta.Models;

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

        public static void Aktualizovat(bool manualne)
        {
            // Spustit aktualizaci pokud
            // 1. Uživatel si aktualizaci vyvolal ručně
            // 2. Aplikace ještě nemá žádnou databázi
            // 3. Je zapnuta automatická aktualizace a uběhla stanovená doba od poslední aktualizace 
            if (manualne || PosledniAktualizace == null || (AutomatickaAktualizace && DateTime.Now.Subtract(Frekvence) >= PosledniAktualizace))
            {
                // Uzamknout UI
                GlobalniPromenne.Uzamknout();

                if(Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    if (!PouzeWifi || Connectivity.ConnectionProfiles.Contains(ConnectionProfile.WiFi))
                    {
                        Database.Aktualizovat().Wait();
                        PosledniAktualizace = DateTime.Now;
                    }
                    else
                    {
                        GlobalniPromenne.Oznameni("Zařízení není připojeno k Wifi!");
                    }
                }
                else
                {
                    GlobalniPromenne.Oznameni("Zařízení není připojeno k internetu!");
                }
            }

            // Odemknout UI
            GlobalniPromenne.Odemknout();
        }
    }
}

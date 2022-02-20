using System;
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

        public static void Aktualizovat(bool okamzite)
        {
            ActivityIndicator aktivita = new ActivityIndicator { Color = Color.Blue };
            // Okamže aktualizovat databázi
            if (okamzite)
            {
                aktivita.IsRunning = true;
                aktivita.IsVisible = true;
                aktivita.InputTransparent = true;
                Database.Aktualizovat().Wait();
                PosledniAktualizace = DateTime.Now;
            }
            // Pokud poslední aktualizace neproběhla, nebo proběhla dávno a je zapnuta automatická aktualizace
            else if (PosledniAktualizace == null || (AutomatickaAktualizace && DateTime.Now.Subtract(Frekvence) >= PosledniAktualizace))
            {
                aktivita.IsRunning = true;
                aktivita.IsVisible = true;
                aktivita.InputTransparent = true;
                Database.Aktualizovat().Wait();
                PosledniAktualizace = DateTime.Now;
            }

            aktivita.IsRunning = false;
            aktivita.IsVisible = false;
            aktivita.InputTransparent = false;
        }
    }
}

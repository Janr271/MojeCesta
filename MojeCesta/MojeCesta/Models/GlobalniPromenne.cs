using Android.Content.Res;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MojeCesta.Models
{
    public static class GlobalniPromenne
    {
        public static List<OdjezdyZeStanice> VysledkyOdjezdu;
        public static List<SpojeniMeziStanicemi> VysledkySpojeni;
        public static bool Aktualizace { get; private set; }

        // Světlý režim true - tmavý false
        public static bool Rezim
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("rezim"))
                {
                    return (bool)Application.Current.Properties["rezim"];
                }
                return false;
            }
            set
            {
                Application.Current.Properties["rezim"] = value;
            }
        }

        public static void Uzamknout()
        {
            Device.BeginInvokeOnMainThread(() => Shell.Current.GoToAsync(nameof(Views.AktualizacePage)));
            Aktualizace = true;
        }

        public static void Odemknout()
        {
            Device.BeginInvokeOnMainThread(() => Shell.Current.GoToAsync(".."));
            Aktualizace = false;
        }

        public static async void Oznameni(string text)
        {
            await Shell.Current.CurrentPage.DisplayAlert("Chyba", text, "OK");
        }
    }
}

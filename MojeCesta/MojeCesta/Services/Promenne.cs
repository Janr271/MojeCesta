using System.Collections.Generic;

using Xamarin.Essentials;
using Xamarin.Forms;

using MojeCesta.Models;
using System;
using System.IO;
using System.Text;
using System.Globalization;

namespace MojeCesta.Services
{
    public static class Promenne
    {
        // Výsledky vyhledávání spojení a odjezdů
        // Musí se nacházet tady aby mohly být sdíleny mezi více okny
        public static List<OdjezdyZeStanice> VysledkyOdjezdu;
        public static List<SpojeniMeziStanicemi> VysledkySpojeni;

        // Cashe pro rychlejší vyhledávání spojení
        public static List<Stop> SeznamZastavek;
        public static List<Route> SeznamLinek;
        public static Dictionary<string, int> Zastavky;

        // Cesta k cache
        public static readonly string CestaSeznamZ = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "seznamz.json");
        public static readonly string CestaSeznamL = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "seznaml.json");
        public static readonly string CestaZastavky = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "zastavky.json");

        // Světlý režim false - tmavý true
        public static bool Rezim
        {
            get
            {
                return Preferences.Get(nameof(Rezim), false);
            }
            set
            {
                Preferences.Set(nameof(Rezim), value);

                AplikovatStyl();
            }
        }

        public static void AplikovatStyl()
        {
            if (Rezim)
            {
                App.Current.UserAppTheme = OSAppTheme.Dark;

            }
            else
            {
                App.Current.UserAppTheme = OSAppTheme.Light;
            }
        }

        public static string Normalizovat(string text)
        {
            string normalizovano = text.ToLower().Normalize(NormalizationForm.FormD);
            StringBuilder s = new StringBuilder(normalizovano.Length);

            for (int i = 0; i < normalizovano.Length; i++)
            {
                char c = normalizovano[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    s.Append(c);
                }
            }

            return s.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}

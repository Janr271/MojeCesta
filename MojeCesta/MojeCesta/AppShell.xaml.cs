using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Forms;
using Newtonsoft.Json;
using MojeCesta.Models;
using MojeCesta.Views;
using MojeCesta.Services;

namespace MojeCesta
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Inicializace cest pro správce oken
            Routing.RegisterRoute(nameof(SpojeniPage), typeof(SpojeniPage));
            Routing.RegisterRoute(nameof(OdjezdyPage), typeof(OdjezdyPage));
            Routing.RegisterRoute(nameof(NastaveniPage), typeof(NastaveniPage));
            Routing.RegisterRoute(nameof(VysledkySpojeniPage), typeof(VysledkySpojeniPage));
            Routing.RegisterRoute(nameof(VysledkyOdjezduPage), typeof(VysledkyOdjezduPage));
            Routing.RegisterRoute(nameof(HledaniPage), typeof(HledaniPage));
            Routing.RegisterRoute(nameof(AktualizacePage), typeof(AktualizacePage));

            Task.Run(() => ZkontrolovatAktualizace());
            Task.Run(() => NacistCache());
        }

        public async void ZkontrolovatAktualizace()
        {
            // Pokud ještě není načtena databáze, nebo pokud je databáze zastaralá
            if (AktualizaceDat.PosledniAktualizace == null || (AktualizaceDat.AutomatickaAktualizace && DateTime.Now.Subtract(AktualizaceDat.Frekvence) >= AktualizaceDat.PosledniAktualizace))
            {
                await Navigation.PushModalAsync(new AktualizacePage(true));
            }
        }
        public async void NacistCache()
        {
            // Pokud existuje Cache tak jí načte
            if (File.Exists(Promenne.CestaZastavky) && File.Exists(Promenne.CestaSeznamZ) && File.Exists(Promenne.CestaSeznamL))
            {
                try
                {
                    Promenne.Zastavky = JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText(Promenne.CestaZastavky));
                    Promenne.SeznamZastavek = JsonConvert.DeserializeObject<List<Zastavka>>(File.ReadAllText(Promenne.CestaSeznamZ));
                    Promenne.SeznamLinek = JsonConvert.DeserializeObject<List<Linka>>(File.ReadAllText(Promenne.CestaSeznamL));
                }
                catch (JsonException)
                {
                    await Shell.Current.DisplayAlert("Chyba", "Chyba při načtení cache!", "OK");
                }
            }
        }
    }
}

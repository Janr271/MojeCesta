using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Xamarin.Essentials;
using Newtonsoft.Json;
using MojeCesta.Models;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace MojeCesta.Services
{
    public static class AktualizaceDat
    {
        public static bool PouzeWifi
        {
            get
            {
                return Preferences.Get(nameof(PouzeWifi), true);
            }
            set
            {
                Preferences.Set(nameof(PouzeWifi), value);
            }
        }

        public static bool AutomatickaAktualizace
        {
            get
            {
                return Preferences.Get(nameof(AutomatickaAktualizace), true);
            }
            set
            {
                Preferences.Set(nameof(AutomatickaAktualizace), value);
            }
        }

        public static TimeSpan Frekvence
        {
            get
            {
                return new TimeSpan(Preferences.Get(nameof(Frekvence), new TimeSpan(7, 0, 0, 0).Ticks));
            }
            set
            {
                Preferences.Set(nameof(Frekvence), value.Ticks);
            }
        }

        public static DateTime? PosledniAktualizace
        {
            get
            {
                return Preferences.ContainsKey(nameof(PosledniAktualizace)) ? (DateTime?)Preferences.Get(nameof(PosledniAktualizace), new DateTime()) : null;
            }
            set
            {
                Preferences.Set(nameof(PosledniAktualizace), (DateTime)value);
            }
        }

        public static async Task Aktualizovat(Views.AktualizacePage aktualizacePage)
        {
            // Indikace aktivity pro uživatele
            aktualizacePage.Aktivita.IsRunning = true;

            // Pokud je zařízení připojeno k internetu
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                // Pokud je připojeno k wifi ve striktním režimu
                if (!PouzeWifi || Connectivity.ConnectionProfiles.Contains(ConnectionProfile.WiFi))
                {
                    var vysledek = await Database.Aktualizovat();
                    
                    // Pokud se aktualizace zdařila
                    if (vysledek.Item1)
                    {
                        // Cashování dat k vyhledávání spojení
                        Route_stop[] linky = await Database.NacistLinky();
                        Dictionary<string, int> zastavky = new Dictionary<string, int>();
                        List<Stop> seznamZastavek = new List<Stop>();
                        List<Route> seznamLinek = new List<Route>();
                        int indexZacatku = 0;

                        // Vytvoření seznamu všech linek s jejich zastávkami
                        for (int i = 0; i < linky.Length; i++)
                        {
                            if (i != 0 && (linky[i].Route_id != linky[i - 1].Route_id || linky[i].Direction_id != linky[i - 1].Direction_id))
                            {
                                int pocet = i - (indexZacatku + 1);

                                Route_stop[] a = new Route_stop[pocet];
                                Array.Copy(linky, indexZacatku, a, 0, pocet);

                                seznamLinek.Add(new Route(a, a[0].Direction_id, await Database.NajitLinku(a[0].Route_id), false));
                                indexZacatku = i;
                            }
                            else if (i == linky.Length - 1)
                            {
                                int pocet = i - indexZacatku;

                                Route_stop[] a = new Route_stop[pocet];
                                Array.Copy(linky, indexZacatku, a, 0, pocet);

                                seznamLinek.Add(new Route(a, a[0].Direction_id, await Database.NajitLinku(a[0].Route_id), false));
                            }

                            if (!zastavky.ContainsKey(linky[i].Stop_id))
                            {
                                zastavky.Add(linky[i].Stop_id, zastavky.Count);
                                seznamZastavek.Add(new Stop(Database.NajitZastavku(linky[i].Stop_id).Result));
                            }

                            seznamZastavek[zastavky[linky[i].Stop_id]].Linky.Add(seznamLinek.Count);
                        }

                        // Poloměr země
                        double R = 6371000 * Math.Sqrt(2.0);

                        // Spočítat pěší vazby mezi stanicemi pomocí Haversinovy formule
                        for (int i = 1; i < seznamZastavek.Count; i++)
                        {
                            double lat1 = seznamZastavek[i].Stop_lat * Math.PI / 180;
                            double lon1 = seznamZastavek[i].Stop_lon * Math.PI / 180;
                            double cos1 = Math.Cos(lat1);
                            double sin1 = Math.Sin(lat1);

                            for (int y = 0; y < i; y++)
                            {
                                double lat2 = seznamZastavek[y].Stop_lat * Math.PI / 180;
                                double lon2 = seznamZastavek[y].Stop_lon * Math.PI / 180;

                                double d = R * Math.Sqrt(Math.Abs(1.0 - cos1 * Math.Cos(lat2) * Math.Cos(lon1 - lon2) - sin1 * Math.Sin(lat2)));

                                if (d < 250)
                                {
                                    seznamLinek.Add(new Route(new Route_stop[] {
                                new Route_stop("0",Route_stop.Direction.OneDirection, seznamZastavek[i].Stop_id, 1),
                                new Route_stop("0",Route_stop.Direction.OneDirection, seznamZastavek[y].Stop_id, 2) },
                                            Route_stop.Direction.OneDirection, new Route(), true));

                                    seznamLinek.Add(new Route(new Route_stop[] {
                                new Route_stop("0",Route_stop.Direction.OneDirection, seznamZastavek[y].Stop_id, 1),
                                new Route_stop("0",Route_stop.Direction.OneDirection, seznamZastavek[i].Stop_id, 2) },
                                            Route_stop.Direction.OppositeDirection, new Route(), true));
                                }
                            }
                        }

                        Promenne.SeznamZastavek = seznamZastavek;
                        Promenne.SeznamLinek = seznamLinek;
                        Promenne.Zastavky = zastavky;

                        try
                        {
                            //Uložit proměnné do souboru
                            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                            File.WriteAllText(Promenne.CestaSeznamZ, JsonConvert.SerializeObject(seznamZastavek, Formatting.Indented, settings), System.Text.Encoding.UTF8);
                            File.WriteAllText(Promenne.CestaSeznamL, JsonConvert.SerializeObject(seznamLinek, Formatting.Indented, settings), System.Text.Encoding.UTF8);
                            File.WriteAllText(Promenne.CestaZastavky, JsonConvert.SerializeObject(zastavky), System.Text.Encoding.UTF8);

                            // Uložit datum poslední aktualizace
                            PosledniAktualizace = DateTime.Now;

                            // Odemknout UI
                            aktualizacePage.Zavrit();
                            return;
                        }
                        catch (JsonException)
                        {
                            await aktualizacePage.DisplayAlert("Chyba", "Chyba při vytváření cache!", "OK");
                        }
                    }

                    // Pokud ne, vypsat chybovou hlášku
                    else
                    {
                        await aktualizacePage.DisplayAlert("Chyba", vysledek.Item2, "OK");
                    }

                }
                else
                {
                    await aktualizacePage.DisplayAlert("Chyba", "Zařízení není připojeno k Wifi!", "OK");
                }
            }
            else
            {
                await aktualizacePage.DisplayAlert("Chyba", "Zařízení není připojeno k internetu!", "OK");

            }

            // Ukonční indikace aktivity
            aktualizacePage.Aktivita.IsRunning = false;
        }
    }
}

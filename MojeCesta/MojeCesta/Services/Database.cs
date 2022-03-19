using FileHelpers;
using MojeCesta.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;

namespace MojeCesta.Services
{

    public static class Database
    {
        private static SQLiteAsyncConnection db;

        static readonly string cesta = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        static readonly string cestaKZipu = Path.Combine(cesta, "gtfs.zip");
        static readonly string cestaKeSlozce = Path.Combine(cesta, "gtfs");
        static readonly string cestaKDatabazi = Path.Combine(cesta, "gtfs.db3");

        public static async Task Inicializovat()
        {
            // Vytvoření spojení s databází
            db = new SQLiteAsyncConnection(cestaKDatabazi);
            await db.EnableWriteAheadLoggingAsync();

            // Vytvoření příslušných tabulek
            await db.CreateTableAsync<Agency>();
            await db.CreateTableAsync<Calendar>();
            await db.CreateTableAsync<Calendar_date>();
            await db.CreateTableAsync<Fare_attribute>();
            await db.CreateTableAsync<Fare_rule>();
            await db.CreateTableAsync<Feed_info>();
            await db.CreateTableAsync<Level>();
            await db.CreateTableAsync<Pathway>();
            await db.CreateTableAsync<Route_stop>();
            await db.CreateTableAsync<Route_sub_agency>();
            await db.CreateTableAsync<Route>();
            await db.CreateTableAsync<Shape>();
            await db.CreateTableAsync<Stop_time>();
            await db.CreateTableAsync<Stop>();
            await db.CreateTableAsync<Transfer>();
            await db.CreateTableAsync<Trip>();
            await db.CreateTableAsync<HistorieSpojeni>();
            await db.CreateTableAsync<HistorieOdjezdu>();
        }

        public static async Task<Tuple<bool, string>> Aktualizovat()
        {
            try
            {
                // Stáhnout soubor z internetu rozzipovat ho a odstranit původní zip
                using (WebClient client = new WebClient())
                {
                   await client.DownloadFileTaskAsync(new Uri(@"http://data.pid.cz/PID_GTFS.zip"), cestaKZipu);
                }
            }
            catch (WebException)
            {
                return new Tuple<bool, string>(false, "Vyskytla se chyba při stahování dat!");
            }

            try
            {
                if (Directory.Exists(cestaKeSlozce))
                {
                    Directory.Delete(cestaKeSlozce, true);
                }
                ZipFile.ExtractToDirectory(cestaKZipu, cestaKeSlozce);
                File.Delete(cestaKZipu);
            }
            catch (IOException)
            {
                return new Tuple<bool, string>(false, "Vyskytla se chyba při rozbalování dat!");
            }

            try
            {
                // Vymaže starou databázi
                SmazatDatabazi();

                // Naplnit databázi hodnotamy ze souborů
                // Některé tabulky nejsou potřeba a kvůli výkonosti nebudou aktualizovány
                Task[] databaze =
                {
                //NaplnitTabulku<Agency>("agency.txt"),
                NaplnitTabulku<Calendar>("calendar.txt"),
                //NaplnitTabulku<Calendar_date>("calendar_dates.txt"),
                //NaplnitTabulku<Fare_attribute>("fare_attributes.txt"),
                //NaplnitTabulku<Fare_rule>("fare_rules.txt"),
                NaplnitTabulku<Feed_info>("feed_info.txt"),
                //NaplnitTabulku<Level>("levels.txt"),
                //NaplnitTabulku<Pathway>("pathways.txt"),
                NaplnitTabulku<Route_stop>("route_stops.txt"),
                //NaplnitTabulku<Route_sub_agency>("route_sub_agencies.txt"),
                NaplnitTabulku<Route>("routes.txt"),
                NaplnitStanice("stops.txt"),
                //NaplnitTabulku<Transfer>("transfers.txt"),
                NaplnitTabulku<Trip>("trips.txt"),
                //NaplnitTabulku<Shape>("shapes.txt"), 
                NaplnitTabulku<Stop_time>("stop_times.txt")
                };

                await Task.WhenAll(databaze);
            }
            catch (SQLiteException)
            {
                return new Tuple<bool, string>(false, "Vyskytla se chyba při práci s databází!");
            }

            return new Tuple<bool, string>(true, "");

        }

        // Naplnit tabulku v databázi hodnotami
        private static async Task NaplnitTabulku<T>(string soubor)
        where T : class
        {
            var engine = new FileHelperEngine<T>();
            engine.Options.IgnoreFirstLines = 1;
            await db.RunInTransactionAsync(tran => { tran.InsertAll(engine.ReadFile(Path.Combine(cestaKeSlozce, soubor))); });
        }

        private static async Task NaplnitStanice(string soubor)
        {
            var engine = new FileHelperEngine<Stop>();
            engine.Options.IgnoreFirstLines = 1;
            Stop[] vysledek = engine.ReadFile(Path.Combine(cestaKeSlozce, soubor));
            for (int i = 0; i < vysledek.Length; i++)
            {
                vysledek[i].Search_name = Promenne.Normalizovat(vysledek[i].Stop_name);
            }
            await db.RunInTransactionAsync(tran => { tran.InsertAll(vysledek); });
        }

        // Smazat všechny tabulky v databázi
        public static async void SmazatDatabazi()
        {
            Task[] databaze =
            {
                db.DeleteAllAsync<Agency>(),
                db.DeleteAllAsync<Calendar>(),
                db.DeleteAllAsync<Calendar_date>(),
                db.DeleteAllAsync<Fare_attribute>(),
                db.DeleteAllAsync<Fare_rule>(),
                db.DeleteAllAsync<Feed_info>(),
                db.DeleteAllAsync<Level>(),
                db.DeleteAllAsync<Pathway>(),
                db.DeleteAllAsync<Route_stop>(),
                db.DeleteAllAsync<Route_sub_agency>(),
                db.DeleteAllAsync<Route>(),
                db.DeleteAllAsync<Shape>(),
                db.DeleteAllAsync<Stop_time>(),
                db.DeleteAllAsync<Stop>(),
                db.DeleteAllAsync<Transfer>(),
                db.DeleteAllAsync<Trip>()
            };

            await Task.WhenAll(databaze);
        }

        // Nápověda stanic v HledaniPage
        public static Task<List<Stop>> HledaniStanicePodleJmena(string jmeno)
        {
            return db.QueryAsync<Stop>($"SELECT Stop_name, Stop_id FROM Stop WHERE Search_name LIKE '%{jmeno}%' AND Stop_id IN (SELECT Stop_id FROM Route_stop) GROUP BY Stop_Name ORDER BY length(Stop_name) LIMIT 10");
        }

        // Vyhledat ostatní nástupiště v odjezdech ze stanice
        public static Task<Stop[]> ZastavkyPodleJmena(string jmeno)
        {
            return db.Table<Stop>().Where(a => a.Search_name.Contains(jmeno) && (a.Location_type == Stop.LocationType.Station || a.Location_type == Stop.LocationType.Stop || a.Location_type == Stop.LocationType.BoardingArea)).ToArrayAsync();
        }

        // Najít odjezdy z nástupiště v odjezdech ze stanice
        public static async Task<Stop_time[]> NajitOdjezdy(Stop zastavka, TimeSpan cas, DateTime datum)
        {
            List<int> linky = Promenne.SeznamZastavek[Promenne.Zastavky[zastavka.Stop_id]].Linky;
            List<Trip> spoje = new List<Trip>();

            for (int i = 0; i < linky.Count; i++)
            {
                spoje.AddRange(await db.QueryAsync<Trip>($"SELECT * FROM Trip WHERE Route_id = '{Promenne.SeznamLinek[linky[i]].Route_id}'"));
            }
           
            List<string> dnesniSpoje = new List<string>();

            // Kontrola, zda spoj odjíždí ve vybraném termínu
            for (int i = 0; i < spoje.Count; i++)
            {
                Calendar c = NajitKalendar(spoje[i].Service_id).Result;
                if (c.Start_date <= datum && c.End_date >= datum && c.DenVTydnu(datum.DayOfWeek))
                {
                    dnesniSpoje.Add(spoje[i].Trip_id);
                }

            }

            return await db.Table<Stop_time>().Where(a => a.Stop_id == zastavka.Stop_id && a.Departure_time >= cas && a.Pickup_type != Stop_time.Pickup.NoPickup && dnesniSpoje.Contains(a.Trip_id)).Take(5).OrderBy(a => a.Departure_time).ToArrayAsync();
        }

        // Najít nejbližší odjezd z nástupiště ve vyhledávání spojení
        public static Task<Stop_time> NajitNejblizsiOdjezd(Stop zastavka, Route route, TimeSpan cas, DateTime datum)
        {
            // Najit seznam spoju na lince
            List<Trip> spoje = db.Table<Trip>().Where(a => a.Route_id == route.Route_id && (Route_stop.Direction)a.Direction_id == route.Smer).ToListAsync().Result;
            List<string> dnesniSpoje = new List<string>();

            // Kontrola, zda spoj odjíždí ve vybraném termínu
            for (int i = 0; i < spoje.Count; i++)
            {
                Calendar c = NajitKalendar(spoje[i].Service_id).Result;
                if (c.Start_date <= datum && c.End_date >= datum && c.DenVTydnu(datum.DayOfWeek))
                {
                    dnesniSpoje.Add(spoje[i].Trip_id);
                }
            }

            TimeSpan maxCas = cas.Add(new TimeSpan(2, 0, 0));

            // Najít nejbližší odjezd některého spoje
            return db.Table<Stop_time>().Where(a => a.Stop_id == zastavka.Stop_id && a.Departure_time > cas && a.Departure_time < maxCas && dnesniSpoje.Contains(a.Trip_id)).OrderBy(a => a.Departure_time).FirstOrDefaultAsync();
        }

        // Najít příjezd spoje do vybrané stanice ve vyhledávání spojení
        public static Task<Stop_time> NajitPrijezd(string stopId, string tripId)
        {
            return db.Table<Stop_time>().Where(a => a.Trip_id == tripId && a.Stop_id == stopId).FirstOrDefaultAsync();
        }
        public static Task<Trip> NajitSpoj(string id)
        {
            return db.FindAsync<Trip>(id);
        }
        public static Task<Route> NajitLinku(string id)
        {
            return db.FindAsync<Route>(id);
        }
        public static Task<Stop> NajitZastavku(string id)
        {
            return db.FindAsync<Stop>(id);
        }
        public static Task<Calendar> NajitKalendar(string id)
        {
            return db.FindAsync<Calendar>(id);
        }

        // Načíst všechny linky při aktualizaci databáze
        public static Task<Route_stop[]> NacistLinky()
        {
            return db.Table<Route_stop>().ToArrayAsync();
        }

        // Načítání a ukládání historie vyhledávání
        public static Task<List<HistorieOdjezdu>> NacistOdjezdy()
        {
            return db.Table<HistorieOdjezdu>().OrderByDescending(a => a.Id).Take(20).ToListAsync();
        }
        public static Task<List<HistorieSpojeni>> NacistSpojeni()
        {
            return db.Table<HistorieSpojeni>().OrderByDescending(a => a.Id).Take(20).ToListAsync();
        }
        public static async Task UlozitSpojeni(HistorieSpojeni spojeni)
        {
            await db.InsertAsync(spojeni);
        }
        public static async Task UlozitOdjezd(HistorieOdjezdu odjezd)
        {
            await db.InsertAsync(odjezd);
        }

        // Získání informací o databázi
        public static Task<Feed_info> InformaceODatabazi()
        {
            return db.GetAsync<Feed_info>(0);
        }
    }
}

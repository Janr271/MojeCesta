using System;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;
using MojeCesta.Models;
using SQLite;
using FileHelpers;
using System.Collections.Generic;

namespace MojeCesta.Services
{

    static class Database
    {
        private static SQLiteAsyncConnection db;

        static readonly string cesta = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        static readonly string cestaKZipu = Path.Combine(cesta, "gtfs.zip");
        static readonly string cestaKeSlozce = Path.Combine(cesta, "gtfs");
        static readonly string cestaKDatabazi = Path.Combine(cesta, "gtfs.db3");

        public static async Task Inicializovat()
        {
            db = new SQLiteAsyncConnection(cestaKDatabazi); // Vytvoření spojení s databází
            await db.EnableWriteAheadLoggingAsync();

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

        public static async Task Aktualizovat()
        {
            // Vymaže starou databázi
            SmazatDatabazi();

            // Stáhnout soubor z internetu rozzipovat ho a odstranit původní zip
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(@"http://data.pid.cz/PID_GTFS.zip"), cestaKZipu);
            }

            if (Directory.Exists(cestaKeSlozce))
            {
                Directory.Delete(cestaKeSlozce, true);
            }
            ZipFile.ExtractToDirectory(cestaKZipu, cestaKeSlozce);
            File.Delete(cestaKZipu);


            // Naplnit databázi hodnotamy ze souborů
            Task[] databaze =
            {
                NaplnitTabulku<Agency>("agency.txt"),
                NaplnitTabulku<Calendar>("calendar.txt"),
                NaplnitTabulku<Calendar_date>("calendar_dates.txt"),
                NaplnitTabulku<Fare_attribute>("fare_attributes.txt"),
                NaplnitTabulku<Fare_rule>("fare_rules.txt"),
                NaplnitTabulku<Feed_info>("feed_info.txt"),
                NaplnitTabulku<Level>("levels.txt"),
                NaplnitTabulku<Pathway>("pathways.txt"),
                NaplnitTabulku<Route_stop>("route_stops.txt"),
                NaplnitTabulku<Route_sub_agency>("route_sub_agencies.txt"),
                NaplnitTabulku<Route>("routes.txt"),
                NaplnitTabulku<Stop>("stops.txt"),
                NaplnitTabulku<Transfer>("transfers.txt"),
                NaplnitTabulku<Trip>("trips.txt"),
                NaplnitTabulku<Shape>("shapes.txt"),
                NaplnitTabulku<Stop_time>("stop_times.txt")
            };

            await Task.WhenAll(databaze);
        }

        private static async Task NaplnitTabulku<T>(string soubor)
        where T : class
        {
            var engine = new FileHelperEngine<T>();
            engine.Options.IgnoreFirstLines = 1;
            await db.RunInTransactionAsync(tran => { tran.InsertAll(engine.ReadFile(Path.Combine(cestaKeSlozce, soubor))); });
        }

        public static string[] SeznamZastavek()
        {
            return db.Table<Stop>().ToArrayAsync().Result.Select(a => a.Stop_name).ToArray();
        }

        public static async void SmazatDatabazi()
        {
            await db.DeleteAllAsync<Agency>();
            await db.DeleteAllAsync<Calendar>();
            await db.DeleteAllAsync<Calendar_date>();
            await db.DeleteAllAsync<Fare_attribute>();
            await db.DeleteAllAsync<Fare_rule>();
            await db.DeleteAllAsync<Feed_info>();
            await db.DeleteAllAsync<Level>();
            await db.DeleteAllAsync<Pathway>();
            await db.DeleteAllAsync<Route_stop>();
            await db.DeleteAllAsync<Route_sub_agency>();
            await db.DeleteAllAsync<Route>();
            await db.DeleteAllAsync<Shape>();
            await db.DeleteAllAsync<Stop_time>();
            await db.DeleteAllAsync<Stop>();
            await db.DeleteAllAsync<Transfer>();
            await db.DeleteAllAsync<Trip>();
        }

        public static Task<Stop[]> NajitStanici(string jmeno)
        {
            return db.Table<Stop>().Where(a => a.Stop_name.ToLower().Contains(jmeno.ToLower()) && a.Asw_stop_id == "1").Take(10).ToArrayAsync();
        }

        public static Task<Stop[]> NajitZastavky(string jmeno)
        {
            return db.Table<Stop>().Where(a => a.Stop_name.ToLower().Contains(jmeno.ToLower()) && a.Location_type == Stop.LocationType.Stop).ToArrayAsync();
        }
        public static Task<Stop> NajitZastavku(string jmeno)
        {
            return db.Table<Stop>().FirstAsync(a => a.Stop_name.Contains(jmeno));
        }
        public static Task<Stop_time[]> NajitOdjezdy(Stop zastavka, TimeSpan cas)
        {
            return db.Table<Stop_time>().Where(a => a.Stop_id == zastavka.Stop_id && a.Departure_time >= cas).Take(5).OrderBy(a => a.Departure_time).ToArrayAsync();
        }
        public static Task<Trip> NajitSpoj(string id)
        {
            return db.FindAsync<Trip>(id);
        }
        public static Task<Route> NajitLinku(string id)
        {
            return db.FindAsync<Route>(id);
        }
        public static bool SpojObsahujeCil(string idSpoje, string idCile, int posun)
        {
            return db.Table<Stop_time>().Where(a => a.Stop_id == idSpoje && a.Trip_id == idCile && a.Stop_sequence > posun).CountAsync().Result != 0;
        }
        public static Task<Stop_time> NajitDalsiZastavku(string idLinky, string idZastavky, int posun)
        {
            return db.FindAsync<Stop_time>(a => a.Stop_id == idLinky && a.Trip_id == idZastavky && a.Stop_sequence > posun);
        }

        public static Task<List<HistorieOdjezdu>> NacistOdjezdy()
        {
            return db.Table<HistorieOdjezdu>().Take(50).ToListAsync();
        }
        public static Task<List<HistorieSpojeni>> NacistSpojeni()
        {
            return db.Table<HistorieSpojeni>().Take(50).ToListAsync();
        }
        public static async Task UlozitSpojeni(HistorieSpojeni spojeni)
        {
            await db.InsertAsync(spojeni);
        }
        public static async Task UlozitOdjezd(HistorieOdjezdu odjezd)
        {
            await db.InsertAsync(odjezd);
        }

        public static Task<Feed_info> InformaceODatabazi()
        {
            return db.GetAsync<Feed_info>(0);
        }
    }
}

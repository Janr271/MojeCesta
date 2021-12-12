using System;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using MojeCesta.Models;
using SQLite;
using FileHelpers;

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
        }

        public static async Task Aktualizovat()
        {
            // Vymaže starou databázi
            SmazatDatabazi();

            // Stáhnout soubor z internetu rozzipovat ho a odstranit původní zip
            AktualizaceDat.Stahnout(cestaKZipu);
            if (Directory.Exists(cestaKeSlozce))
            {
                Directory.Delete(cestaKeSlozce, true);
            }
            ZipFile.ExtractToDirectory(cestaKZipu, cestaKeSlozce);
            File.Delete(cestaKZipu);

            // Naplnit databázi údaji ze souborů
            await NaplnitTabulku<Agency>(Path.Combine(cestaKeSlozce, "agency.txt"));
            await NaplnitTabulku<Calendar>(Path.Combine(cestaKeSlozce, "calendar.txt"));
            await NaplnitTabulku<Calendar_date>(Path.Combine(cestaKeSlozce, "calendar_dates.txt"));
            await NaplnitTabulku<Fare_attribute>(Path.Combine(cestaKeSlozce, "fare_attributes.txt"));
            await NaplnitTabulku<Fare_rule>(Path.Combine(cestaKeSlozce, "fare_rules.txt"));
            await NaplnitTabulku<Feed_info>(Path.Combine(cestaKeSlozce, "feed_info.txt"));
            await NaplnitTabulku<Level>(Path.Combine(cestaKeSlozce, "levels.txt"));
            await NaplnitTabulku<Pathway>(Path.Combine(cestaKeSlozce, "pathways.txt"));
            await NaplnitTabulku<Route_stop>(Path.Combine(cestaKeSlozce, "route_stops.txt"));
            await NaplnitTabulku<Route_sub_agency>(Path.Combine(cestaKeSlozce, "route_sub_agencies.txt"));
            await NaplnitTabulku<Route>(Path.Combine(cestaKeSlozce, "routes.txt"));
            await NaplnitTabulkuRychle<Shape>(Path.Combine(cestaKeSlozce, "shapes.txt"));
            await NaplnitTabulkuRychle<Stop_time>(Path.Combine(cestaKeSlozce, "stop_times.txt"));
            await NaplnitTabulku<Stop>(Path.Combine(cestaKeSlozce, "stops.txt"));
            await NaplnitTabulku<Transfer>(Path.Combine(cestaKeSlozce, "transfers.txt"));
            await NaplnitTabulku<Trip>(Path.Combine(cestaKeSlozce, "trips.txt"));

            // Odstranit rozzipované soubory
            Directory.Delete(cestaKeSlozce, true);
        }

        private static async Task NaplnitTabulku<T>(string cestaKSouboru)
        where T : class
        {
            var engine = new FileHelperAsyncEngine<T>();
            engine.Options.IgnoreFirstLines = 1;

            using (engine.BeginReadFile(cestaKSouboru))
            {
                foreach (var zaznam in engine)
                {
                    await db.InsertAsync(zaznam);
                }
            }
        }

        private static async Task NaplnitTabulkuRychle<T>(string cestaKSouboru)
        where T : IConstructor, new()
        {
            using (StreamReader s = new StreamReader(cestaKSouboru))
            {
                string radek = s.ReadLine();

                while (true)
                {
                    radek = s.ReadLine();

                    if(radek == null)
                    {
                        break;
                    }

                    T hodnota = new T();
                    hodnota.Consturctor(radek.Split(','));

                    await db.InsertAsync(hodnota);
                }
            }
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

        public static Task<Stop[]> NajitZastavky(string jmeno)
        {
            return db.Table<Stop>().Where(a => a.Stop_name.Contains(jmeno)).ToArrayAsync();
        }
        public static Task<Stop_time[]> NajitOdjezdy(Stop zastavka, DateTime cas)
        {
            return db.Table<Stop_time>().Where(a => a.Stop_id == zastavka.Stop_id && a.Departure_time > cas).OrderBy(a => a.Departure_time).Take(50).ToArrayAsync();
        }
        public static Task<Trip> NajitLinku(string id)
        {
            return db.FindAsync<Trip>(id);
        }
        public static async Task<bool> LinkaObsahujeCil(string idLinky, string idCile, int posun)
        {
            return db.Table<Stop_time>().Where(a => a.Stop_id == idLinky && a.Trip_id == idCile && a.Stop_sequence > posun).CountAsync().Result != 0;
        }
        public static Task<Stop_time> NajitDalsiZastavku(string idLinky, string idZastavky, int posun)
        {
            return db.FindAsync<Stop_time>(a => a.Stop_id == idLinky && a.Trip_id == idZastavky && a.Stop_sequence > posun);
        }
    }
}

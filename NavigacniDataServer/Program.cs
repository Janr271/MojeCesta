using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using SQLite;
using FileHelpers;
using System.Threading.Tasks;
using MojeCesta.Models;
using System.Diagnostics;

namespace NavigacniDataServer
{
    class Program
    {
        static void Main()
        {
            Stopwatch casovac = new();
            casovac.Start();

            Inicializovat().Wait(); // Inicializace databáze
            Aktualizovat(); // Aktualizace databáze

            casovac.Stop();
            Console.WriteLine($"Aktualizace trvala {casovac.Elapsed.Seconds} sekund");
        }

        private static SQLiteAsyncConnection db;

        static readonly string cestaKZipu = "gtfs.zip";
        static readonly string cestaKeSlozce = "gtfs";
        static readonly string cestaKDatabazi = "gtfs.db3";

        public static async Task Inicializovat()
        {
            Console.WriteLine("Inicializace!");

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
        }

        public static void Aktualizovat()
        {
            // Vymaže starou databázi
            SmazatDatabazi();

            // Stáhnout soubor z internetu
            if (!File.Exists(cestaKZipu))
            {
                using (WebClient client = new())
                {
                    Console.WriteLine("Stahování dat");
                    client.DownloadFile(new Uri(@"http://data.pid.cz/PID_GTFS.zip"), cestaKZipu);
                }
            }

            // Rozzipovat ho a odstranit původní zip
            if (Directory.Exists(cestaKeSlozce))
            {
                Directory.Delete(cestaKeSlozce, true);
            }

            ZipFile.ExtractToDirectory(cestaKZipu, cestaKeSlozce);

            File.Delete(cestaKZipu);

            Console.WriteLine("Výroba databáze");

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

            Task.WhenAll(databaze).Wait();
        }

        private static async Task NaplnitTabulku<T>(string soubor)
        where T : class
        {
            var engine = new FileHelperEngine<T>();
            engine.Options.IgnoreFirstLines = 1;
            await db.RunInTransactionAsync(tran => { tran.InsertAll(engine.ReadFile(Path.Combine(cestaKeSlozce, soubor))); });
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
    }
}

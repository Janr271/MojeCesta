using System;
using System.IO;
using System.Linq;
using SQLite;

namespace MojeCesta.Services
{
    
    static class Database
    {
        private static SQLiteConnection db;
        static readonly string cesta = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        static readonly string cestaKZipu = Path.Combine(cesta, "gtfs.zip");
        static readonly string cestaKeSlozce = Path.Combine(cesta, "gtfs");
        static readonly string cestaKDatabazi = Path.Combine(cesta, "gtfs.db");
        public static void Inicializovat()
        {

            bool databazeExistuje = File.Exists(cestaKDatabazi);

            var connection = new SQLiteConnectionString(cestaKDatabazi,SQLiteOpenFlags.ReadWrite,true);
            db = new SQLiteConnection(connection);

            db.CreateTable<Models.Agency>();
            db.CreateTable<Models.Calendar>();
            db.CreateTable<Models.Calendar_date>();
            db.CreateTable<Models.Fare_attribute>();
            db.CreateTable<Models.Fare_rule>();
            db.CreateTable<Models.Feed_info>();
            db.CreateTable<Models.Level>();
            db.CreateTable<Models.Pathway>();
            db.CreateTable<Models.Route_stop>();
            db.CreateTable<Models.Route_sub_agency>();
            db.CreateTable<Models.Route>();
            db.CreateTable<Models.Shape>();
            db.CreateTable<Models.Stop_time>();
            db.CreateTable<Models.Stop>();
            db.CreateTable<Models.Transfer>();
            db.CreateTable<Models.Trip>();

            if (!databazeExistuje)
            {
                Aktualizovat();
            }
        }

        public static void Aktualizovat()
        {
            File.Delete(cestaKDatabazi);
            File.Create(cestaKDatabazi);

            DataDownloader.Download(cestaKZipu, cestaKeSlozce);

            NaplnitTabulku<Models.Agency>(Path.Combine(cestaKeSlozce, "agency.txt"));
            NaplnitTabulku<Models.Calendar>(Path.Combine(cestaKeSlozce, "calendar.txt"));
            NaplnitTabulku<Models.Calendar_date>(Path.Combine(cestaKeSlozce, "calendar_times.txt"));
            NaplnitTabulku<Models.Fare_attribute>(Path.Combine(cestaKeSlozce, "fare_attributes.txt"));
            NaplnitTabulku<Models.Fare_rule>(Path.Combine(cestaKeSlozce, "fare_rules.txt"));
            NaplnitTabulku<Models.Feed_info>(Path.Combine(cestaKeSlozce, "feed_info.txt"));
            NaplnitTabulku<Models.Level>(Path.Combine(cestaKeSlozce, "levels.txt"));
            NaplnitTabulku<Models.Pathway>(Path.Combine(cestaKeSlozce, "pathways.txt"));
            NaplnitTabulku<Models.Route_stop>(Path.Combine(cestaKeSlozce, "route_stops.txt"));
            NaplnitTabulku<Models.Route_sub_agency>(Path.Combine(cestaKeSlozce, "route_sub_agencies.txt"));
            NaplnitTabulku<Models.Route>(Path.Combine(cestaKeSlozce, "routes.txt"));
            NaplnitTabulku<Models.Shape>(Path.Combine(cestaKeSlozce, "shapes.txt"));
            NaplnitTabulku<Models.Stop_time>(Path.Combine(cestaKeSlozce, "stop_times.txt"));
            NaplnitTabulku<Models.Stop>(Path.Combine(cestaKeSlozce, "stops.txt"));
            NaplnitTabulku<Models.Transfer>(Path.Combine(cestaKeSlozce, "transfers.txt"));
            NaplnitTabulku<Models.Trip>(Path.Combine(cestaKeSlozce, "trips.txt"));
        }

        public static void NaplnitTabulku<T>(string cestaKSouboru)
        where T : Models.IConstructor, new()
        {
            using(StreamReader reader = new StreamReader(cestaKSouboru))
            {
                reader.ReadLine(); // přeskočit hlavičku
                string radek = reader.ReadLine();

                while(radek != null)
                {
                    T hodnota = new T();
                    hodnota.Consturctor(radek);
                    db.Insert(hodnota);
                }
            }
        }

        public static string[] SeznamZastavek()
        {
            return db.Query<Models.Stop>("SELECT stop_name FROM stops").Select(a => a.Stop_name).ToArray();
        }
    }
}

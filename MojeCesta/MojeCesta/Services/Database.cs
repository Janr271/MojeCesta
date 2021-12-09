using System;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using SQLite;
using System.Threading.Tasks;
using MojeCesta.Models;

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

            db.CreateTableAsync<Agency>().Wait();
            db.CreateTableAsync<Calendar>().Wait();
            db.CreateTableAsync<Calendar_date>().Wait();
            db.CreateTableAsync<Fare_attribute>().Wait();
            db.CreateTableAsync<Fare_rule>().Wait();
            db.CreateTableAsync<Feed_info>().Wait();
            db.CreateTableAsync<Level>().Wait();
            db.CreateTableAsync<Pathway>().Wait();
            db.CreateTableAsync<Route_stop>().Wait();
            db.CreateTableAsync<Route_sub_agency>().Wait();
            db.CreateTableAsync<Route>().Wait();
            db.CreateTableAsync<Shape>().Wait();
            db.CreateTableAsync<Stop_time>().Wait();
            db.CreateTableAsync<Stop>().Wait();
            db.CreateTableAsync<Transfer>().Wait();
            db.CreateTableAsync<Trip>().Wait();
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
            //await NaplnitTabulku<Shape>(Path.Combine(cestaKeSlozce, "shapes.txt"));
            await NaplnitTabulku<Stop_time>(Path.Combine(cestaKeSlozce, "stop_times.txt"));
            await NaplnitTabulku<Stop>(Path.Combine(cestaKeSlozce, "stops.txt"));
            await NaplnitTabulku<Transfer>(Path.Combine(cestaKeSlozce, "transfers.txt"));
            await NaplnitTabulku<Trip>(Path.Combine(cestaKeSlozce, "trips.txt"));

            // Odstranit rozzipované soubory
            Directory.Delete(cestaKeSlozce, true);
        }

        private static async Task NaplnitTabulku<T>(string cestaKSouboru)
        where T : IConstructor, new()
        {
            using (StreamReader reader = new StreamReader(cestaKSouboru))
            {
                string radek = reader.ReadLine(); // přeskočit hlavičku

                while (true) // Přidávat záznamy do tabulky, dokud neprojedeme všechny záznamy
                {
                    radek = reader.ReadLine();

                    if (radek == null)
                    {
                        break;
                    }

                    T hodnota = new T();
                    hodnota.Consturctor(RozdelitCSV(radek));

                    db.InsertAsync(hodnota).Wait();
                }
            }
        }

        private static string[] RozdelitCSV(string radek)
        {
            List<string> vysledek = new List<string>();

            int zacatek = 0; // počáteční index hodnoty
            bool obsahujeUvozovky = false; // hodnota obsahuje uvozovky
            for (int i = 0; i < radek.Length; i++)
            {
                if (radek[i] == ',')
                {
                    if (obsahujeUvozovky == false)
                    {
                        vysledek.Add(radek.Substring(zacatek, i - zacatek)); // Pokud neobsahuje uvozovky tak přidá prvek
                    }
                    else
                    {
                        obsahujeUvozovky = false;
                    }
                    zacatek = i + 1; // nastaví začátek dalšího pole
                }
                else if (radek[i] == '"')
                {
                    obsahujeUvozovky = true;
                    int konec = radek.IndexOf('"', i + 1); // najde druhou uvozovku
                    if (konec != -1)
                    {
                        vysledek.Add(radek.Substring(i + 1, konec - (i + 1))); // přidá hodnotu v uvozovkách
                        i = konec;
                    }
                    else
                    {
                        vysledek.Add(radek.Substring(i + 1, radek.Length - (i + 1))); // přidá poslední člen s chybějící uzavírací uvozovkou a skončí
                        break;
                    }
                }
                else if (i == radek.Length - 1)
                {
                    vysledek.Add(radek.Substring(zacatek, (i + 1) - zacatek)); // přidá poslední prvek
                }
            }

            return vysledek.ToArray();
        }

        public static string[] SeznamZastavek()
        {
            return db.Table<Stop>().ToArrayAsync().Result.Select(a => a.Stop_name).ToArray();
        }

        public static void SmazatDatabazi()
        {
            db.DeleteAllAsync<Agency>().Wait();
            db.DeleteAllAsync<Calendar>().Wait();
            db.DeleteAllAsync<Calendar_date>().Wait();
            db.DeleteAllAsync<Fare_attribute>().Wait();
            db.DeleteAllAsync<Fare_rule>().Wait();
            db.DeleteAllAsync<Feed_info>().Wait();
            db.DeleteAllAsync<Level>().Wait();
            db.DeleteAllAsync<Pathway>().Wait();
            db.DeleteAllAsync<Route_stop>().Wait();
            db.DeleteAllAsync<Route_sub_agency>().Wait();
            db.DeleteAllAsync<Route>().Wait();
            db.DeleteAllAsync<Shape>().Wait();
            db.DeleteAllAsync<Stop_time>().Wait();
            db.DeleteAllAsync<Stop>().Wait();
            db.DeleteAllAsync<Transfer>().Wait();
            db.DeleteAllAsync<Trip>().Wait();
        } 

        public static Task<Stop[]> NajitZastavku(string jmeno)
        {
            return db.Table<Stop>().Where(a => a.Stop_name.Contains(jmeno)).ToArrayAsync();
        }
        public static Task<Stop_time[]> NajitOdjezdy(Stop zastavka, DateTime cas)
        {
            return db.Table<Stop_time>().Where(a => a.Stop_id == zastavka.Stop_id && a.Departure_time > cas).OrderBy(a => a.Departure_time).Take(50).ToArrayAsync();
        }
    }
}

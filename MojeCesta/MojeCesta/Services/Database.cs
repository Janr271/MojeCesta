using System;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using SQLite;

namespace MojeCesta.Services
{
    
    static class Database
    {
        private static SQLiteAsyncConnection db;
        static readonly string cesta = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        static readonly string cestaKZipu = Path.Combine(cesta, "gtfs.zip");
        static readonly string cestaKeSlozce = Path.Combine(cesta, "gtfs");
        static readonly string cestaKDatabazi = Path.Combine(cesta, "gtfs.db3");
        public static void Inicializovat()
        {

            bool databazeExistuje = File.Exists(cestaKDatabazi);

            db = new SQLiteAsyncConnection(cestaKDatabazi);

            db.CreateTableAsync<Models.Agency>().Wait();
            db.CreateTableAsync<Models.Calendar>().Wait();
            db.CreateTableAsync<Models.Calendar_date>().Wait();
            db.CreateTableAsync<Models.Fare_attribute>().Wait();
            db.CreateTableAsync<Models.Fare_rule>().Wait();
            db.CreateTableAsync<Models.Feed_info>().Wait();
            db.CreateTableAsync<Models.Level>().Wait();
            db.CreateTableAsync<Models.Pathway>().Wait();
            db.CreateTableAsync<Models.Route_stop>().Wait();
            db.CreateTableAsync<Models.Route_sub_agency>().Wait();
            db.CreateTableAsync<Models.Route>().Wait();
            db.CreateTableAsync<Models.Shape>().Wait();
            db.CreateTableAsync<Models.Stop_time>().Wait();
            db.CreateTableAsync<Models.Stop>().Wait();
            db.CreateTableAsync<Models.Transfer>().Wait();
            db.CreateTableAsync<Models.Trip>().Wait();

            if (databazeExistuje == false)
            {
                Aktualizovat();
            }
        }

        public static void Aktualizovat()
        {
            // Odstranit starou databázi a vytvořit novou
            File.Delete(cestaKDatabazi);
            File.Create(cestaKDatabazi);

            // Stáhnout soubor z internetu rozzipovat ho a odstranit původní zip
            DataDownloader.Download(cestaKZipu);
            ZipFile.ExtractToDirectory(cestaKZipu, cestaKeSlozce);
            File.Delete(cestaKZipu);

            // Naplnit databázi údaji ze souborů
            NaplnitTabulku<Models.Agency>(Path.Combine(cestaKeSlozce, "agency.txt"));
            NaplnitTabulku<Models.Calendar>(Path.Combine(cestaKeSlozce, "calendar.txt"));
            NaplnitTabulku<Models.Calendar_date>(Path.Combine(cestaKeSlozce, "calendar_dates.txt"));
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

            // Odstranit rozzipované soubory
            Directory.Delete(cestaKeSlozce, true);
        }

        private static void NaplnitTabulku<T>(string cestaKSouboru)
        where T : Models.IConstructor, new()
        {
            using(StreamReader reader = new StreamReader(cestaKSouboru))
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
                    db.InsertAsync(hodnota);
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
                if(radek[i] == ',')
                {
                    if(obsahujeUvozovky == false)
                    {
                        vysledek.Add(radek.Substring(zacatek, i - zacatek)); // Pokud neobsahuje uvozovky tak přidá prvek
                    }
                    else
                    {
                        obsahujeUvozovky = false;
                    }
                    zacatek = i + 1; // nastaví začátek dalšího pole
                }
                else if(radek[i] == '"')
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
                else if(i == radek.Length - 1)
                {
                    vysledek.Add(radek.Substring(zacatek, (i + 1) - zacatek)); // přidá poslední prvek
                }
            }

            return vysledek.ToArray();
        }

        public static string[] SeznamZastavek()
        {
            return db.Table<Models.Stop>().ToArrayAsync().Result.Select(a => a.Stop_name).ToArray();
        }
    }
}

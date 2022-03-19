using System;
using System.Linq;
using System.ComponentModel;

using Xamarin.Forms;
using MojeCesta.Models;
using MojeCesta.Services;
using System.Windows.Input;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace MojeCesta.ViewModels
{
    public class SpojeniViewModel : INotifyPropertyChanged
    {
        public SpojeniViewModel()
        {
            Hledat = new Command(() => NajitSpojeni());
            Task.Run(() => AktualizovatHistorii());
        }

        private Stop zeZastavky = new Stop(null, "");
        private Stop naZastavku = new Stop(null, "");
        private DateTime datum;
        private TimeSpan cas;
        private bool odjezd;
        private bool aktivita;
        private int pocetPrestupu;
        private List<HistorieSpojeni> historie;

        public Stop ZeZastavky
        {
            get => zeZastavky;
            set
            {
                if (zeZastavky == value)
                    return;

                zeZastavky = value;
                OnPropertyChanged(nameof(ZeZastavky));
            }
        }

        public Stop NaZastavku
        {
            get => naZastavku;
            set
            {
                if (naZastavku == value)
                    return;

                naZastavku = value;
                OnPropertyChanged(nameof(NaZastavku));
            }
        }

        public DateTime Datum
        {
            get => datum;
            set
            {
                if (datum == value)
                    return;

                datum = value;
                OnPropertyChanged(nameof(Datum));
            }
        }

        public TimeSpan Cas
        {
            get => cas;
            set
            {
                if (cas == value)
                    return;

                cas = value;
                OnPropertyChanged(nameof(Cas));
            }
        }

        public bool Odjezd
        {
            get => odjezd;
            set
            {
                if (odjezd == value)
                    return;

                odjezd = value;
                OnPropertyChanged(nameof(Odjezd));
            }
        }

        public int PocetPrestupu
        {
            get => pocetPrestupu;
            set
            {
                if (pocetPrestupu == value)
                    return;

                pocetPrestupu = value;
                OnPropertyChanged(nameof(PocetPrestupu));
                OnPropertyChanged(nameof(PrestupyText));
            }
        }

        public string PrestupyText
        {
            get
            {
                if (PocetPrestupu == 1)
                {
                    return $"Maximum {PocetPrestupu} přestup";
                }
                if (PocetPrestupu == 2 || PocetPrestupu == 3 || PocetPrestupu == 4)
                {
                    return $"Maximum {PocetPrestupu} přestupy";
                }

                return $"Maximum {PocetPrestupu} přestupů";
            }
        }

        public List<HistorieSpojeni> Historie
        {
            get
            {
                return historie;
            }
            private set
            {
                if (historie == value)
                    return;

                historie = value;
                OnPropertyChanged(nameof(Historie));
            }
        }

        public bool Aktivita
        {
            get => aktivita;
            set
            {
                if (aktivita == value)
                    return;

                aktivita = value;
                OnPropertyChanged(nameof(Aktivita));
            }
        }

        public List<SpojeniMeziStanicemi> Vysledky
        {
            get => Promenne.VysledkySpojeni;
            set
            {
                if (Promenne.VysledkySpojeni == value)
                    return;

                Promenne.VysledkySpojeni = value;
                OnPropertyChanged(nameof(Vysledky));
            }
        }
        public ICommand Hledat { private set; get; }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string jmeno)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(jmeno));
        }

        public async void AktualizovatHistorii()
        {
            Historie = await Database.NacistSpojeni();
        }

        public async void NajitSpojeni()
        {
            // Zapnout indikaci aktivity pro uživatele
            Aktivita = true;

            if (File.Exists(Promenne.CestaSeznamL))
            {
                try
                {
                    Promenne.SeznamLinek = JsonConvert.DeserializeObject<List<Route>>(File.ReadAllText(Promenne.CestaSeznamL));
                }
                catch (JsonException)
                {
                    await Shell.Current.DisplayAlert("Chyba", "Chyba při načtení cache!", "OK");
                }
            }

            List<Cesta> reseni = new List<Cesta>();
            Queue<Cesta> zasobnik = new Queue<Cesta>();
            int vychoziStanId = Promenne.Zastavky[ZeZastavky.Stop_id];
            int cilovaStanId = Promenne.Zastavky[NaZastavku.Stop_id];
            // Přidat výchozí stanici do zásobníku
            zasobnik.Enqueue(new Cesta(vychoziStanId, new List<Presun>(), 0));

            // Dokud zásobník není prázdný
            while(zasobnik.Count > 0)
            {
                // Získat cestu ze zásobníku
                Cesta c = zasobnik.Dequeue();

                // Pokud cílová stanice není cíl  nebo pokud nevypršel limit přestupů
                if(c.IdStanice != cilovaStanId && c.Prestupu < PocetPrestupu)
                {
                    // Zastávka, na které končí aktuální cesta
                    Stop aktualniZ = Promenne.SeznamZastavek[c.IdStanice];

                    // Najít všechny linky, co procházejí vybranou stanicí
                    for (int i = 0; i < aktualniZ.Linky.Count; i++)
                    {
                        int poradiStanice = -1;
                        bool obsahujeCil = false;
                        Route aktualniL = Promenne.SeznamLinek[aktualniZ.Linky[i]];

                        for (int y = 0; y < aktualniL.Zastavky.Length; y++)
                        {
                            // Získat pořadí aktuální stanice
                            if (Promenne.Zastavky[aktualniL.Zastavky[y].Stop_id] == c.IdStanice)
                            {
                                poradiStanice = y;
                            }
                            // Zkontrolovat, že linka neobsahuje cíl
                            if (poradiStanice != -1 && Promenne.Zastavky[aktualniL.Zastavky[y].Stop_id] == cilovaStanId)
                            {
                                Cesta novaCesta = new Cesta(Promenne.Zastavky[aktualniL.Zastavky[y].Stop_id], c.ListPresunu, c.Prestupu);
                                novaCesta.ListPresunu.Add(new Presun(c.IdStanice, cilovaStanId, aktualniZ.Linky[i]));
                                reseni.Add(novaCesta);
                                obsahujeCil = true;
                                break;
                            }
                        }

                        // Pokud linka neobsahuje cíl a obsahuje nenavštívené zastávky
                        if(!obsahujeCil && (aktualniL.Navstiveno == -1 || aktualniL.Navstiveno > poradiStanice))
                        {
                            // Vytvořit dotaz pro všechny další nenavštívené stanice linky
                            int konecSeznamu = aktualniL.Navstiveno == -1 ? aktualniL.Zastavky.Length : aktualniL.Navstiveno;

                            for (int y = poradiStanice + 1; y < konecSeznamu; y++)
                            {
                                // Vytvořit nový krok k prozkoumání
                                Cesta novaCesta = new Cesta(Promenne.Zastavky[aktualniL.Zastavky[y].Stop_id], c.ListPresunu, aktualniL.Pesky? c.Prestupu :  c.Prestupu + 1);
                                novaCesta.ListPresunu.Add(new Presun(c.IdStanice, Promenne.Zastavky[aktualniL.Zastavky[y].Stop_id], aktualniZ.Linky[i]));
                                zasobnik.Enqueue(novaCesta);
                            }

                            // Uložit poslední návštěvu linky
                            aktualniL.Navstiveno = poradiStanice;
                        }
                    }
                }
            }

            // Najít odjezdy spojů na nalezených trasách
            List<SpojeniMeziStanicemi> noveVysledky = new List<SpojeniMeziStanicemi>();

            // Pokud existuje nějaké řešení
            if(reseni.Count > 0)
            {
                int i = 0;
                TimeSpan cas = Cas;
                bool existujeReseni = true;

                // Projít nalezená řešení a spočítat odjezdy
                do
                {
                    TimeSpan pocatecniCas;
                    Stop_time odjezd, prijezd = new Stop_time();
                    double ujetaVzdalenost = 0;
                    SpojeniMeziStanicemi spojeni = new SpojeniMeziStanicemi();
                    bool prvni = true;

                    // Projít všechny dílčí spoje
                    for (int y = 0; y < reseni[i].ListPresunu.Count && existujeReseni; y++)
                    {
                        Presun aktualniP = reseni[i].ListPresunu[y];
                        // Nezapočítávat pěší přechody
                        if (!Promenne.SeznamLinek[aktualniP.LinkaId].Pesky)
                        {
                            odjezd = await Database.NajitNejblizsiOdjezd(Promenne.SeznamZastavek[aktualniP.ZeStaniceId], Promenne.SeznamLinek[aktualniP.LinkaId], cas, Datum);

                            // Pokud neexistuje odjezd přestat hledat
                            if (odjezd == null)
                            {
                                existujeReseni = false;
                                break;
                            }

                            prijezd = await Database.NajitPrijezd(Promenne.SeznamZastavek[aktualniP.NaStaniciId].Stop_id, odjezd.Trip_id);

                            // Pokud neexistuje příjezd přestat hledat
                            if (prijezd == null)
                            {
                                existujeReseni = false;
                                break;
                            }
                            Trip spoj = await Database.NajitSpoj(odjezd.Trip_id);

                            // První spoj naplní hlavičku
                            if (prvni)
                            {
                                prvni = false;
                                pocatecniCas = odjezd.Departure_time;
                                TimeSpan doOdjezdu = new DateTime(Datum.Year, Datum.Month, pocatecniCas.Days > 0 ? Datum.Day + 1 : Datum.Day, pocatecniCas.Hours, pocatecniCas.Minutes, pocatecniCas.Seconds).Subtract(DateTime.Now);
                                spojeni.DoOdjezdu = $"{(doOdjezdu > TimeSpan.Zero ? "za" : "před")} " +
                                    $"{(doOdjezdu.Days > 0 ? $"{Math.Abs(doOdjezdu.Days)} {(doOdjezdu.Days > 1 ? (doOdjezdu.Days < 5 ? "dny" : "dni") : "den")} " : "")}" +
                                    $"{(doOdjezdu.Hours > 0 ? $"{Math.Abs(doOdjezdu.Hours)} hod " : "")}" +
                                    $"{(doOdjezdu.Minutes > 0 ? $"{Math.Abs(doOdjezdu.Minutes)} min" : "")}";
                            }

                            // Přidat do výsledků nalezené spojení
                            spojeni.Add(new Spojeni(Promenne.SeznamLinek[aktualniP.LinkaId].Route_short_name,
                                                     $"směr {spoj.Trip_headsign}",
                                                     Promenne.SeznamZastavek[aktualniP.ZeStaniceId].Stop_name,
                                                     Promenne.SeznamZastavek[aktualniP.NaStaniciId].Stop_name,
                                                     odjezd.Departure_time.ToString(@"hh\:mm"),
                                                     prijezd.Arrival_time.ToString(@"hh\:mm"),
                                                     y == reseni[i].ListPresunu.Count - 1 ? "" : "přestup asi 3 min"));
                            
                            // Posunout čas pro vyhledávání navazujícího odjezdu
                            cas = prijezd.Arrival_time.Add(new TimeSpan(0, 3, 0));

                            // Připočíst celkovou ujetou vzdálenost
                            ujetaVzdalenost += (double)prijezd.Shape_dist_traveled - (double)odjezd.Shape_dist_traveled;
                        }
                    }

                    if (existujeReseni)
                    {
                        // Započítat metriku cesty
                        spojeni.Metrika = $"{(int)prijezd.Arrival_time.Subtract(pocatecniCas).TotalMinutes} min, {(int)ujetaVzdalenost} km";
                        noveVysledky.Add(spojeni);
                    }

                    // Pokud se jedná o poslední možné řešení, projít seznam znovu s pozdějším časem, jinak přejít na další řešení
                    if (i == reseni.Count - 1)
                    {
                        // Pokud se řešení nedaří najít, ukončit
                        if (noveVysledky.Count == 0)
                        {
                            break;
                        }

                        i = 0;
                        cas = pocatecniCas.Add(new TimeSpan(0, 3, 0));
                        existujeReseni = true;
                    }
                    else
                    {
                        i++;
                        cas = Cas;
                        existujeReseni = true;
                    }
                }
                // Opakovat dokud nebude nalezeno určité množství výsledků, 
                while (noveVysledky.Count < 6);
            }

            if(noveVysledky.Count == 0)
            {
                // Oznámit, že nebylo nalezeno spojení
                SpojeniMeziStanicemi s = new SpojeniMeziStanicemi
                {
                    new Spojeni("", "Nebylo nalezeno žádné spojení", "", "", "", "", "")
                };
                noveVysledky.Add(s);
            }

            // Uložit výsledek do seznamu výsledků
            Vysledky = noveVysledky;

            // Přejít na okno s výsledky
            await Device.InvokeOnMainThreadAsync(() => Shell.Current.GoToAsync(nameof(Views.VysledkySpojeniPage)));

            // Uložit dotaz do historie a aktualizovat seznam
            await Database.UlozitSpojeni(new HistorieSpojeni(ZeZastavky.Stop_id, ZeZastavky.Stop_name, NaZastavku.Stop_id, NaZastavku.Stop_name, PocetPrestupu, Datum, Cas));
            await Task.Run(() => AktualizovatHistorii());

            // Vypnout indikaci aktivity
            Aktivita = false;
        }

        class Presun
        {
            public Presun(int zeStaniceId, int naStaniciId, int linkaId)
            {
                ZeStaniceId = zeStaniceId;
                NaStaniciId = naStaniciId;
                LinkaId = linkaId;
            }

            public int ZeStaniceId;
            public int NaStaniciId;
            public int LinkaId;
        }

        class Cesta
        {
            public Cesta(int idStanice,List<Presun> presuny, int prestupu)
            {
                IdStanice = idStanice;
                ListPresunu = new List<Presun>();
                Prestupu = prestupu;

                for (int i = 0; i < presuny.Count; i++)
                {
                    ListPresunu.Add(new Presun(presuny[i].ZeStaniceId, presuny[i].NaStaniciId, presuny[i].LinkaId));
                }
            }
            public int IdStanice;
            public List<Presun> ListPresunu;
            public int Prestupu;
        }
    }
}
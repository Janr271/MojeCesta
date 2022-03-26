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
        private Color barva;
        private bool prijezd;
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

        public bool Prijezd
        {
            get => prijezd;
            set
            {
                if (prijezd == value)
                    return;

                prijezd = value;
                OnPropertyChanged(nameof(Prijezd));
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

        // Barva aktivity
        public Color Barva
        {
            get => barva;
            set
            {
                if (barva == value)
                    return;

                barva = value;
                OnPropertyChanged(nameof(Barva));
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
            Barva = Color.Accent;
            Aktivita = true;

            // Vyresetovat hodnoty z předchozího hledání
            for (int i = 0; i < Promenne.SeznamLinek.Count; i++)
            {
                Promenne.SeznamLinek[i].Navstiveno = Promenne.SeznamLinek[i].Zastavky.Length;
                Promenne.SeznamLinek[i].DnesniSpoje = null;
            }

            List<Cesta> reseni = new List<Cesta>();
            Queue<Cesta> zasobnik = new Queue<Cesta>();
            int vychoziStanId = Promenne.Zastavky[ZeZastavky.Stop_id];
            int cilovaStanId = Promenne.Zastavky[NaZastavku.Stop_id];

            // Seznam všech nástupních bodů cílové zastávky
            List<int> ciloveStanId = new List<int>();
            ciloveStanId.Add(cilovaStanId);
            for (int i = 0; i < Promenne.SeznamZastavek[cilovaStanId].Linky.Count; i++)
            {
                if (Promenne.SeznamLinek[Promenne.SeznamZastavek[cilovaStanId].Linky[i]].Pesky)
                {
                    ciloveStanId.Add(Promenne.Zastavky[Promenne.SeznamLinek[Promenne.SeznamZastavek[cilovaStanId].Linky[i]].Zastavky[1].Stop_id]);
                }
            }

            // Přidat výchozí stanici do zásobníku
            zasobnik.Enqueue(new Cesta(vychoziStanId, new List<Presun>(), 0));

            // Dokud zásobník není prázdný
            while(zasobnik.Count > 0)
            {
                // Získat cestu ze zásobníku
                Cesta c = zasobnik.Dequeue();

                // Pokud cílová stanice není cíl  nebo pokud nevypršel limit přestupů
                if(c.IdStanice != cilovaStanId && c.Prestupu <= PocetPrestupu)
                {
                    // Zastávka, na které končí aktuální cesta
                    Stop aktualniZ = Promenne.SeznamZastavek[c.IdStanice];

                    // Najít všechny linky, co procházejí vybranou stanicí
                    for (int i = 0; i < aktualniZ.Linky.Count; i++)
                    {
                        int poradiStanice = -1;
                        bool obsahujeCil = false;
                        Route aktualniL = Promenne.SeznamLinek[aktualniZ.Linky[i]];
                        
                        // Zakázat linky s více pěšímy přestupy za sebou
                        if(c.ListPresunu.Count != 0 && Promenne.SeznamLinek[c.ListPresunu[c.ListPresunu.Count - 1].LinkaId].Pesky && aktualniL.Pesky)
                        {
                          continue;
                        }

                        // Najít výchozí zastávku na lince a zkontrolovat, zda linka neobsahuje cíl
                        for (int y = 0; y < aktualniL.Zastavky.Length; y++)
                        {
                            // Najít výchozí zastávku
                            if (Promenne.Zastavky[aktualniL.Zastavky[y].Stop_id] == c.IdStanice)
                            {
                                poradiStanice = y;
                            }
                            // Zkontrolovat, že linka neobsahuje cíl
                            if (poradiStanice != -1 && ciloveStanId.Contains(Promenne.Zastavky[aktualniL.Zastavky[y].Stop_id]))
                            {
                                Cesta novaCesta = new Cesta(Promenne.Zastavky[aktualniL.Zastavky[y].Stop_id], c.ListPresunu, c.Prestupu);
                                novaCesta.ListPresunu.Add(new Presun(c.IdStanice, Promenne.Zastavky[aktualniL.Zastavky[y].Stop_id], aktualniZ.Linky[i]));
                                reseni.Add(novaCesta);
                                obsahujeCil = true;
                                break;
                            }
                        }

                        // Pokud linka neobsahuje cíl a obsahuje nenavštívené zastávky
                        if(!obsahujeCil && poradiStanice != -1 && (aktualniL.Navstiveno > poradiStanice))
                        {
                            // Prozkoumat věechny další nenavštívené zastávky
                            for (int y = poradiStanice + 1; y < aktualniL.Navstiveno; y++)
                            {
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

            // Indikovat, že algoritmus přešel do druhé poloviny výpočtu
            Barva = Color.MediumSeaGreen;

            // Pokud existuje nějaké řešení
            if(reseni.Count > 0)
            {
                int i = 0;
                SpojeniMeziStanicemi spojeni;
                DateTime cas = Datum.Add(Cas);
                TimeSpan pocatecniCas;
                double ujetaVzdalenost;
                bool nejakeReseni = false;

                // Projít nalezená řešení a spočítat odjezdy
                do
                {
                    // Spočítat časy nalezeného spojení
                    if (Prijezd)
                    {
                        spojeni = SpocitatSpojeniObracene(reseni[i], out ujetaVzdalenost, cas);
                    }
                    else
                    {
                        spojeni = SpocitatSpojeni(reseni[i], out ujetaVzdalenost, cas);
                    }
                    
                    if (spojeni != null)
                    {
                        pocatecniCas = spojeni[0].CasOdjezdu;

                        // Započítat metriku cesty
                        spojeni.Vzdalenost = ujetaVzdalenost;
                        spojeni.Cas = spojeni[spojeni.Count -1].CasPrijezdu.Subtract(pocatecniCas);
                        spojeni.Odjezd = Datum.Add(pocatecniCas);

                        // Spočítat přestupy
                        for (int y = 1; y < spojeni.Count; y++)
                        {
                            spojeni[y - 1].Prestup = $"přestup asi {(int)spojeni[y].CasOdjezdu.Subtract(spojeni[y - 1].CasPrijezdu).TotalMinutes} min";
                        }

                        noveVysledky.Add(spojeni);
                        nejakeReseni = true;
                    }
                    else
                    {
                        // Odebrat řešení, které není možné tento den použít
                        reseni.RemoveAt(i--);
                    }

                    // Pokud se jedná o poslední možné řešení, projít seznam znovu s pozdějším časem, jinak přejít na další řešení
                    if (i == reseni.Count - 1)
                    {
                        // Pokud se řešení nedaří najít, ukončit
                        if (!nejakeReseni)
                        {
                            break;
                        }

                        i = 0;
                        cas = Datum.Add(pocatecniCas).AddMinutes(3);
                        nejakeReseni = false;
                    }
                    else
                    {
                        i++;
                        cas = Datum.Add(Cas);
                    }
                }
                // Opakovat dokud nebude nalezeno určité množství výsledků, 
                while (noveVysledky.Count < 6);
            }

            // Oznámit, že nebylo nalezeno spojení
            if (noveVysledky.Count == 0)
            {
                SpojeniMeziStanicemi s = new SpojeniMeziStanicemi { new Spojeni() };
                s.MaReseni = false;
                noveVysledky.Add(s);
            }

            // Seřadit podle odjezdu a zapsat do seznamu výsledků
            noveVysledky.Sort();
            Vysledky = noveVysledky;

            // Přejít na okno s výsledky
            await Device.InvokeOnMainThreadAsync(() => Shell.Current.GoToAsync(nameof(Views.VysledkySpojeniPage)));

            // Uložit dotaz do historie a aktualizovat seznam
            await Database.UlozitSpojeni(new HistorieSpojeni(ZeZastavky.Stop_id, ZeZastavky.Stop_name, NaZastavku.Stop_id, NaZastavku.Stop_name, PocetPrestupu, Datum, Cas));
            await Task.Run(() => AktualizovatHistorii());

            // Vypnout indikaci aktivity
            Aktivita = false;
        }

        SpojeniMeziStanicemi SpocitatSpojeni(Cesta reseni, out double ujetaVzdalenost, DateTime cas)
        {
            bool maReseni = true;
            ujetaVzdalenost = 0;
            SpojeniMeziStanicemi spojeni = new SpojeniMeziStanicemi();
            Stop_time odjezd, prijezd;

            // Projít všechny dílčí spoje
            for (int y = 0; y < reseni.ListPresunu.Count && maReseni; y++)
            {
                Presun aktualniP = reseni.ListPresunu[y];

                // Nezapočítávat pěší přechody
                if (!Promenne.SeznamLinek[aktualniP.LinkaId].Pesky)
                {
                    odjezd = Database.NajitNejblizsiOdjezd(Promenne.SeznamZastavek[aktualniP.ZeStaniceId], Promenne.SeznamLinek[aktualniP.LinkaId], cas).Result;

                    // Pokud neexistuje odjezd přestat hledat
                    if (odjezd == null)
                    {
                        maReseni = false;
                        break;
                    }

                    prijezd = Database.NajitZastaveni(Promenne.SeznamZastavek[aktualniP.NaStaniciId].Stop_id, odjezd.Trip_id).Result;

                    // Pokud neexistuje příjezd přestat hledat
                    if (prijezd == null)
                    {
                        maReseni = false;
                        break;
                    }
                    Trip spoj = Database.NajitSpoj(odjezd.Trip_id).Result;

                    // Přidat do výsledků nalezené spojení
                    spojeni.Add(new Spojeni(
                        Promenne.SeznamLinek[aktualniP.LinkaId].Route_short_name,
                        $"směr {spoj.Trip_headsign}",
                        Promenne.SeznamZastavek[aktualniP.ZeStaniceId].Stop_name,
                        Promenne.SeznamZastavek[aktualniP.NaStaniciId].Stop_name,
                        odjezd.Departure_time,
                        prijezd.Arrival_time
                                            ));

                    // Posunout čas pro vyhledávání navazujícího odjezdu
                    cas = Datum.Add(prijezd.Arrival_time).AddMinutes(3);

                    // Připočíst celkovou ujetou vzdálenost
                    ujetaVzdalenost += (double)prijezd.Shape_dist_traveled - (double)odjezd.Shape_dist_traveled;
                }
            }

            return maReseni? spojeni : null;
        }

        SpojeniMeziStanicemi SpocitatSpojeniObracene(Cesta reseni, out double ujetaVzdalenost, DateTime cas)
        {
            bool maReseni = true;
            ujetaVzdalenost = 0;
            SpojeniMeziStanicemi spojeni = new SpojeniMeziStanicemi();
            Stop_time odjezd, prijezd;

            // Projít všechny dílčí spoje
            for (int y = reseni.ListPresunu.Count - 1; y >= 0 && maReseni; y--)
            {
                Presun aktualniP = reseni.ListPresunu[y];

                // Nezapočítávat pěší přechody
                if (!Promenne.SeznamLinek[aktualniP.LinkaId].Pesky)
                {
                    prijezd = Database.NajitNejblizsiPrijezd(Promenne.SeznamZastavek[aktualniP.NaStaniciId], Promenne.SeznamLinek[aktualniP.LinkaId], cas).Result;

                    // Pokud neexistuje příjezd přestat hledat
                    if (prijezd == null)
                    {
                        maReseni = false;
                        break;
                    }

                    odjezd = Database.NajitZastaveni(Promenne.SeznamZastavek[aktualniP.ZeStaniceId].Stop_id, prijezd.Trip_id).Result;

                    // Pokud neexistuje odjezd přestat hledat
                    if (odjezd == null)
                    {
                        maReseni = false;
                        break;
                    }

                    Trip spoj = Database.NajitSpoj(odjezd.Trip_id).Result;

                    // Přidat do výsledků nalezené spojení
                    spojeni.Add(new Spojeni(
                        Promenne.SeznamLinek[aktualniP.LinkaId].Route_short_name,
                        $"směr {spoj.Trip_headsign}",
                        Promenne.SeznamZastavek[aktualniP.ZeStaniceId].Stop_name,
                        Promenne.SeznamZastavek[aktualniP.NaStaniciId].Stop_name,
                        odjezd.Departure_time,
                        prijezd.Arrival_time
                                            ));

                    // Posunout čas pro vyhledávání navazujícího odjezdu
                    cas = Datum.Add(prijezd.Arrival_time).AddMinutes(3);

                    // Připočíst celkovou ujetou vzdálenost
                    ujetaVzdalenost += (double)prijezd.Shape_dist_traveled - (double)odjezd.Shape_dist_traveled;
                }
            }

            return maReseni ? spojeni : null;
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
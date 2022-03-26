using System;
using System.ComponentModel;

using Xamarin.Forms;
using MojeCesta.Models;
using MojeCesta.Services;
using System.Windows.Input;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MojeCesta.ViewModels
{
    public class OdjezdyViewModel : INotifyPropertyChanged
    {
        public OdjezdyViewModel()
        {
            Hledat = new Command(() => NajitOdjezdy());
            Task.Run(() => AktualizovatHistorii());
        }

        private bool aktivita;
        private Stop zeZastavky = new Stop(null,"");
        private DateTime datum;
        private TimeSpan cas;
        private List<HistorieOdjezdu> historie;

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
        public List<HistorieOdjezdu> Historie 
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
        public List<OdjezdyZeStanice> Vysledky
        {
            get => Promenne.VysledkyOdjezdu;
            set
            {
                if (Promenne.VysledkyOdjezdu == value)
                    return;

                Promenne.VysledkyOdjezdu = value;

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
            Historie = await Database.NacistOdjezdy();
        }

        public async void NajitOdjezdy()
        {
            // Spustit indikátor o aktivitě pro uživatele
            Aktivita = true;

            // Vyresetovat hodnoty z předchozího hledání
            for (int i = 0; i < Promenne.SeznamLinek.Count; i++)
            {
                Promenne.SeznamLinek[i].DnesniSpoje = null;
            }

            List<OdjezdyZeStanice> noveVysledky = new List<OdjezdyZeStanice>();
            int stanId = Promenne.Zastavky[ZeZastavky.Stop_id];

            // Najít všechny podzastávky
            List<Stop> stanice = new List<Stop>();
            stanice.Add(Promenne.SeznamZastavek[stanId]);
            for (int i = 0; i < Promenne.SeznamZastavek[stanId].Linky.Count; i++)
            {
                if (Promenne.SeznamLinek[Promenne.SeznamZastavek[stanId].Linky[i]].Pesky)
                {
                    stanice.Add(Promenne.SeznamZastavek[Promenne.Zastavky[Promenne.SeznamLinek[Promenne.SeznamZastavek[stanId].Linky[i]].Zastavky[1].Stop_id]]);
                }
            }
            stanice.Sort();

            // Najít všechny nástupiště zvolené stanice
            for (int i = 0; i < stanice.Count; i++)
            {
                if (!Promenne.Zastavky.ContainsKey(stanice[i].Stop_id))
                {
                    continue;
                }
                Stop_time[] odjezdy = await Database.NajitOdjezdy(stanice[i], Cas, Datum);
                List<Odjezd> seznamOdjezdu = new List<Odjezd>();

                // Najít odjezdy z vybraného nástupiště pokud existují
                for (int y = 0; y < odjezdy.Length; y++)
                {
                    Trip spoj = await Database.NajitSpoj(odjezdy[y].Trip_id);
                    Route linka = await Database.NajitLinku(spoj.Route_id);
                    seznamOdjezdu.Add(new Odjezd(linka.Route_short_name, $"směr {spoj.Trip_headsign}", odjezdy[y].Departure_time.ToString(@"hh\:mm")));
                }
                if(seznamOdjezdu.Count != 0)
                {
                    noveVysledky.Add(new OdjezdyZeStanice($"{stanice[i].Stop_name} {stanice[i].Platform_code}", seznamOdjezdu));
                }
            }

            // Vypsat chybu v případě, že nebyly nalezeny žádné odjezdy
            if(noveVysledky.Count == 0)
            {
                noveVysledky.Add(new OdjezdyZeStanice("Nebyly nalezeny žádné výsledky", new List<Odjezd>()));
            }

            Vysledky = noveVysledky;

            // Přejít na okno s výsledky
            await Device.InvokeOnMainThreadAsync(() => Shell.Current.GoToAsync(nameof(Views.VysledkyOdjezduPage)));

            // Uložit dotaz do historie a aktualizovat seznam
            await Database.UlozitOdjezd(new HistorieOdjezdu(ZeZastavky.Stop_id, ZeZastavky.Stop_name, Datum, Cas));
            await Task.Run(() => AktualizovatHistorii());

            // Vypnout indikaci aktivity
            Aktivita = false;
        }
    }
}
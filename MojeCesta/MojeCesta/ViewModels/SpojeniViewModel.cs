using System;
using System.Linq;
using System.ComponentModel;

using Xamarin.Forms;
using MojeCesta.Models;
using MojeCesta.Services;
using System.Windows.Input;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MojeCesta.ViewModels
{
    public class SpojeniViewModel : INotifyPropertyChanged
    {
        public SpojeniViewModel()
        {
            Hledat = new Command(() => NajitSpojeni());
            Task.Run(() => AktualizovatHistorii());
        }

        private string zeZastavky = string.Empty;
        private string naZastavku = string.Empty;
        private DateTime datum;
        private TimeSpan cas;
        private bool odjezd;
        private bool aktivita;
        private int pocetPrestupu;
        private List<HistorieSpojeni> historie;

        public string ZeZastavky
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

        public string NaZastavku
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
                if(PocetPrestupu == 1)
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
            get => GlobalniPromenne.VysledkySpojeni;
            set
            {
                if (GlobalniPromenne.VysledkySpojeni == value)
                    return;

                GlobalniPromenne.VysledkySpojeni = value;
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

            // TODO: dodělat
            Stop[] stanice = Database.NajitZastavky(ZeZastavky).Result;

            List<Trip> spoje = new List<Trip>();

            // Najít všechny nástupiště zvolené stanice
            for (int i = 0; i < stanice.Length; i++)
            {
                Stop_time[] odjezdy = await Database.NajitOdjezdy(stanice[i], Cas);

                // Najít odjezdy z vybraného nástupiště
                for (int y = 0; y < odjezdy.Length; y++)
                {
                    spoje.Add(await Database.NajitSpoj(odjezdy[y].Trip_id));
                }

            }

            // Přejít na okno s výsledky
            await Device.InvokeOnMainThreadAsync(() => Shell.Current.GoToAsync(nameof(Views.VysledkySpojeniPage)));

            // Uložit dotaz do historie a aktualizovat seznam
            await Database.UlozitSpojeni(new HistorieSpojeni(ZeZastavky, NaZastavku, PocetPrestupu, Datum, Cas));
            await Task.Run(() => AktualizovatHistorii());

            // Vypnout indikaci aktivity
            Aktivita = false;
        }
    }
}
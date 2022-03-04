using System;
using System.Linq;
using System.ComponentModel;

using Xamarin.Forms;
using MojeCesta.Models;
using MojeCesta.Services;
using System.Windows.Input;
using System.Collections.Generic;

namespace MojeCesta.ViewModels
{
    public class SpojeniViewModel : INotifyPropertyChanged
    {
        public SpojeniViewModel()
        {
            Hledat = new Command(() => NajitSpojeni());
        }

        private string zeZastavky = string.Empty;
        private string naZastavku = string.Empty;
        private DateTime datum;
        private TimeSpan cas;
        private bool odjezd;
        private int pocetPrestupu;
        private Tuple<string, string>[] historie;
        private List<OdjezdyZeStanice> vysledky;

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
                return $"Maximum {PocetPrestupu} přestupů";
            }
        }

        public Tuple<string,string>[] Historie 
        {
            get
            {
                //AktualizovatHistorii();
                return historie;
            }
            private set { historie = value; }
        }

        public List<OdjezdyZeStanice> Vysledky
        {
            get => vysledky;
            set
            {
                if (vysledky == value)
                    return;

                vysledky = value;
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
            List<HistorieSpojeni> spojeni = await Database.NacistSpojeni();

            if (spojeni != null)
            {
                Tuple<string, string>[] historie = new Tuple<string, string>[spojeni.Count];

                for (int i = 0; i < historie.Length; i++)
                {
                    historie[i] = new Tuple<string, string>(spojeni[i].ZeZastavky, spojeni[i].NaZastavku);
                }

                Historie = historie;
            }
        }

        public async void NajitSpojeni()
        {
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


        }
    }
}
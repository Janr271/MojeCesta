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
    public class OdjezdyViewModel : INotifyPropertyChanged
    {
        public OdjezdyViewModel()
        {
            Hledat = new Command(() => NajitOdjezdy());
        }

        private string zeZastavky = string.Empty;
        private DateTime datum;
        private TimeSpan cas;
        private string[] historie;
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
        public string[] Historie 
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
            List<HistorieOdjezdu> odjezdy = await Database.NacistOdjezdy();

            if(odjezdy != null)
            {
                Historie = odjezdy.Select(a => a.ZeZastavky).ToArray();
            }
        }

        public async void NajitOdjezdy()
        {
            Stop[] stanice = Database.NajitZastavky(ZeZastavky).Result;
            List<OdjezdyZeStanice> noveVysledky = new List<OdjezdyZeStanice>();

            // Najít všechny nástupiště zvolené stanice
            for (int i = 0; i < stanice.Length; i++)
            {
                OdjezdyZeStanice novaStanice = new OdjezdyZeStanice($"{stanice[i].Stop_name} {stanice[i].Platform_code}");
                Stop_time[] odjezdy = Database.NajitOdjezdy(stanice[i], Cas).Result;

                // Najít odjezdy z vybraného nástupiště
                for (int y = 0; y < odjezdy.Length; y++)
                {
                    Trip spoj = await Database.NajitSpoj(odjezdy[y].Trip_id);
                    Route linka = await Database.NajitLinku(spoj.Route_id);
                    novaStanice.Add(new Odjezd(linka.Route_short_name, $"směr {spoj.Trip_headsign}", odjezdy[y].Departure_time.ToString("t")));
                }

                noveVysledky.Add(novaStanice);
            }

            Vysledky = noveVysledky;

            // Uložit dotaz do historie
            await Database.UlozitOdjezd(new HistorieOdjezdu(ZeZastavky, Datum, Cas));
            OnPropertyChanged(nameof(Historie));

        }
    }
}